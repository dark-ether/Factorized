using Terraria.ModLoader;
using Factorized.Machines;

namespace Factorized.Content.SteelForge
{
  public class SteelForgeTE : MachineTE
  {
    public override int ValidTile => ModContent.TileType<SteelForgeTile>();
  }
}
