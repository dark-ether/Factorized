using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.ID;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace factorized.TE.machineTE{
    public abstract class machineTE : ModTileEntity
    {
        public int inputSlotsNumber{get; protected set;}
        public int outputSlotsNumber{get ; protected set;}
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
/*
        public override void LoadData(TagCompound tag){
            inputSlotsNumber = tag.GetInt("inputSlotsNumber");
            outputSlotsNumber= tag.GetInt("outputSlotsNumber");
            
            inputSlots = tag.Get<Item[]>("inputSlots");
            outputSlots = tag.Get<Item[]>("outputSlots");
        }
   */     
        public virtual void onPlace(){
        inputSlotsNumber = 1;
        outputSlotsNumber = 1;
        inputSlots = new Item[]{new Item()};
        outputSlots = new Item[]{new Item()};
        }
/*
        public override void SaveData(TagCompound tag){
            tag.Set("inputSlotNumber",inputSlotsNumber);
            tag.Set("outputSlotNumber",outputSlotsNumber);

            tag.Set("inputItems",inputSlots.Clone());
            tag.Set("outputItems",outputSlots.Clone());
        }
        */
    }
}
