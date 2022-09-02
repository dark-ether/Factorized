using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Factorized.Utility;
using Terraria.ID;
using System.Collections.Generic;
using Factorized.Machines;

namespace Factorized.Machines.Processing.HeavyFurnace
{
    public class HeavyFurnaceTE : MachineTE
    {
        public override int ValidTile => ModContent.TileType<HeavyFurnaceTile>();

        public override List<MachineProcess> allProcesses => Recipes;

        public override Dictionary<MachineSlotType, int> SlotsComposition { 
            get =>  new (){[MachineSlotType.Input] = 3 , [MachineSlotType.Output] = 2};
        }

        public static List<MachineProcess> Recipes = new ();
        static HeavyFurnaceTE()
        {
            MachineProcess process = new();
            Item item = new (ItemID.SandBlock,1);
            process.Consume.Add(item);
            item = new (ItemID.Glass,2);
            process.Produce.Add(item);
            process.ProcessingTime = 5*60;
            Recipes.Add(process);
        }
    }
}
