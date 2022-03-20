using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Factorized.tiles.machines;
using Terraria.ID;
using System.Collections.Generic;
using Factorized.library;

namespace Factorized.TE.machineTE{
    public class melterTE : machineTE {
        public override int getValidTile() => ModContent.TileType<melterTile>();


    }
}
