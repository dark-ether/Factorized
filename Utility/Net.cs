using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Factorized.Utility {
    public static class Net
    {
        public static void Write(this BinaryWriter writer,Dictionary<string,int> dict) 
        {
            writer.Write(dict.Count());
            foreach(var keyValuePair in dict)
            {
                writer.Write(keyValuePair.Key);
                writer.Write(keyValuePair.Value);
            }
        }
        
        public static void Write(this BinaryWriter writer,Dictionary<string,double> dict)
        {
            writer.Write(dict.Count());
            foreach (var keyValuePair in dict)
            {
                writer.Write(keyValuePair.Key);
                writer.Write(keyValuePair.Value);
            }
        }
        
        public static void Write(this BinaryWriter writer, Dictionary<string,string> dict)
        {
            writer.Write(dict.Count());
            foreach(var keyValuePair in dict)
            {
                writer.Write(keyValuePair.Key);
                writer.Write(keyValuePair.Value);
            }
        }

        public static void Write(this BinaryWriter writer, MachineState machineState)
        {
            writer.Write(machineState.counters);
            writer.Write(machineState.values);
            writer.Write(machineState.properties);
            writer.Write(machineState.countersData);
            writer.Write(machineState.valuesData);
            writer.Write(machineState.propertiesData);

            writer.Write(machineState.currentProcess);
        }

        public static void Write(this BinaryWriter writer, MachineOutput output)
        {
            writer.Write(output.itemsToAdd);
            writer.Write(output.itemsToRemove);
            writer.Write(output.changeCounters);
            writer.Write(output.changeValues);
            writer.Write(output.propertiesToSet);
        }
        
        public static void Write(this BinaryWriter writer, List<(int,int)> listoftuples)
        {
            writer.Write(listoftuples.Count);
            foreach(var tuple in listoftuples)
            {
                writer.Write(tuple.Item1);
                writer.Write(tuple.Item2);
            }
        }

        public static Dictionary<string,int> ReadStringIntDictionary(this BinaryReader reader)
        {
            int capacity = reader.ReadInt32();
            Dictionary<string,int> counters = new();
            for(int i = 0; i < capacity;i++)
            {
                string key  = reader.ReadString();
                int counter = reader.ReadInt32();
                counters[key] = counter;
            }
            return counters;
        }
        
        public static Dictionary<string, double> ReadStringDoubleDictionary(this BinaryReader reader)
        {
            int capacity = reader.ReadInt32();
            Dictionary<string,double> counters = new();
            for(int i = 0; i < capacity;i++)
            {
                string key  = reader.ReadString();
                double counter = reader.ReadDouble();
                counters[key] = counter;
            }
            return counters;
        }
        
        public static Dictionary<string, string> ReadStringStringDictionary(this BinaryReader reader)
        {
            int capacity = reader.ReadInt32();
            Dictionary<string, string> counters = new();
            for(int i = 0; i < capacity;i++)
            {
                string key  = reader.ReadString();
                string counter = reader.ReadString();
                counters[key] = counter;
            }
            return counters;
        }

        public static MachineState ReadMachineState(this BinaryReader reader)
        {
            MachineState machine = new MachineState();

            machine.counters = reader.ReadStringIntDictionary();
            machine.values = reader.ReadStringDoubleDictionary();
            machine.properties = reader.ReadStringStringDictionary();
            machine.countersData = reader.ReadStringIntDictionary();
            machine.valuesData = reader.ReadStringDoubleDictionary();
            machine.propertiesData = reader.ReadStringStringDictionary();
            machine.currentProcess = reader.ReadMachineOutput();
            return machine;
        }

        public static MachineOutput ReadMachineOutput(this BinaryReader reader)
        {
            MachineOutput machine = new ();

            machine.itemsToAdd = reader.ReadListOfTupleOfInts();
            machine.itemsToRemove = reader.ReadListOfTupleOfInts();
            machine.changeCounters = reader.ReadStringIntDictionary();
            machine.changeValues = reader.ReadStringDoubleDictionary();
            machine.propertiesToSet = reader.ReadStringStringDictionary();
            return machine;
        }

        public static List<(int,int)> ReadListOfTupleOfInts(this BinaryReader reader)
        {
            List<(int,int)> list = new ();
            int count = reader.ReadInt32();
            for(int i = 0; i< count; i++)
            {
                int type = reader.ReadInt32();
                int quantity = reader.ReadInt32();
                list.Add((type,quantity));
            }
            return list;
        }
    }
}
