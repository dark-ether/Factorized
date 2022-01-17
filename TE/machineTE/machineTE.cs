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

namespace factorized.TE.machineTE{
    public abstract class machineTE : ModTileEntity
    {
        public Item[] inputSlots;
        public Item[] outputSlots; 
        

        public virtual int getHeight() => 2;//alowing customization of size of multitile
        public virtual Point16 getMouseRelativePlacePosition() => new Point16(0,0);
        public virtual int getWidth() => 2;
        public abstract int getValidTile();

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate){
            
            if(Main.netMode == NetmodeID.MultiplayerClient){
                int height = getHeight();
                int width = getWidth();
                
                NetMessage.SendTileSquare(Main.myPlayer,i,j,width,height);
                
                NetMessage.SendData(MessageID.TileEntityPlacement,-1,-1,null,i,j,Type);
            
            }
            Point16 tileOrigin = getMouseRelativePlacePosition();
            int placedEntity = Place(i-tileOrigin.X,j-tileOrigin.Y);
            ((machineTE)TileEntity.ByID[placedEntity]).onPlace();
            return placedEntity;
        }
        
        public override bool IsTileValidForEntity(int x, int y){
            try{
            Tile tileInPosition = Main.tile[x,y];
            if(tileInPosition.type == getValidTile()){
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
        }
    
        public override void OnNetPlace()
        {
            if(Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
        }
        
        public virtual void onPlace(){
        inputSlots = new Item[]{new Item()};
        outputSlots = new Item[]{new Item()};
        }

        public override void SaveData(TagCompound tag){
            tag.Set("inputSlots",inputSlots.ToList());
            tag.Set("outputSlots",outputSlots.ToList());
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnPlayerUpdate(Player player)
        {
            base.OnPlayerUpdate(player);
        }


        public override void NetPlaceEntityAttempt(int i, int j)
        {
            base.NetPlaceEntityAttempt(i, j);
        }

        public override void NetSend(BinaryWriter writer)
        {
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

        public override void NetReceive(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            for(int i =0 ;i < length;i++){
                int type = reader.ReadInt32();
                int stack = reader.ReadInt32();
                inputSlots[i] = new Item(type,stack);
            }
            length = reader.ReadInt32();
            for(int i = 0 ;i < length;i++){
                int type = reader.ReadInt32();
                int stack = reader.ReadInt32();
                outputSlots[i] = new Item(type,stack);
            }

        }

    }
}
