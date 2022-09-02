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
using Factorized.UI.Elements;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

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
        public static event MachineOperation OnMachineUpdate;
        public static event MachineOperation OnMachineProgress;
        public static event MachineOperation OnMachineFinish;

        public event MachineOperation OnPlaceEvent;
        public event MachineOperation OnStartEvent;
        public event MachineOperation OnUpdateEvent;
        public event MachineOperation OnProgressEvent;
        public event MachineOperation OnFinishEvent;

        public abstract int ValidTile {get;}
        public abstract List<MachineProcess> allProcesses{get;}
        public abstract Dictionary<MachineSlotType,int> SlotsComposition{get;}

        protected virtual void OnPlace(){}
        protected virtual void OnStart(){}
        protected virtual void OnUpdate(){}
        protected virtual void OnProgress(){}
        protected virtual bool CanProgress(){return true;}
        protected virtual void OnFinish(){}

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
            Slots = new ();
            foreach(var kvp in SlotsComposition)
            {
                for(int i=0;i<kvp.Value;i++)
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
           //TODO: Test if storing position and getting machine every second is more ergonomic; 
        }
        public override void NetSend(BinaryWriter writer){
            writer.Write(Slots.Count());
            foreach(MachineSlot slot in Slots){
                writer.Write(slot);
            }
            writer.Write(Process);
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
            tag[nameof(Process)] = Process;
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
            if(OnMachineUpdate != null)
            {
                OnMachineUpdate(this);
            }
            if(OnUpdateEvent != null)
            {
                OnUpdateEvent(this);
            }
            OnUpdate();
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
        }
        protected void _Start()
        {
            foreach(var p in allProcesses)
            {
                if(ValidateProcess(p))
                {
                    Process = p;
                    if(OnMachineStart!= null)
                    {
                        OnMachineStart(this);
                    }
                    if(OnStartEvent!= null )
                    {
                        OnStartEvent(this);
                    }
                    OnStart();
                    Start(Process);
                    break;

                }
            }
        }

        private void Start(MachineProcess Process)
        {
            foreach(var item in Process.Consume)
            {
                foreach(var slot in Slots)
                {
                    if(slot.IType ==item.type)
                    {
                        slot.SlotItem.stack -= item.stack;
                        if(slot.stack <= 0)
                        {
                            slot.SlotItem = new Item();
                        }
                        break;
                    }
                }
            }
        }

        protected void _Progress()
        {
            if(!CanProgress())return;
            timer++;
            if(OnMachineProgress!= null){
                OnMachineProgress(this);
            }
            if(OnProgressEvent != null)
            {
                OnProgressEvent(this);
            }
            OnProgress();
        }

        protected void _Finish()
        {
            if(!CanFinish(Process)) return;
            timer = 0;
            Finish(Process);
            if(OnMachineFinish != null)
            {
                OnMachineFinish(this);
            }
            if(OnFinishEvent != null)
            {
                OnFinishEvent(this);
            }
            OnFinish();
            Process = null;
        }
        protected void Finish(MachineProcess Process)
        {
            List<Item> noCopies = new ();
            foreach(var item in Process.Produce)
            {
                Item p = item.Clone();
                for(int i =0; i< GetSlots(MachineSlotType.Output).Count; i++){
                    MachineSlot slot =GetSlots(MachineSlotType.Output)[i];
                    if(slot.IType == p.type){
                        if(slot.stack + p.stack <= slot.maxStack )
                        {
                            slot.SlotItem.stack += p.stack;
                            p.stack = 0;
                        }
                        else
                        {
                            slot.SlotItem.stack = slot.maxStack;
                            p.stack += slot.stack - slot.maxStack;
                        }
                        if(p.stack > 0)
                        {
                            noCopies.Add(p);
                        }
                    }
                }
            }
            foreach (var item in noCopies)
            {
                for(int i = 0; i< GetSlots(MachineSlotType.Output).Count();i++)
                {
                    if(GetSlots(MachineSlotType.Output)[i].IsAir)
                    {
                        GetSlots(MachineSlotType.Output)[i].SlotItem = item;
                        break;
                    }
                }
            }
        }
        protected virtual bool CanFinish(MachineProcess Process){
            int freeSlots = 0;
            foreach(var slot in GetSlots(MachineSlotType.Output))
            {
                if(Process.Produce.Exists(
                    item => item.type == slot.IType && slot.stack + item.stack <= slot.maxStack)
                    || slot.IsAir){
                    freeSlots++;
                }
            }
            return freeSlots >= Process.Produce.Count();
        }

        protected virtual bool ValidateProcess(MachineProcess process)
        {
            return hasItems(process);
        }
        protected bool hasItems(MachineProcess Process)
        {
            bool hasItems = true;
            foreach (var item in Process.Consume)
            {
                hasItems = hasItems && Slots.Exists(slot => slot.IType == item.type && slot.stack >= item.stack);
            }
            return hasItems;
        }
        
        public List<MachineSlot> GetSlots(MachineSlotType type)
        {
            List<MachineSlot> r = new ();
            foreach (var entry in Slots)
            {
                if(entry.Type == type) r.Add(entry);
            }
            return r;
        }
        public List<MachineSlot> GetSlots()
        {
            return Slots;
        }

        public bool TryAddItemToSlot(MachineSlotType slotType, int slotNumber,Item myItem)
        {
            List<MachineSlot> slots = GetSlots(slotType);
            if(slotNumber >= 0 && slotNumber< slots.Count())
            {
                if(slots[slotNumber].IsAir)
                {
                    slots[slotNumber].SlotItem = myItem;
                    return true;
                }
            }
            return false;
        }
        public Factorized.ItemReferrer InputSlotRef(int i) 
        {
            return GetSlots(MachineSlotType.Input)[i].GetRef();
        }
        public Factorized.ItemReferrer OutputSlotRef(int i)
        {
            return GetSlots(MachineSlotType.Output)[i].GetRef();
        }
        public List<Item> GetItems()
        {
            IEnumerable<Item> list = from s in Slots
                                select s.SlotItem;
            return list.ToList();
        }
        public List<Item>GetItems(MachineSlotType type)
        {
            List<Item> list = (List<Item>)from s in GetSlots(type)
                              select s.SlotItem;
            return list;
        }

        public void TryAddItemToSlot(int slotNumber, Item myItem)
        {
            MachineSlot slot = Slots[slotNumber];
            if(slot.IType == myItem.type|| myItem.IsAir || slot.IsAir)
            {
                slot.SlotItem = myItem;
            }
        }
        public void GenerateUI(MachineUI UI)
        {
            UIPanel inputPanel = new ();
            inputPanel.Width.Set(300,0f);
            inputPanel.Height.Set(150,0f);
            inputPanel.VAlign = 0.39f;
            inputPanel.HAlign = 0.1f;

            GenerateSlotGroup(inputPanel,MachineSlotType.Input,ItemSlot.Context.InventoryItem);
            UI.Append(inputPanel);

            UIPanel outputPanel = new ();
            outputPanel = new UIPanel();
            outputPanel.Width.Set(300,0);
            outputPanel.Height.Set(150,0);
            outputPanel.HAlign = 0.1f;
            outputPanel.VAlign = 0.88f;
            GenerateSlotGroup(outputPanel,MachineSlotType.Output,ItemSlot.Context.ChestItem);
            UI.Append(outputPanel);

            UIPanel processingPanel = new ();
            processingPanel = new UIPanel();
            processingPanel.Width.Set(150,0f);
            processingPanel.Height.Set(200,0f);
            processingPanel.HAlign = 0.2f;
            processingPanel.VAlign = 0.65f;

            FProgressBar progress = new (UI.fullFire,UI.emptyFire,
                    () =>{if(Process != null) return timer ; else return 0;},
                    () =>{ if(Process != null) return Process.ProcessingTime; else return 1;});
            processingPanel.Append(progress);
            UI.Append(processingPanel);
        }

        public void GenerateSlotGroup(UIPanel inputPanel, MachineSlotType type, int context)
        {
            List<MachineSlot> machineSlots = GetSlots(type);
            for(int i = 0; i < machineSlots.Count; i++)
            {
                float padding = 10f;
                float sizeWithPadding = 50f;
                int rowFit = (int)(inputPanel.Width.Pixels / sizeWithPadding);
                int colunmFit = (int)(inputPanel.Height.Pixels/ sizeWithPadding);
                if(rowFit * colunmFit < machineSlots.Count)
                {
                    Factorized.mod.Logger.Warn("Created Machine UI with missing elements as they couldn't fit");
                }
                float hStart = padding/inputPanel.Width.Pixels;
                float vStart = padding/inputPanel.Height.Pixels;
                float hStep = sizeWithPadding/inputPanel.Width.Pixels;
                float vStep = sizeWithPadding/inputPanel.Height.Pixels;
                int colunm = i / rowFit;
                int row = i % rowFit;
                FItemSlot slot = new ( GetSlotUpdated(i,type),context);
                slot.Height.Set(sizeWithPadding - 2*padding,0f);
                slot.Width.Set(sizeWithPadding - 2*padding,0f);
                slot.HAlign = hStart + row * hStep;
                slot.VAlign = vStart + colunm * vStep;
                inputPanel.Append(slot);
            }
        }
        public Func<MachineSlot> GetSlotUpdated(int i, MachineSlotType type)
        {
            return () => {
                TileEntity test = TileEntity.ByPosition[Position];
                if(test is MachineTE)
                {
                    MachineTE newTE = (MachineTE)test;
                    return newTE.GetSlots(type)[i];
                }else
                {
                    return GetSlots(type)[i];
                }
            };
        }
    }
}
