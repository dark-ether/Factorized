using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Factorized.Utility;
using Factorized.TE.MachineTE;

namespace Factorized.Tiles.Machines
{
    public class HeavyFurnaceTile : MachineTile
    {
        
        public override void KillMultiTile(int i, int j ,int FrameX , int FrameY)
        {
            Point16 tileOrigin = TileUtils.GetTileOrigin(i,j);
            Item.NewItem(new EntitySource_TileBreak(i, j),i * 16 , j * 16,48,32,ModContent.ItemType<Items.Placeables.HeavyFurnaceItem>());
            //remember world coordinates are 16* tile coordinates

        }
        public override MachineTE getTileEntity() => ModContent.GetInstance<HeavyFurnaceTE>();
    }
}
