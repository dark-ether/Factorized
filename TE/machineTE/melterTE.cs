using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using factorized.tiles.machines;

namespace factorized.TE.machineTE{
    class melterTE : ModTileEntity
    {
        public int timesClicked;
        
        public melterTE(){
            timesClicked = 0;
            ModContent.GetInstance<factorized>().Logger.Debug("created melterTE");
        }
        
        public melterTE(int x, int y){
            timesClicked = 0;
            Place(x,y);
            ModContent.GetInstance<factorized>().Logger.Debug($"position {x},{y}");
        }   
        
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
            
            ModContent.GetInstance<factorized>().Logger.Debug("saving tile entity");
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag.Set(nameof(timesClicked),timesClicked);
            ModContent.GetInstance<factorized>().Logger.Debug("saving tile entity");
        }


    }
}
