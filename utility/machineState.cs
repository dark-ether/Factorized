using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;

namespace factorized.utility
{
    public class MachineState : TagSerializable
    {
        public static readonly Func<TagCompound,MachineState> DESERIALIZER = machineStateLoad;

        public Dictionary<string,int> counters;
        public Dictionary<string,Double> values;
        public Dictionary<string,string> properties;
        
        //these here define how the above are interpreted 
        protected Dictionary<string,int> countersData;
        protected Dictionary<string,Double> valuesData;
        protected Dictionary<string,string> propertiesData;
        

        public static TagCompound SerializeData()
        {
            TagCompound myTag = new TagCompound(){
                ["counters"]        = counters
                ["values"]          = values,
                ["properties"]      = properties,
                ["countersData"]    = countersData,
                ["valuesData"]      = valuesData,
                ["propertiesData"]  = propertiesData
            };                
            return myTag;
        }
    }
}
