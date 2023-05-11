using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Factorized.Utility;
using Factorized.Machines;

namespace Factorized.Machines.Processing.HeavyFurnace
{
  public class HeavyFurnaceTile : MachineTile
  {
    public override string Texture => "Factorized/Machines/Processing/HeavyFurnace/HeavyFurnace";
    public override MachineTE getTileEntity() => ModContent.GetInstance<HeavyFurnaceTE>();
  }
}
