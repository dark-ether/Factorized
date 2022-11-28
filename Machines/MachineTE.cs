using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Reflection;
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
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Factorized.Machines{
    public delegate bool MachineValidation(MachineTE target);
    public class PersistentAttribute : Attribute
    {
    }
    
    public class MachineSlot : TagSerializable {
      public Item item;
      public int type {get => item.type ; set => item.type = value;}
      public int stack {get => item.stack; set => item.stack = value;}
      public int maxStack {get => item.maxStack; set => item.maxStack = value;}
      public bool IsAir {get => item.IsAir;}
      public MachineSlot() {
        item = new Item ();
      }
      public ItemReferrer itemRef() {
        return () => ref item;
      }
      public static readonly Func<TagCompound,MachineSlot> DESERIALIZER = Load;
      public TagCompound SerializeData()
      {
            return new TagCompound {["item"] = item};
      }
      public static MachineSlot Load(TagCompound tag) {
        MachineSlot t = new ();
        t.item = tag.Get<Item>("item");
        return t;
      }
    }
    /// <summary>
    /// Basic machine type,when inheriting from it, 
    /// mark any field necessary to behave properly with the PersistentAttribute
    /// </summary>
    public abstract class MachineTE : ModTileEntity
    {
        public int Height{
            get{return TileObjectData.GetTileData(ValidTile,0).Height;}
        }
        public Point16 MouseRelativePlacePosition{
            get{return TileObjectData.GetTileData(ValidTile,0).Origin;}
        }
        public int Width{
            get{return TileObjectData.GetTileData(ValidTile,0).Width;}
        }

        public static event Action<MachineTE> OnMachinePlace;
        public static event Action<MachineTE> OnMachineStart;
        public static event Action<MachineTE> OnMachinePreUpdate;
        public static event Action<MachineTE> OnMachinePostUpdate;
        public static event Action<MachineTE> OnMachineProgress;
        public static event Action<MachineTE> OnMachineFinish;

        public event Action<MachineTE> OnPlaceEvent;
        public event Action<MachineTE> OnStartEvent;
        public event Action<MachineTE> OnPreUpdateEvent;
        public event Action<MachineTE> OnPostUpdateEvent;
        public event Action<MachineTE> OnProgressEvent;
        public event Action<MachineTE> OnFinishEvent;
        public static Dictionary<Type,List<TagCompound>> allProcesses;
        [Persistent]
        protected TagCompound currentProcess;
        [Persistent]
        public int timer {get; protected set;}
        [Persistent]
        public MachineSlot[] inputSlots;
        [Persistent]
        public MachineSlot[] outputSlots;

        public abstract int ValidTile {get;}
        public abstract List<TagCompound> setupMachineProcesses();
        public abstract int inputSlotsNumber {get;}
        public abstract int outputSlotsNumber {get;}
        protected virtual void OnPlace(){}
        protected virtual void OnStart(){}
        protected virtual void OnPreUpdate(){}
        protected virtual void OnPostUpdate(){}
        protected virtual void OnProgress(){}
        protected virtual bool CanProgress(){return true;}
        protected virtual void OnFinish(){}

        public MachineTE()
        {
            inputSlots = new MachineSlot[inputSlotsNumber];
            for (int i = 0; i < inputSlots.Length; i++)
            {
                inputSlots[i] = new ();
            }
            outputSlots = new MachineSlot [outputSlotsNumber];
            for (int i = 0; i < outputSlots.Length; i++)
            {
                inputSlots[i] = new ();
            }
        }

        protected void _Place()
        {
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

        public override void Load(){
          allProcesses = new ();
        }
        
        public override void LoadData(TagCompound tag)
        {
          var fields = this.GetType().GetFieldsIwA<PersistentAttribute>();
          fields
            .OrderBy(field => field.Name)
            .ToList()
            .ForEach(field => {
                if (tag.ContainsKey(field.Name)) field.SetValue(this,tag[field.Name]);
                }
            );
        
        }

        public override void SaveData(TagCompound tag){
          var fields = this.GetType().GetFieldsIwA<PersistentAttribute>();
          fields
            .OrderBy(field => field.Name)
            .ToList()
            .ForEach(field => tag[field.Name] = field.GetValue(this));
        }
        
        public override void NetReceive(BinaryReader reader){
          TagCompound tag = TagIO.Read(reader);
          var fields = this.GetType().GetFieldsIwA<PersistentAttribute>();
          fields
            .OrderBy(field => field.Name)
            .ToList()
            .ForEach( field => field.SetValue(this,tag[field.Name]));
        }
        public override void NetSend(BinaryWriter writer){
          TagCompound tag = new ();
          var fields = this.GetType().GetFieldsIwA<PersistentAttribute>();
          fields
            .OrderBy(field => field.Name)
            .ToList()
            .ForEach( field => tag[field.Name] = field.GetValue(this));
          TagIO.Write(tag,writer);
        }
        
        public override void OnNetPlace(){
           if(Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
        }

        public override sealed void Update(){
            if(OnMachinePreUpdate != null)
            {
                OnMachinePreUpdate(this);
            }
            if(OnPreUpdateEvent != null)
            {
                OnPreUpdateEvent(this);
            }
            OnPreUpdate();
            if(currentProcess == null)
            {
                _Start();
                if(currentProcess != null)
                {
                    _Progress();
                }
            } else
            {
                if(currentProcess.GetInt("time") < timer)
                {
                    _Finish();
                }else
                {
                    _Progress();
                }
            }
            if(OnMachinePostUpdate != null)
            {
                OnMachinePostUpdate(this);
            }
            if(OnPostUpdateEvent != null)
            {
                OnPostUpdateEvent(this);
            }
            OnPostUpdate();
            //SMH.UpdateSend(Position);
            //NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
        }
        protected void _Start()
        {
            void removeItems(List<(int,int)> toRemove) {
                foreach (var item in toRemove)
                {
                    foreach (var inputSlot in inputSlots) {
                      var inputItem = inputSlot.item;
                        if (inputItem.type != item.Item1) continue;
                        if (inputItem.stack <= item.Item2) inputItem.TurnToAir();
                        else {
                            inputItem.stack -= item.Item2;
                            break;
                        }
                    }
                }
            }
            Type mt = this.GetType();
            if (!allProcesses.ContainsKey(mt)) allProcesses[mt] = setupMachineProcesses();
            foreach(var p in allProcesses[mt])
            {
                if(canChooseProcess(p)){
                    currentProcess = (TagCompound) p.Clone();
                    if(OnMachineStart!= null)
                    {
                        OnMachineStart(this);
                    }
                    if(OnStartEvent!= null )
                    {
                        OnStartEvent(this);
                    }
                    OnStart();
                    removeItems(currentProcess.Get<List<(int,int)>>("consumed"));
                    break;
                }
            }
        }

        public virtual bool canChooseProcess(TagCompound p)
        {
            bool hi = true;
            // int,int means type,quantity
            foreach (var recipeItem in (List<(int,int)>)p["consumed"]) {
                int found = 0;
                foreach (var itemSlot in inputSlots) {
                    var item = itemSlot.item;
                    if (item.type == recipeItem.Item1) found += item.stack;
                }
                if (found < recipeItem.Item2) return false;
            }
            return hi;
        }

        protected void _Progress()
        {
            if(!CanProgress()) return;
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
            if(!CanFinish()) return;
            Finish();
            if(OnMachineFinish != null)
            {
                OnMachineFinish(this);
            }
            if(OnFinishEvent != null)
            {
                OnFinishEvent(this);
            }
            OnFinish();
            currentProcess = null;
            timer = 0;
        }

        protected bool CanFinish() {
            bool hasSpace = true;
            // int int means type, quantity
            foreach (var pItem in currentProcess.Get<List<(int,int)>>("produced")) {
                int stillNeeds = pItem.Item2;
                foreach (var oItemSlot in outputSlots){
                    var oItem = oItemSlot.item;
                    stillNeeds = oItem switch {
                        var x when x.type == pItem.Item1 => x.maxStack < x.stack + stillNeeds 
                          ? stillNeeds - x.maxStack + x.stack : 0,
                        _ => stillNeeds,
                    };
                }
                if (stillNeeds > 0) return false;
            }
            return hasSpace;
        }


        protected void Finish()
        {
            /// precondition: it has enough space if it doesn't it will discard some Items
            void addItems(List<(int,int)> pItems) 
            {
                foreach(var pItem in pItems) {
                    int stillHas = pItem.Item2;
                    for( int i =0; i< outputSlots.Count(); i++) {
                        if (stillHas <= 0) break;
                        if (outputSlots[i].type == pItem.Item1){
                          if (stillHas + outputSlots[i].stack <= outputSlots[i].maxStack) 
                          {
                            stillHas = 0;
                            outputSlots[i].stack += stillHas;
                          } else {
                            stillHas = stillHas - outputSlots[i].maxStack + outputSlots[i].stack;
                            outputSlots[i].stack = outputSlots[i].maxStack;
                          }
                        }
                        if (outputSlots[i].IsAir) {
                          outputSlots[i].item = new Item (pItem.Item1,pItem.Item2);
                        }
                    }
                }
            }
            addItems(currentProcess.Get<List<(int,int)>>("produce"));
            timer = 0;
            currentProcess = null;
        }
        
        public virtual IEnumerable<Item> GetItems() {
            return from slot in inputSlots.Concat(outputSlots)
              select slot.item;
        }
        public ItemReferrer InputSlotRef(int i) 
        {
            return inputSlots[i].itemRef();
        }
        public ItemReferrer OutputSlotRef(int i)
        {
            return outputSlots[i].itemRef();
        }

        public virtual void GenerateUI(UIState UI)
        {
            
        }

        public Func<int> GetTimerUpdated()
        {
            return () => 
            {
                TileEntity newTE; 
                TileEntity.ByPosition.TryGetValue(Position, out newTE);
                if(newTE == null) return 0;
                if(!(newTE is MachineTE)) return 0;
                if(((MachineTE)newTE).currentProcess == null) return 0;
                else {
                 return ((MachineTE)newTE).timer;
                }
            };
        }
        public Func<int> GetLimitUpdated()
        {
            return () => 
            {
                TileEntity newTE;
                TileEntity.ByPosition.TryGetValue(Position,out newTE);
                if(newTE == null) return 1;
                if(!(newTE is MachineTE)) return 1;
                MachineTE m = (MachineTE)newTE;
                if(m.currentProcess == null) return 1;
                else return (int)m.currentProcess["duration"];
            };
        }
        public static MachineTE Get(int id)
        {
            TileEntity te;
            TileEntity.ByID.TryGetValue(id,out te);
            if(te == null) return null;
            if(!(te is MachineTE)) return null;
            return (MachineTE) te;
        }
        public static MachineTE Get(Point16 pos)
        {
            TileEntity te;
            TileEntity.ByPosition.TryGetValue(pos,out te);
            if(te == null) return null;
            if(!(te is MachineTE)) return null;
            return (MachineTE) te;
        }
        
    }

}
