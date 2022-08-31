using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.ID;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using System.Linq;
using System.IO;
using Factorized.Utility;
using Factorized.Net;
using Terraria.ObjectData;
using Factorized.UI;

namespace Factorized.TE.MachineTE{
    public abstract class BaseMachineTE : ModTileEntity
    {
        public Item[] inputSlots {get; protected set;}
        public Item[] outputSlots {get; protected set;}
        public MachineState machineState;
        //TODO: change process to not have a dictionary per machine
        protected Dictionary<MachineInput,MachineOutput> ProcessIO;
        
        public virtual int Height{
            get{return TileObjectData.GetTileData(ValidTile,0).Height;}
        }
        public virtual Point16 MouseRelativePlacePosition{
            get{return TileObjectData.GetTileData(ValidTile,0).Origin;}
        }
        public virtual int NumberOfInputSlots{get;} = 3;
        public virtual int NumberOfOutputSlots{get;} = 2;
        public virtual int Width{
            get{return TileObjectData.GetTileData(ValidTile,0).Width;}
        }

        protected virtual void onPlace(){}
        protected virtual void onProcessFailure(MachineOutput process){}                            
        protected virtual void onProcessNotFound(){}
        protected virtual void onProcessSuccess(MachineInput inputConsumed,MachineOutput outputCreated){}
        protected virtual void onUpdate(){}
        protected virtual bool canProgress(MachineOutput output){return true;}
        protected virtual void onProgress(MachineOutput output){}
        protected virtual void onProcessSuccess(MachineOutput output){}
        protected virtual void onProgressFailure(MachineOutput output){}
        protected virtual void onUnableToComplete(MachineOutput output){}
        protected virtual void onProgressFailure(){}
        protected virtual void onProgress(){}

        public abstract int ValidTile {get;}
        protected abstract void setupProcessIO();

        public BaseMachineTE()
        {
        }

        protected void setup()
        {
            basicSetup();
            onPlace();
            setupProcessIO();
        }

        protected void basicSetup(){
            inputSlots = new Item[NumberOfInputSlots];
            for(int i = 0 ; i < NumberOfInputSlots; i++){
              inputSlots[i]= new Item();
            }
            outputSlots = new Item[NumberOfOutputSlots];
            for(int i = 0 ; i < NumberOfOutputSlots; i++){
              outputSlots[i] = new Item();
            }
            machineState = new ();
            for(int i = 0; i < inputSlots.Length; i++){
                machineState.inputSlotsMetadata.Add("input");
            }
            for(int i = 0; i < outputSlots.Length; i++){
                machineState.outputSlotsMetadata.Add("output");
            }
        }
        
        protected virtual void onProcessFound(MachineOutput foundProcess){
            foundProcess.consumeItems(inputSlots);
        }

        protected virtual void updateMachineState(MachineOutput change){
            change.incrementCounters(machineState);
            change.incrementValues(machineState);
            change.setProperties(machineState);
            change.produceItems(outputSlots);
            machineState.currentProcess = null;
            machineState.timer = 0;
        }
        
        protected virtual MachineOutput getProcess(){
            MachineOutput firstFound = null;
            foreach(var processInputPair in ProcessIO)
            {
                if(processInputPair.Key.isCompatible(inputSlots,machineState))
                {   
                    firstFound = processInputPair.Value;
                    break;
                }
            }
            return firstFound;
        }

        public sealed override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate){
            
            if(Main.netMode == NetmodeID.MultiplayerClient){
                int height = Height;
                int width = Width;
                
                NetMessage.SendTileSquare(Main.myPlayer,i,j,width,height);
                
                NetMessage.SendData(MessageID.TileEntityPlacement,-1,-1,null,i,j,Type);
            }
            Point16 tileOrigin = MouseRelativePlacePosition;
            int placedEntity = Place(i - tileOrigin.X, j - tileOrigin.Y);

            BaseMachineTE placedMachine = (BaseMachineTE)TileEntity.ByID[placedEntity];
            placedMachine.setup();

            return placedEntity;
        }

        public override bool IsTileValidForEntity(int x, int y){
            try{
            Tile tileInPosition = Main.tile[x,y];
            if(tileInPosition.TileType == ValidTile){
                return true;
            }
            }catch(IndexOutOfRangeException){
                ModContent.GetInstance<Factorized>().Logger.Warn("tried to access a tile outside the world");
                return false;
            }
            return false;
        }

        public override void LoadData(TagCompound tag){
            inputSlots = tag.Get<Item[]>("inputSlots");
            outputSlots = tag.Get<Item[]>("outputSlots");
            machineState = new (tag.Get<MachineState>("machineState"));
            setupProcessIO();
        }
        
        public override void NetReceive(BinaryReader reader){
            int length = reader.ReadInt32();
            if(inputSlots == null) inputSlots = new Item[length];
            for(int i = 0; i < length; i++){
                inputSlots[i] = ItemIO.Receive(reader,true);
            }
            length = reader.ReadInt32();
            if(outputSlots == null) outputSlots = new Item[length];
            for(int i = 0; i < length; i++){
                outputSlots[i] = ItemIO.Receive(reader,true);
            }
            machineState = reader.ReadMachineState();
            //this is probably a bad idea, static variables are just another name to global variables
            if(UICaller.machine == null) return;
            UICaller.machine.inputSlots = inputSlots;
            UICaller.machine.outputSlots = outputSlots;
            UICaller.machine.machineState = machineState;
        }
        //TODO: change net send and net receive
        public override void NetSend(BinaryWriter writer){
            writer.Write(inputSlots.Length);
            foreach(Item item in inputSlots){
                ItemIO.Send(item,writer,true);
            }
            writer.Write(outputSlots.Length);
            foreach(Item item in outputSlots){
                ItemIO.Send(item,writer,true);
            }
            writer.Write(machineState); 
        }
        
        public override void OnNetPlace(){
           setup();
           if(Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
        }
        
        public override void SaveData(TagCompound tag){
            tag.Add("inputSlots",inputSlots);
            tag.Add("outputSlots",outputSlots);
            tag.Add("machineState",machineState);
        }

        //TODO: change net messages again
        //TODO: change update so different stacks of the same item aren't consumed simultaneously
        public override void Update(){
            onUpdate();
            if(!machineState.IsProcessing())
            {
                MachineOutput process = getProcess();
                machineState.SetProcess(process);
                if(process != null)
                {
                    onProcessFound(machineState.currentProcess);
                }
                else 
                {
                    onProcessNotFound();
                }
            }
            else
            {
                if(machineState.timer < machineState.currentProcess.processingTime){ 
                    if(canProgress(machineState.currentProcess))
                    {
                        machineState.timer += 1;
                        onProgress();
                    }
                    else
                    {
                        onProgressFailure();
                    }
                }
                else 
                {
                    if(canGenerateOutput(machineState.currentProcess))
                    {
                        updateMachineState(machineState.currentProcess);
                        onProcessSuccess(machineState.currentProcess);
                    }
                    else 
                    {
                        onUnableToComplete(machineState.currentProcess);
                    }
                }
            }
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
        }

        protected virtual bool canGenerateOutput(MachineOutput currentProcess){
            int freeSlots =  0;
            for(int i = 0; i < outputSlots.Length; i++)
            {
                foreach(var outputItem in currentProcess.itemsToAdd)
                {
                    if(outputSlots[i].type == outputItem.Item1)
                    {
                        freeSlots += 1;
                    }
                }
                if(outputSlots[i].IsAir)
                {
                    freeSlots += 1;
                }
            }
            return freeSlots >= currentProcess.itemsToAdd.Count();
        }

        public virtual bool hasItems()
        {
            bool foundItem = false;
            foreach(var item in outputSlots)
            {
                if (!item.IsAir)
                {
                    foundItem = true;
                }
            }            
            foreach(var item in inputSlots)
            {
                if (!item.IsAir)
                {
                    foundItem = true;
                }
            }
            return foundItem;
        }

        public bool TryAddItemToSlot(MachineSlotType slotType, int slotNumber,Item myItem)
        {
            switch (slotType) {
                case MachineSlotType.InputSlot: 
                    if (inputSlots[slotNumber].IsAir){
                        inputSlots[slotNumber] = myItem;
                        return true;
                    }
                    else if (inputSlots[slotNumber].type == myItem.type || myItem.IsAir){
                        inputSlots[slotNumber] = myItem;
                        return true;
                    }
                    else {
                        return false;
                    }
                case MachineSlotType.OutputSlot:
                    if (outputSlots[slotNumber].IsAir){
                        outputSlots[slotNumber] = myItem;
                        return true;
                    }
                    else if (outputSlots[slotNumber].type == myItem.type || myItem.IsAir){
                        outputSlots[slotNumber] = myItem;
                        return true;
                    }
                    else {
                        return false;
                    }
                default: return false;
            }
        }
        public Factorized.ItemReferrer InputSlotRef(int i) 
        {
            return () => {return ref inputSlots[i];};
        }
        public Factorized.ItemReferrer OutputSlotRef(int i)
        {
            return () => {return ref outputSlots[i];};
        }
    }
}
