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
using Factorized;

namespace Factorized.TE.MachineTE{
    public abstract class MachineTE : ModTileEntity
    {
        public Item[] inputSlots {get; protected set;}
        public Item[] outputSlots {get; protected set;}
        public MachineState machineState;
        protected Dictionary<MachineInput,MachineOutput> ProcessIO;
        
        public virtual int Height{get;} = 2;
        public virtual Point16 MouseRelativePlacePosition{get;} = new Point16(0,0);
        // point of origin on tile use the same as newTile.origin
        public virtual int NumberOfInputSlots{get;} = 3;
        public virtual int NumberOfOutputSlots{get;} = 2;
        public virtual int Width{get;} = 2;
        
        protected virtual void onPlace(int x,int y){}
        protected virtual void onProcessFailure(MachineOutput process){}                            
        protected virtual void onProcessNotFound(){}
        protected virtual void onProcessSuccess(MachineInput inputConsumed,MachineOutput outputCreated){}
        protected virtual void onUpdate(){}
        protected virtual void machineSpecificSetup(){}
        protected virtual bool canProgress(MachineOutput output){return true;}
        protected virtual void onProgress(MachineOutput output){}
        protected virtual void onProcessSuccess(MachineOutput output){}
        protected virtual void onProgressFailure(MachineOutput output){}
        protected virtual void onUnableToComplete(MachineOutput output){}
        protected virtual void onProgressFailure(){}
        protected virtual void onProgress(){}

        public abstract int ValidTile {get;}
        protected abstract void setupProcessIO();

        protected void basicSetup(){
            inputSlots = new Item[NumberOfInputSlots];
            for(int i = 0 ; i<NumberOfInputSlots; i++){
              inputSlots[i]= new Item();
            }
            outputSlots = new Item[NumberOfOutputSlots];
            for(int i = 0 ; i < NumberOfOutputSlots; i++){
              outputSlots[i] = new Item();
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
            ModContent.GetInstance<Factorized>().Logger.Info("updateMachineState was ran");
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

            MachineTE placedMachine = (MachineTE)TileEntity.ByID[placedEntity];
            placedMachine.basicSetup();
            placedMachine.machineSpecificSetup();
            placedMachine.onPlace(i - tileOrigin.X,j - tileOrigin.Y);
            placedMachine.setupProcessIO();

            return placedEntity;
        }

        public override bool IsTileValidForEntity(int x, int y){
            try{
            Tile tileInPosition = Main.tile[x,y];
            if(tileInPosition.TileType == ValidTile){
                return true;
            }
            }catch(IndexOutOfRangeException){
                ModContent.GetInstance<Factorized>().Logger.Warn("tried to acess a tile outside the world");
                return false;
            }
            return false;
        }

        public override void LoadData(TagCompound tag){
            inputSlots = tag.Get<List<Item>>("inputSlots").ToArray();
            outputSlots = tag.Get<List<Item>>("outputSlots").ToArray();
            machineState = tag.Get<MachineState>("machineState");
            setupProcessIO();
        }
        
        public override void NetReceive(BinaryReader reader){
            int length = reader.ReadInt32();
            for(int i = 0; i < length; i++){
                int type = reader.ReadInt32();
                int stack = reader.ReadInt32();
                inputSlots[i] = new Item(type,stack);
            }
            length = reader.ReadInt32();
            for(int i = 0; i < length; i++){
                int type = reader.ReadInt32();
                int stack = reader.ReadInt32();
                outputSlots[i] = new Item(type,stack);
            }
            machineState = reader.ReadMachineState();
        }
        //todo change net send and net receive
        public override void NetSend(BinaryWriter writer){
            writer.Write(inputSlots.Length);
            foreach(Item item in inputSlots){
                writer.Write(item.type);
                writer.Write(item.stack);
            }
            writer.Write(outputSlots.Length);
            foreach(Item item in outputSlots){
                writer.Write(item.type);
                writer.Write(item.stack);
            }
            writer.Write(machineState);
            
        }
        
        public override void OnNetPlace(){
           if(Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
        }
        
        public override void SaveData(TagCompound tag){
            tag.Set("inputSlots",inputSlots.ToList());
            tag.Set("outputSlots",outputSlots.ToList());
            tag.Set("machineState",machineState);
        }
        //todo : change net messages again

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
                if(machineState.timer != machineState.timerLimit){ 
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
    }
}
