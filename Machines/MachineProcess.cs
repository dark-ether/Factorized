using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;

namespace Factorized.Machines
{
    public class MachineProcess
    {
        public int ProcessingTime { get; set;} = 5 * 60;
        public List<Item> Consume { get; set;} = new ();
        public List<Item> Produce { get; set;} = new ();
        public TagCompound Properties {get; set;} = new ();
    }
}
