using Terraria.ModLoader.IO;
using System.Collections.Generic;

namespace FactorizedUtility
{  
    public class KeyValuePairStringIntSerializer : TagSerializer<KeyValuePair<string,int>,TagCompound>
    {
        public override KeyValuePair<string,int> Deserialize(TagCompound tag)
        {
            return new KeyValuePair<string,int>(tag.GetString("key"),tag.GetInt("value"));
        }
        public override TagCompound Serialize(KeyValuePair<string,int> pair)
        {
            return new TagCompound {["key"] = pair.Key, ["value"] = pair.Value};
        }
    }

    public class KeyValuePairStringDoubleSerializer : TagSerializer<KeyValuePair<string,double>,TagCompound>
    {
        public override KeyValuePair<string,double> Deserialize(TagCompound tag)
        {
            return new KeyValuePair<string,double>(tag.GetString("key"),tag.GetDouble("value"));
        }
        public override TagCompound Serialize(KeyValuePair<string,double> pair)
        {
            return new TagCompound {["key"] = pair.Key, ["value"] = pair.Value};
        }
    }
    
    public class KeyValuePairStringStringSerializer : TagSerializer<KeyValuePair<string,string>,TagCompound>
    {
        public override KeyValuePair<string,string> Deserialize(TagCompound tag)
        {
            return new KeyValuePair<string,string>(tag.GetString("key"),tag.GetString("value"));
        }
        public override TagCompound Serialize(KeyValuePair<string,string> pair)
        {
            return new TagCompound {["key"] = pair.Key, ["value"] = pair.Value};
        }
    }
}
