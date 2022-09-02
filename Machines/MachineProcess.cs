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
    public class MachineProcessSerializer : TagSerializer<MachineProcess, TagCompound>
    {
        public override MachineProcess Deserialize(TagCompound tag)
        {
            MachineProcess r =  new ();
            r.ProcessingTime = tag.GetAsInt("Pt");
            r.Consume = (List<Item>)tag.GetList<Item>("Consume");
            r.Produce = (List<Item>)tag.GetList<Item>("Produce");
            r.Properties = tag.GetCompound("Props");
            return r;
        }

        public override TagCompound Serialize(MachineProcess value)
        {
            return new TagCompound(){["Pt"] = value.ProcessingTime,["Consume"] = value.Consume,
            ["Produce"] = value.Produce, ["Props"] = value.Properties};
        }
    }
}
