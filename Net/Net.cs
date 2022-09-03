using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Factorized.Utility;
using Factorized.Machines;
using Terraria.DataStructures;

namespace Factorized.Net {
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


        public static void Write(this BinaryWriter writer, List<(int,int)> listoftuples)
        {
            writer.Write(listoftuples.Count);
            foreach(var tuple in listoftuples)
            {
                writer.Write(tuple.Item1);
                writer.Write(tuple.Item2);
            }
        }

        public static void Write(this BinaryWriter writer, List<string> listostrings)
        {
            writer.Write(listostrings.Count);
            foreach (var myString in listostrings) {
                writer.Write(myString);
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
        public static List<string> ReadListOfString(this BinaryReader reader)
        {
            List<string> list = new ();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                list.Add(reader.ReadString());
            }
            return list;
        }
        public static MachineSlot ReadMachineSlot(this BinaryReader reader)
        {
            MachineSlot r = new ((MachineSlotType)reader.ReadInt32());
            r.SlotItem = ItemIO.Receive(reader,true);
            return r;
        }
        public static void Write(this BinaryWriter writer,MachineSlot inp)
        {
            writer.Write((int)inp.Type);
            ItemIO.Send(inp.SlotItem,writer,true);
        }
        public static void Write(this BinaryWriter writer,MachineProcess process)
        {
            writer.Write(process.ProcessingTime);
            if(process.Consume == null){
                writer.Write(false);
            }
            else{
                writer.Write(true);
                writer.Write(process.Consume);
            }
            if(process.Produce!= null){
                writer.Write(true);
                writer.Write(process.Produce);
            }
            else
            {
                writer.Write(false);
            }
            if(process.Properties == null){
                writer.Write(false);
            }else{
                writer.Write(true);
                TagIO.Write(process.Properties,writer);
            }
        }
        public static MachineProcess ReadMachineProcess(this BinaryReader reader)
        {
            MachineProcess r = new ();
            r.ProcessingTime = reader.ReadInt32();
            if(reader.ReadBoolean()){
                r.Consume = reader.ReadListOfItems();
            }
            else
            {
                r.Consume = new ();
            }
            if(reader.ReadBoolean()){
                r.Produce = reader.ReadListOfItems();
            }
            else
            {
                r.Produce = new ();
            }
            if(reader.ReadBoolean()){
                r.Properties = TagIO.Read(reader);
            }
            else 
            {
                r.Properties = new ();
            }
            return r;
        }
        public static void Write(this BinaryWriter writer, List<Item> inp)
        {
            writer.Write(inp.Count());
            foreach(var item in inp)
            {
                ItemIO.Send(item, writer, true);
            }
        }
        public static List<Item> ReadListOfItems(this BinaryReader reader)
        {
            List<Item> r= new ();
            int c = reader.ReadInt32();
            for(int i =0; i < c;i++)
            {
                r.Add(ItemIO.Receive(reader,true));
            }
            return r;
        }
        public static Point16 ReadPoint16(this BinaryReader reader)
        {
            int x = reader.ReadInt16();
            return new Point16 (x,reader.ReadInt16());
        }
        public static void Write(this BinaryWriter writer, Point16 pos)
        {
            writer.Write(pos.X);
            writer.Write(pos.Y);
        }
    }
}
