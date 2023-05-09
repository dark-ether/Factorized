using Terraria.ModLoader;
using Factorized.Machines;
using Terraria;
using Terraria.ObjectData;

namespace Factorized.Machines.Processing.Furnace
{
    public class FurnaceTile : MachineTile
    {
        public override string Texture => "Factorized/Machines/Processing/Furnace/Furnace";

        public override MachineTE getTileEntity()
        {
            return ModContent.GetInstance<FurnaceTE>();
        }
        public override void modifyObjectData()
        {
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[]{16,16,10};
            TileObjectData.newTile.DrawYOffset = 8;
        }
    }
}
