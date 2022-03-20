using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using factorized.tiles.machines;
using Terraria.ID;
using System.Collections.Generic;
using factorized.library;

namespace factorized.TE.machineTE{
    public class melterTE : machineTE {
        public override int getValidTile() => ModContent.TileType<melterTile>();


    }
}
