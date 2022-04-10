using System.Collections.Generic;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using Terraria;
using System;
namespace Factorized.Utility
{ 
    public class MachineOutput : TagSerializable
    {
        public List<(int,int)> itemsToAdd;
        public List<(int,int)> itemsToRemove;
        public Dictionary<string,int>     changeCounters;
        public Dictionary<string,double>  changeValues;
        public Dictionary<string,string>  propertiesToSet;
        

        public static readonly Func<TagCompound,MachineOutput> DESERIALIZER = machineOutputLoad;
        public MachineOutput()
        {
            this.itemsToAdd = new ();
            this.itemsToRemove = new ();
            this.changeCounters = new ();
            this.changeValues = new ();
            this.propertiesToSet = new ();
        }
        
        public MachineOutput(MachineOutput toCopy)
        {
            this.itemsToAdd = new (toCopy.itemsToAdd);
            this.itemsToRemove = new (toCopy.itemsToRemove);
            this.changeCounters = new (toCopy.changeCounters);
            this.propertiesToSet = new (toCopy.propertiesToSet);
            this.changeValues =  new (toCopy.changeValues);
        }
        
        public void incrementCounters(MachineState machineState)
        {
            foreach (var counter in changeCounters)
            {
                if(machineState.counters.ContainsKey(counter.Key))
                {
                    machineState.counters[counter.Key] += counter.Value;
                }
                else
                {
                    machineState.counters[counter.Key] = counter.Value;
                }
            }
        }

        public void incrementValues(MachineState machineState)
        {
            foreach (var value in changeValues)
            {
                if(machineState.values.ContainsKey(value.Key))
                {
                    machineState.values[value.Key] += value.Value;
                }
                else
                {
                    machineState.values[value.Key] = value.Value; 
                }
            }
        }

        public void setProperties(MachineState machineState)
        {
            foreach (var property in propertiesToSet)
            {
                machineState.properties[property.Key] = property.Value;
            }
        }
        public void consumeItems(Item[] input)
        {
            foreach(var item in itemsToRemove)
            {
                for(int i = 0; i< input.Length;i++)
                {
                    if(input[i].type ==item.Item1)
                    {
                        input[i].stack -= item.Item2;
                    }
                    if(input[i].stack <= 0)
                    {
                        input[i] = new Item();
                    }
                }
            }
        }
        public void produceItems(Item[] output)
        {
            foreach(var item in itemsToAdd)
            {
                bool hasAddedItem = false;
                for(int i = 0;i< output.Length;i++)
                {
                    if(output[i].type == item.Item1&& !hasAddedItem)
                    {
                        output[i].stack += item.Item2;
                        hasAddedItem = true;
                    }
                    if(output[i].IsAir && !hasAddedItem)
                    {
                        output[i] = new Item (item.Item1);
                        output[i].stack = item.Item2;
                        hasAddedItem = true;
                    }
                }
            }
        }

        public static MachineOutput machineOutputLoad(TagCompound tag)
        {
            MachineOutput loaded = new ();
            loaded.itemsToAdd = (List<(int,int)>) tag.GetList<(int,int)>("itemsToAdd");
            loaded.itemsToRemove =  (List<(int,int)>) tag.GetList<(int,int)>("itemsToRemove");
            loaded.changeCounters = tag.Get<Dictionary<string,int>>("changeCounters");
            loaded.changeValues = tag.Get<Dictionary<string,double>>("changeValues");
            loaded.propertiesToSet = tag.Get<Dictionary<string,string>>("propertiesToSet");
            return loaded;
        }

        public TagCompound SerializeData()
        {
            return new TagCompound ()
            {
                ["itemsToAdd"] = itemsToAdd,
                ["itemsToRemove"] = itemsToRemove,
                ["changeCounters"] = changeCounters,
                ["changeValues"] = changeValues,
                ["propertiesToSet"] = propertiesToSet
            };
        }
    }
} 
