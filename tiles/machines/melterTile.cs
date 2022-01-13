using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using factorized.ui;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

using factorized.TE.machineTE;
using factorized.tileUtils;

namespace factorized.tiles.machines{
	public class melterTile : ModTile
	{
        public override string Name => base.Name;

        public override string Texture => base.Texture;

        public override string HighlightTexture => base.HighlightTexture;

        public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
			//ItemDrop = ModContent.ItemType<content.items.placeables.melter>();
            
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<melterTE>()
            .Hook_AfterPlacement,-1,0,false);
            TileObjectData.newTile.Origin = new Point16(0,0);    

            TileObjectData.addTile(Type);
			AddMapEntry(new Color(200, 200, 200));

		}
        
        public override void KillMultiTile(int i, int j ,int FrameX , int FrameY)
        {
            Point16 tileOrigin = TileUtils.GetTileOrigin(i,j);
            Item.NewItem(i * 16 , j * 16,48,32,ModContent.ItemType<items.placeables.melterItem>());
        }

        public override bool RightClick(int x ,int y){
            //Point16 pos = new(x,y);
            //TileEntity = TileEntity.ByPosition[pos];
            //code for acessing tile entity
                    
            Point16 tileOrigin = TileUtils.GetTileOrigin(x,y);
            UICaller.showMelterUI(tileOrigin.X, tileOrigin.Y);
            Main.playerInventory = true;
            TileEntity entityInPosition;
            if(TileEntity.ByPosition.TryGetValue(tileOrigin,out entityInPosition)){
                if(entityInPosition is melterTE){
                    melterTE thisMelter = (melterTE) entityInPosition;
                    thisMelter.timesClicked += 1;
                }
            }
            return true;
        
        }
        

    }
}
