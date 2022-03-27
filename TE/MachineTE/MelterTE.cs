using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Factorized.Tiles.Machines;
using Factorized.Utility;
using Terraria.ID;
using System.Collections.Generic;

namespace Factorized.TE.MachineTE{
    public class MelterTE : MachineTE {
        public override int ValidTile => ModContent.TileType<MelterTile>();
        protected override void setupProcessIO()
        {
            MachineInput input = new ();
            MachineOutput output = new ();
            (int,int) item; 
            item = (ItemID.SandBlock,1);
            input.inputItems.Add(item);
            output.itemsToRemove.Add(item);
            item = (ItemID.Glass,2);
            output.itemsToAdd.Add(item);

        }
    }
}
