using Terraria.ModLoader.IO;
using System.Collections.Generic;
using System;

namespace Factorized.Utility
{  
    public class MachineState : TagSerializable
    {
        public static readonly Func<TagCompound,MachineState> DESERIALIZER = machineStateLoad;

        public Dictionary<string,int> counters;
        public Dictionary<string,double> values;
        public Dictionary<string,string> properties;
        public MachineOutput currentProcess;
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
            get
            { 
              int val;
                counters.TryGetValue("energy",out val);
              return val;
            }
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
        public int numberOfSpecialInputSlots
        {
            get{return countersData["numberOfSpecialInputSlots"];}
            set{countersData["numberOfSpecialInputSlots"] = value;}
        }
        public int numberOfSpecialOutputSlots
        { 
          get{return countersData["numberOfSpecialOutputSlots"];} 
          set{countersData["numberOfSpecialOutputSlots"] = value;} 
        }

        public MachineState(){
            this.counters       = new ();
            this.values         = new ();
            this.properties     = new ();
            this.countersData   = new ();
            this.valuesData     = new ();
            this.propertiesData = new ();
            this.timer = 0;
            this.timerLimit = 5*60;
            this.energy = 0;
            this.progressEnergyComsumption = energyMultiplier * 5;
            this.numberOfSpecialOutputSlots = 0;
            this.numberOfSpecialInputSlots = 0;
        }
        
        public MachineState(MachineState toCopy)
        {
            this.counters = new Dictionary<string, int>(toCopy.counters);
            this.values  = new Dictionary<string, double>(toCopy.values) ;
            this.properties = new Dictionary<string, string>(toCopy.properties);

            this.countersData = new Dictionary<string, int>(toCopy.countersData);
            this.valuesData =  new Dictionary<string, double>(toCopy.valuesData);
            this.propertiesData = new Dictionary<string, string>(toCopy.propertiesData);
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
            myMachineState.counters = new (tag.Get<Dictionary<string,int>>("counters"));
            myMachineState.values = new (tag.Get<Dictionary<string,double>>("values"));
            myMachineState.properties = new (tag.Get<Dictionary<string,string>>("properties"));
            myMachineState.countersData = new (tag.Get<Dictionary<string,int>>("countersData"));
            myMachineState.valuesData = new (tag.Get<Dictionary<string,double>>("valuesData"));
            myMachineState.propertiesData = new (tag.Get<Dictionary<string,string>>("propertiesData"));
            return myMachineState;
        }

        public bool IsProcessing()
        {
            return currentProcess != null;
        }

        public void SetProcess(MachineOutput process)
        {
           currentProcess = process;
        }

        public virtual bool CanProgress()
        {
            return true;
        }

    }
}
