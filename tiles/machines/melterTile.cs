using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using factorized.ui;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using factorized.TE.machineTE;

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
            
            TileObjectData.addTile(Type);
			AddMapEntry(new Color(200, 200, 200));
		}
        
        public override void KillMultiTile(int i, int j ,int FrameX , int FrameY)
        {
            Item.NewItem(i * 16 , j * 16,48,32,ModContent.ItemType<items.placeables.melterItem>());
        }

        public override bool RightClick(int x ,int y){
            //Point16 pos = new(x,y);
            //TileEntity = TileEntity.ByPosition[pos];
            //code for acessing tile entity
            
            UICaller.showMelterUI(x, y);
            Main.playerInventory = true;
            Tile myTile = Main.tile[x,y];

            int newX = x - myTile.frameX/18;
            int newY = y - myTile.frameY / 18 + 1;  
            TileEntity entityInPosition = TileEntity.ByPosition[new Point16(newX,newY)];
            if(entityInPosition is melterTE){
                melterTE thisMelter = (melterTE) entityInPosition;
                thisMelter.timesClicked += 1;
            }
            return true;
        
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            base.PlaceInWorld(i, j, item);
            melterTE thisMelter = new melterTE(i,j);
        }

    }
}
