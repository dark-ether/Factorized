using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ID;
using Factorized.Utility;
using Terraria;
using System.Text.RegularExpressions;
using Terraria.ModLoader.IO;

namespace Factorized.Machines.Processing.Furnace
{
  public class FurnaceTE : MachineTE
  {
    public override int ValidTile => ModContent.TileType<FurnaceTile>();
  }
}
