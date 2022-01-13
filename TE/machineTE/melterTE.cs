using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using factorized.tiles.machines;
using Terraria.ID;

namespace factorized.TE.machineTE{
    class melterTE : ModTileEntity
    {
        public int timesClicked;
        
        public override bool IsTileValidForEntity(int x, int y)
        {//abstract class has to be overrided
            /* get tile
                Main[x,y];
                
                tiletype to mod/tilename
                ModContent.getModTile(tile.type) is ModTile modTile;

                mod/tilename to tile.type
                mod.Find<ModTile>(name).Type;
            */ 
            Tile myTile = Main.tile[x,y];
            if((ModContent.GetModTile(myTile.type)) is melterTile){
                return true;
            }
            return false;
        }
        
        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            timesClicked = tag.Get<int>(nameof(timesClicked));
            
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag.Set(nameof(timesClicked),timesClicked);
        }
        
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
                int width = 2;
                int height = 2;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //Sync the entire multitile's area.  Modify "width" and "height" to the size of your multitile in tiles
                
                NetMessage.SendTileSquare(Main.myPlayer, i, j, width, height);

                //Sync the placement of the tile entity with other clients
                //The "type" parameter refers to the tile type which placed the tile entity, 
                //so "Type" (the type of the tile entity) needs to be used here instead
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
            }

                //ModTileEntity.Place() handles checking if the entity can be placed, then places it for you
                //Set "tileOrigin" to the same value you set TileObjectData.newTile.Origin to in the ModTile
            Point16 tileOrigin = new Point16(0, 0);
            int newX = i - tileOrigin.X;
            int newY = j - tileOrigin.Y;
            int placedEntity = Place(newX,newY);
            ModContent.GetInstance<factorized>().Logger.Debug($"placed tile entity {newX},{newY}");
            return placedEntity;
        }   
        
        public override void OnNetPlace()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
        }

    }
}
