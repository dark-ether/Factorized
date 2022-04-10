using Terraria.ModLoader.IO;

namespace Factorized.Utility
{
    public class IntTupleSerializer : TagSerializer<(int, int), TagCompound>
    {
        public override (int, int) Deserialize(TagCompound tag)
        {
            (int,int) pair = new ();
            pair.Item1  = tag.GetInt("Item1");
            pair.Item2  = tag.GetInt("Item2");
            return pair;
        }

        public override TagCompound Serialize((int, int) value)
        {
            TagCompound tag = new TagCompound();
            tag["Item1"] = value.Item1;
            tag["Item2"] = value.Item2;
            return tag;
        }
    }
}  
