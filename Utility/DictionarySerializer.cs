using Terraria.ModLoader.IO;
using System.Collections.Generic;
using System.Linq;
namespace Factorized.Utility
{  
    /*public class DictionarySerializer<T1,T2>: TagSerializer<Dictionary<T1,T2>,TagCompound>
    {
        public override Dictionary<T1, T2> Deserialize(TagCompound tag)
        {
            return new Dictionary<T1,T2>( tag.GetList<KeyValuePair<T1,T2>>("dict"));
        }

        public override TagCompound Serialize(Dictionary<T1,T2> dict){
            return new TagCompound {["dict"] = dict.ToList()};
        }
    }*/
    public class DictionarySerializerStringInt : TagSerializer<Dictionary<string,int>,TagCompound>
    {
        public override Dictionary<string, int> Deserialize(TagCompound tag)
        {
            return new Dictionary<string,int>( tag.GetList<KeyValuePair<string,int>>("dict"));
        }

        public override TagCompound Serialize(Dictionary<string,int> dict){
            return new TagCompound {["dict"] = dict.ToList()};
        }
    }
    
    public class DictionarySerializerStringDouble: TagSerializer<Dictionary<string,double>,TagCompound>
    {
        public override Dictionary<string, double> Deserialize(TagCompound tag)
        {
            return new Dictionary<string,double>( tag.GetList<KeyValuePair<string,double>>("dict"));
        }

        public override TagCompound Serialize(Dictionary<string,double> dict){
            return new TagCompound {["dict"] = dict.ToList()};
        }
    }
    
    public class DictionarySerializerStringString: TagSerializer<Dictionary<string,string>,TagCompound>
    {
        public override Dictionary<string, string> Deserialize(TagCompound tag)
        {
            return new Dictionary<string,string>( tag.GetList<KeyValuePair<string,string>>("dict"));
        }

        public override TagCompound Serialize(Dictionary<string,string> dict){
            return new TagCompound {["dict"] = dict.ToList()};
        }
    }
}
