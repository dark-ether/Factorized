using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Factorized.Utility;
using Factorized.Machines;

namespace Factorized.Machines.Processing.HeavyFurnace
{
    public class HeavyFurnaceTile : MachineTile
    {
        public override MachineTE getTileEntity() => ModContent.GetInstance<HeavyFurnaceTE>();
        public override int getItemType() => ModContent.ItemType<Machines.Processing.HeavyFurnace.HeavyFurnaceItem>();
    }
}
