using Terraria.ModLoader;
using Factorized.Machines;
using Terraria.ObjectData;

namespace Factorized.Content.SteelForge
{
  public class SteelForgeTile : MachineTile
  {
    public override string Texture => "Factorized/Content/SteelForge/SteelForge";

    public override MachineTE getTileEntity()
    {
      return ModContent.GetInstance<SteelForgeTE>();
    }
    public override void modifyObjectData()
    {
      TileObjectData.newTile.Height = 3;
      TileObjectData.newTile.CoordinateHeights = new int[] {16,16,10};
      TileObjectData.newTile.DrawYOffset = 8;
    }
  }
}
