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

namespace Factorized.Machines.Processing.HeavyFurnace
{
    public class HeavyFurnaceTE : MachineTE
    {
        public override int ValidTile => ModContent.TileType<HeavyFurnaceTile>();
        protected override void setupProcessIO()
        {
            MachineInput input = new ();
            MachineOutput output = new ();
            List<(int,int)> category1 = new ();
            (int,int) item; 
            item = (ItemID.SandBlock,1);
            category1.Add(item);
            input.inputItems["input"] = category1;
            output.itemsToRemove.Add(item);
            item = (ItemID.Glass,2);
            output.itemsToAdd.Add(item);
            output.processingTime = 60*5;
            ProcessIO = new ();
            ProcessIO[input] = output;
        }
    }
}
