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
        public Dictionary<string,double> values;
        public Dictionary<string,string> properties;
        
        //these here define how the above are interpreted 
        public Dictionary<string,int> countersData;
        public Dictionary<string,double> valuesData;
        public Dictionary<string,string> propertiesData;
        
        public virtual int timer 
        {
            get{ return counters["timer"];}
            set{ counters["timer"] = value;}
        }
        
        public virtual int timerLimit
        {
            get{ return countersData["timerLimit"];}
            set{ countersData["timerLimit"] = value;}
        }
        
        public virtual int energy 
        {
            get{ int val;
                counters.TryGetValue("energy",out val);
              return val;}
            set{ counters["energy"] = value;}
        }
        public virtual int progressEnergyComsumption
        {
            get{ return countersData["energyComsumption"];}
            set{ countersData["energyComsumption"] = value;} 
        }
        public int energyMultiplier
        {
            get{return 27720;}
        }
        public double TerraFlux
        {
            get{ return energy/energyMultiplier;}
        }

        public MachineState(){
            this.counters       = new ();
            this.values         = new ();
            this.properties     = new ();
            this.countersData   = new ();
            this.valuesData     = new ();
            this.propertiesData = new ();
            timer = 0;
            timerLimit = 5*60;
            energy = 0;
            energyComsumption = 0;
        }

        public MachineState(Dictionary<string, int> counters, Dictionary<string, double> values, 
                            Dictionary<string, string> properties, Dictionary<string, int> countersData, 
                            Dictionary<string, double> valuesData, 
                            Dictionary<string, string> propertiesData)
        {
            this.counters = counters;
            this.values = values;
            this.properties = properties;
            this.countersData = countersData;
            this.valuesData = valuesData;
            this.propertiesData = propertiesData;
        }

        public TagCompound SerializeData()
        {
            TagCompound myTag = new TagCompound(){
                ["counters"]        = counters,
                ["values"]          = values,
                ["properties"]      = properties,
                ["countersData"]    = countersData,
                ["valuesData"]      = valuesData,
                ["propertiesData"]  = propertiesData
            };                
            return myTag;
        }

        public static MachineState machineStateLoad(TagCompound tag)
        {
            MachineState myMachineState = new MachineState ();
            myMachineState.counters = tag.Get<Dictionary<string,int>>("counters");
            myMachineState.values = tag.Get<Dictionary<string,double>>("values");
            myMachineState.properties = tag.Get<Dictionary<string,string>>("properties");
            myMachineState.countersData = tag.Get<Dictionary<string,int>>("countersData");
            myMachineState.valuesData = tag.Get<Dictionary<string,double>>("valuesData");
            myMachineState.propertiesData = tag.Get<Dictionary<string,string>>("propertiesData");
            return myMachineState;
        }

    }
}
