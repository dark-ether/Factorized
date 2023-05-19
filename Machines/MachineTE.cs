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

namespace Factorized.Machines {
  /// <summary>
  /// texto
  /// </summary>
  [System.AttributeUsage(System.AttributeTargets.Field)]
  public class MachineDataAttribute : Attribute
  {
  }
  public class MachineSlot : TagSerializable {
    public Item item;
    public int type {
      get => item.type ;
      set => item.type = value;
    }
    public int stack {
      get => item.stack;
      set => item.stack = value;
    }
    public int maxStack {
      get => item.maxStack;
      set => item.maxStack = value;
    }
    public bool IsAir {
      get => item.IsAir;
    }
    public MachineSlot() {
      item = new Item ();
    }
    public ItemReferrer itemRef() {
      return () => ref item;
    }
    public static readonly Func<TagCompound,MachineSlot> DESERIALIZER = Load;
    public TagCompound SerializeData()
    {
      return new TagCompound {
        ["item"] = item
      };
    }
    public static MachineSlot Load(TagCompound tag) {
      MachineSlot t = new ();
      t.item = tag.Get<Item>("item");
      return t;
    }
  }

  /// <remarks>
  /// basic machine capable of simulating a process
  /// </remarks>
  public abstract class MachineTE : ModTileEntity
  {
    public enum State {Idle, Working, Halted, Frozen};
    //TODO: what was the zero for?
    public int Height {
      get => TileObjectData.GetTileData(ValidTile,0).Height;
    }
    public Point16 MouseRelativePlacePosition {
      get => TileObjectData.GetTileData(ValidTile,0).Origin;
    }
    public int Width {
      get => TileObjectData.GetTileData(ValidTile,0).Width;
    }
    public static event Action<MachineTE> OnMachinePlace;
    public static event Action<MachineTE> OnMachinePreUpdate;
    public static event Action<MachineTE> OnMachineUpdate;
    public static event Action<MachineTE> OnMachinePostUpdate;
    public static event Action<MachineTE> OnMachineCounterReset;
    public static event Action<MachineTE> OnMachineStart;
    public static event Action<MachineTE> OnMachineWorking;
    public static event Action<MachineTE> OnMachineFinish;
    public static event Action<MachineTE> OnMachineHalt;
    public static event Action<MachineTE> OnMachineRestart;

    public event Action<MachineTE> OnPlaceEvent;
    public event Action<MachineTE> OnPreUpdateEvent;
    public event Action<MachineTE> OnUpdateEvent;
    public event Action<MachineTE> OnPostUpdateEvent;
    public event Action<MachineTE> OnCounterResetEvent;
    public event Action<MachineTE> OnStartEvent;
    public event Action<MachineTE> OnWorkingEvent;
    public event Action<MachineTE> OnFinishEvent;
    public event Action<MachineTE> OnHaltEvent;
    public event Action<MachineTE> OnRestartEvent;

    /// <remarks>
    /// you should call ModContent.Tiletype here and do nothing else
    /// </remarks>
    public abstract int ValidTile {
      get;
    }
    /// <remarks>
    /// a simple counter which increases each time the machine updates while working
    /// DOES NOT reset On Finish.
    /// </remarks>
    [field: MachineData]
    public int counter {get; private set;} = 0;
    /// <remarks>
    /// current State of the machine is used in the update method to control 
    /// whether the machine is working and if so apply the corresponding logic
    /// </remarks>
    [field: MachineData]
    public State state {get; private set;} = State.Idle;
    /// <remarks>
    /// stores the previous state for unfreezing correctly
    /// </remarks>
    [field: MachineData]
    private State prev {get; set;} = State.Idle;

    protected virtual void OnPlace(){}
    protected virtual void OnPreUpdate(){}
    protected virtual void OnUpdate(){}
    protected virtual void OnPostUpdate(){}
    protected virtual void OnCounterReset(){}
    protected virtual void OnStart(){}
    protected virtual void OnWorking(){}
    protected virtual void OnFinish(){}
    protected virtual void OnHalt(){}
    protected virtual void OnRestart(){}


    protected virtual bool Start() => true;
    protected virtual bool Work() => true;
    protected virtual bool Finish() => true;
    protected virtual bool Restart() => true;

    public MachineTE()
    {
    }

    protected void _Place()
    {
      if(OnMachinePlace != null) OnMachinePlace(this);
      if(OnPlaceEvent != null) OnPlaceEvent(this);
      OnPlace();
    }

    public sealed override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate) {
      if(Main.netMode == NetmodeID.MultiplayerClient) {
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
    public sealed override bool IsTileValidForEntity(int x, int y) {
      try {
        Tile tileInPosition = Main.tile[x,y];
        if(tileInPosition.TileType == ValidTile) {
          return true;
        }
      } catch(IndexOutOfRangeException e) {
        ModContent.GetInstance<Factorized>().Logger.WarnFormat("tried to access a tile outside the world {0}",e.StackTrace);
        return false;
      }
      return false;
    }

    public sealed override void Load() {
      //TODO: why?
    }

    public sealed override void LoadData(TagCompound tag)
    {
      this
        .GetType()
        .GetFieldsIwA<MachineDataAttribute>()
        .OrderBy(field => field.Name)
        .ToList()
        .ForEach(field => {
          if (tag.ContainsKey(field.Name)) field.SetValue(this,tag[field.Name]);
        });
    }
    public sealed override void SaveData(TagCompound tag) {
      this
        .GetType()
        .GetFieldsIwA<MachineDataAttribute>()
        .OrderBy(field => field.Name)
        .ToList()
        .ForEach(field => tag[field.Name] = field.GetValue(this));
    }

    // TODO: NetReceive Constructs a new Machine?
    // if so will it be necessary to recreate the UIItemSlots?
    public sealed override void NetReceive(BinaryReader reader) {
      TagCompound tag = TagIO.Read(reader);
      this
        .GetType()
        .GetFieldsIwA<MachineDataAttribute>()
        .OrderBy(field => field.Name)
        .ToList()
        .ForEach(field => field.SetValue(this,tag[field.Name]));
    }
    public sealed override void NetSend(BinaryWriter writer) {
      TagCompound tag = new ();
      this
        .GetType()
        .GetFieldsIwA<MachineDataAttribute>()
        .OrderBy(field => field.Name)
        .ToList()
        .ForEach(field => tag[field.Name] = field.GetValue(this));
      TagIO.Write(tag,writer);
    }
    public sealed override void OnNetPlace() {
      if(Main.netMode == NetmodeID.Server)
        NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
    }

    public sealed override void Update() {
      if(OnMachinePreUpdate is not null) OnMachinePreUpdate(this);
      if(OnPreUpdateEvent is not null) OnPreUpdateEvent(this);
      OnPreUpdate();

      if(OnMachineUpdate is not null) OnMachineUpdate(this);
      if(OnUpdateEvent is not null) OnUpdateEvent(this);
      OnUpdate();
      switch(state){
        case State.Frozen: break;
        case State.Idle: 
        if(Start()){
          if(OnMachineStart is not null) OnMachineStart(this);
          if(OnStartEvent is not null) OnStartEvent(this);
          OnStart();
          state = State.Working;
        }
        break;
        case State.Working:
          if(Work()){
            if(OnMachineWorking is not null) OnMachineWorking(this);
            if(OnWorkingEvent is not null) OnWorkingEvent(this);
            OnWorking();
            counter++;
            if(Finish()){
              if(OnMachineFinish is not null) OnMachineFinish(this);
              if(OnFinishEvent is not null) OnFinishEvent(this);
              OnFinish();
              state = State.Idle;
            }
          } else {
            if(OnMachineHalt is not null) OnMachineHalt(this);
            if(OnHaltEvent is not null) OnHaltEvent(this);
            OnHalt();
            state = State.Halted;
          }
        break;
        case State.Halted:
          if(Restart()){
            if(OnMachineRestart is not null) OnMachineRestart(this);
            if(OnRestartEvent is not null) OnRestartEvent(this);
            OnRestart();
            state = State.Working;
          }
        break;
      }

      if(OnMachinePostUpdate is not null) OnMachinePostUpdate(this);
      if(OnPostUpdateEvent is not null) OnPostUpdateEvent(this);
      OnPostUpdate();
      // TODO: reimplement code for sending updates to clients
      // NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
    }

    /// <summary>
    /// Sets counter to 0
    /// </summary>
    /// <remarks>
    /// Do not call this method on OnMachineCounterReset, OnCounterResetEvent or OnCounterReset
    /// </remarks>
    /// <returns>
    /// Previous value of counter
    /// </returns>
    public int ResetCounter(){
      var c = counter;
      if(OnMachineCounterReset is not null) OnMachineCounterReset(this);
      if(OnCounterResetEvent is not null) OnCounterResetEvent(this);
      OnCounterReset();

      counter = 0;
      return c;
    }
    /// <summary>
    /// Causes the machine to enter the State.Frozen state
    /// </summary>
    /// <remarks>
    /// This method stores the previous state on a private variable so that 
    /// when Unfreeze is called it returns to the previous state.
    ///
    /// Do note that the machine will still call methods and events 
    /// from the OnUpdate family while frozen.
    /// </remarks>
    public void Freeze() {
      if(state is State.Frozen) return;
      prev = state;
      state = State.Frozen;
    }
    /// <summary>
    /// Unfreezes the machine
    /// </summary>
    /// <remarks>
    /// see <see cref="Freeze">
    /// </remarks>
    public void Unfreeze(){if(state is State.Frozen) state = prev;}
    public Func<int> GetCounterUpdated()
    {
      return () =>
        {
          var n = Get(Position);
          return n is null ? 0 : n.counter;
        };
    }

    /// <summary>
    /// Gets a MachineTE by id
    /// </summary>
    /// <remarks>
    /// While perfectly fine in single player each tile entity may have a 
    /// different id on each client, which may be different than the server's
    /// id so prefer to call the overload that takes a position
    /// </remarks>
    public static MachineTE Get(int id)
    {
      TileEntity te;
      TileEntity.ByID.TryGetValue(id,out te);
      if(te == null) return null;
      if(te is not MachineTE) return null;
      return (MachineTE) te;
    }
    /// <summary>
    /// Gets a MachineTE by position
    /// </summary>
    /// <remarks>
    /// When possible this method should be preferred over the overload that takes id
    /// </remarks>
    public static MachineTE Get(Point16 pos)
    {
      TileEntity te;
      TileEntity.ByPosition.TryGetValue(pos,out te);
      if(te == null) return null;
      if(te is not MachineTE) return null;
      return (MachineTE) te;
    }
    /// <summary>
    /// Gets the MachineTE at this machine's current position
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    public MachineTE Get() => MachineTE.Get(Position);

  }
}
