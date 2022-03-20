using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.ID;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.IO;
using factorized.utility;

//don't forget to read this code for todos
namespace factorized.TE.machineTE{
    public abstract class machineTE : ModTileEntity
    {
        public Item[] inputSlots {get; protected set;}
        public Item[] outputSlots {get; protected set;}
        public Item[] otherSlots {get; protected set;}
        protected MachineState machineState;
        protected Dictionary<MachineInput,MachineOutput> ProcessIO;
        
        public virtual int Height{get;} = 2;
        public virtual Point16 MouseRelativePlacePosition{get;} = new Point16(0,0);
        // point of origin on tile use the same as newTile.origin
        public virtual int NumberOfInputSlots{get;} = 1;
        public virtual int NumberOfOutputSlots{get;} = 1;
        public virtual int NumberOfOtherSlots{get;} = 0;
        public virtual int Width{get;} = 2;
        
        public virtual void onPlace(int x,int y){}
        public virtual void onProcessFailure(product process){}                            
        public virtual void onProcessFound(product foundProcess){}
        public virtual void onProcessNotFound(){}
        public virtual void onProcessSucess(product inputConsumed,product outputCreated){}
        public virtual void onUpdate(){}
        
        public abstract int ValidTile {get;}
        protected abstract void setupProcessIO();

        protected virtual void basicSetup(){
            inputSlots = new Item[NumberOfInputSlots];
            for(int i = 0 ;i<NumberOfInputSlots; i++){
              inputSlots[i]= new Item();
            }
            outputSlots = new Item[NumberOfOutputSlots];
            for(int i = 0 ; i < NumberOfOutputSlots; i++){
              outputSlots[i]= new Item();
            }
            otherSlots = new Item[NumberOfOtherSlots];
            for(int i = 0 ; i< NumberOfOtherSlots;i++){
              otherSlots[i]= new Item();
            }
        }

        protected virtual void updateMachineState(MachineOutput change){
        }
        
        protected MachineOutput getProcess(){
        }
        
        protected bool hasSpaceForProcess(MachineOutput results){
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
            machineTE placedMachine = (machineTE)TileEntity.ByID[placedEntity];
            placedMachine.basicSetup();
            placedMachine.onPlace(i - tileOrigin.X,j - tileOrigin.Y);
            placedMachine.setupProcessIO();
            return placedEntity;
        }

        public override bool IsTileValidForEntity(int x, int y){
            try{
            Tile tileInPosition = Main.tile[x,y];
            if(tileInPosition.type == ValidTile){
                return true;
            }
            }catch(IndexOutOfRangeException){
                ModContent.GetInstance<factorized>().Logger.Warn("tried to acess a tile outside the world");
                return false;
            }
            return false;
        }

        public override void LoadData(TagCompound tag){
            inputSlots = tag.Get<List<Item>>("inputSlots").ToArray();
            outputSlots = tag.Get<List<Item>>("outputSlots").ToArray();
            machineState = tag.Get<MachineState>("machineState");
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
            timer =  reader.ReadInt32();

        }
        //todo change net place send and net receive
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
            tag.Set("otherSlots",otherSlots.ToList());
            tag.Set("machineState",machineState);
        }
        //todo : change net messages again

        public override void Update(){
            onUpdate();
            if(machineState.isProcessing)
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void OnPlayerUpdate(Player player)
        {
            base.OnPlayerUpdate(player);
        }






        public override void NetPlaceEntityAttempt(int i, int j)
        {
            base.NetPlaceEntityAttempt(i, j);
        }

        public override void Load(Mod mod)
        {
            base.Load(mod);
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return base.IsLoadingEnabled(mod);
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void PreGlobalUpdate()
        {
            base.PreGlobalUpdate();
        }

        public override void PostGlobalUpdate()
        {
            base.PostGlobalUpdate();
        }

        public override void OnKill()
        {
            base.OnKill();
        }
    }
