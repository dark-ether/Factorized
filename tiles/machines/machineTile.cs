using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;
using Microsoft.Xna.Framework;
using factorized.ui;
using factorized.TE.machineTE;

namespace factorized.tiles.machines{
    public abstract class machineTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
			//ItemDrop = ModContent.ItemType<content.items.placeables.melter>();
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.Origin = new Point16(0,0);
            modifyObjectData();
            machineTE myMachine = getTileEntity();//get tile entity makes the tile call the correct tile entity
            if(myMachine is not null){
                TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(myMachine.Hook_AfterPlacement
                ,-1,0,false);
            }
            TileObjectData.addTile(Type);
        }
        
        public virtual void modifyObjectData()
        {
        }

        public override bool RightClick(int x, int y){
            Vector2 playerPosition = Main.LocalPlayer.Center;
            Vector2 tilePosition = new(x * 16, y * 16);//remember that tile coordinates are 1/16 world coordinates

            if(Vector2.Distance(playerPosition,tilePosition)< 5 *16){
            UICaller.showMachineUI(x,y);
            return true;
            }
            return false;
        }

        public virtual machineTE getTileEntity() => null;

    }
}
