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

namespace Factorized.Machines{
    public delegate void MachineOperation(MachineTE target);
    public delegate bool MachineValidation(MachineTE target);
    /// <summary>
    /// use this to create a machine
    /// </summary>
    public abstract class MachineTE : ModTileEntity
    {
        protected List<MachineSlot> Slots {get; set;}
        protected MachineProcess Process {get; set;}
        public int timer{
            get;
            protected set;
        }
        public int Height{
            get{return TileObjectData.GetTileData(ValidTile,0).Height;}
        }
        public Point16 MouseRelativePlacePosition{
            get{return TileObjectData.GetTileData(ValidTile,0).Origin;}
        }
        public int Width{
            get{return TileObjectData.GetTileData(ValidTile,0).Width;}
        }

        public static event MachineOperation OnMachinePlace;
        public static event MachineOperation OnMachineStart;
        public static event MachineOperation OnMachineProgress;
        public static event MachineOperation OnMachineFinish;

        public event MachineOperation OnPlaceEvent;
        public event MachineOperation OnStartEvent;
        public event MachineOperation OnProgressEvent;
        public event MachineOperation OnFinishEvent;

        public abstract int ValidTile {get;}
        public abstract List<MachineProcess> allProcesses{get;}
        public abstract Dictionary<MachineSlotType,int> SlotsComposition{get; protected set;}

        protected virtual void OnPlace(){}
        protected virtual void OnStart(){}
        protected virtual void OnProgress(){}
        protected virtual bool CanProgress(){return true;}
        protected virtual void OnFinish(){}
        protected virtual bool CanFinish(){return true;}

        public MachineTE()
        {
        }

        protected void _Place()
        {
            _Setup();
            if(OnMachinePlace != null)
            {
                OnMachinePlace(this);
            }
            if(OnPlaceEvent != null)
            {
                OnPlaceEvent(this);
            }
            OnPlace();
        }
        protected virtual void _Setup()
        {
            foreach(var kvp in SlotsComposition)
            {
                for(int i=0;i<=kvp.Value;i++)
                {
                    Slots.Add(new MachineSlot(kvp.Key));
                }
            }
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
            placedMachine._Place();

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
            if (tag.ContainsKey(nameof(Slots)))
            {
                Slots = tag.GetList<MachineSlot>(nameof(Slots)).ToList<MachineSlot>();
            } else _Setup();
            if (tag.ContainsKey(nameof(Process)))
            {
                Process = tag.Get<MachineProcess>(nameof(Process));
            } else Process = null;
        }
        
        public override void NetReceive(BinaryReader reader){
            _Setup();
            int length = reader.ReadInt32();
            for(int i = 0; i< length;i++)
            {
                Slots[i] = reader.ReadMachineSlot();
            }
            reader.ReadMachineProcess();
            // add things to update the ui machine again
            if(UICaller.machine == null) return;
        }
        //TODO: change net send and net receive
        public override void NetSend(BinaryWriter writer){
            writer.Write(Slots.Count());
            foreach(MachineSlot slot in Slots){
                writer.Write(slot);
            }
            write.Write(Process);
        }
        
        public override void OnNetPlace(){
           _Setup();
           if(Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
        }
        
        public override void SaveData(TagCompound tag){
            tag[nameof(Slots)] = Slots;
        }

        public override sealed void Update(){
            if(Process == null)
            {
                _Start();
                if(Process != null)
                {
                    _Progress();
                }
            } else
            {
                if(Process.ProcessingTime < timer)
                {
                    _Finish();
                }else
                {
                    _Progress();
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
                    if(outputSlots[i].type == outputItem.Item1 && outputSlots[i].stack +outputItem.Item2 <= outputSlots[i].maxStack)
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
                case MachineSlotType.Input: 
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
                case MachineSlotType.Output:
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
