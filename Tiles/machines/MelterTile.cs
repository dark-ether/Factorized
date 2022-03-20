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
	public class melterTile : machineTile
	{
        
        public override void KillMultiTile(int i, int j ,int FrameX , int FrameY)
        {
            Point16 tileOrigin = TileUtils.GetTileOrigin(i,j);
            Item.NewItem(i * 16 , j * 16,48,32,ModContent.ItemType<items.placeables.melterItem>());

        }

        public override machineTE getTileEntity() => ModContent.GetInstance<melterTE>();
        

    }
}
