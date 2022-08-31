using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Factorized.Utility;
using Factorized.TE.MachineTE;

namespace Factorized.Tiles.Machines
{
    public class HeavyFurnaceTile : MachineTile
    {
        
        public override BaseMachineTE getTileEntity() => ModContent.GetInstance<HeavyFurnaceTE>();
        public override int getItemType() => ModContent.ItemType<Items.Placeables.HeavyFurnaceItem>();
    }
}
