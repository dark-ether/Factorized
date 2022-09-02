using System.Collections.Generic;
using System.Linq;
using System.IO;
using Terraria;
using Factorized.Machines;
using Terraria.DataStructures;

namespace Factorized.Utility
{  
    public static class Lib
    {
        public static Dictionary<T1,T2> mergeDictionariesNoRepeats<T1,T2>(IEnumerable<Dictionary<T1,T2>> dicts)
        {
            Dictionary<T1,T2> mergedDictionaries = new ();
            foreach(var dict in dicts)
            {
                foreach(var keyValuePair in dict){
                    if (!mergedDictionaries.ContainsKey(keyValuePair.Key))
                    {
                        mergedDictionaries[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
            }
            return mergedDictionaries;
        }
        public static Item[] cloneItemArray(Item[] array) 
        {
            Item[] myClone = new Item[array.Length];
            for(int i = 0; i < array.Length ; i++) 
            {
                myClone[i] = array[i].Clone();
            }
            return myClone;
        }
        public static MachineTE GetMachineTE(Point16 pos)
        {
            TileEntity i;
            TileEntity.ByPosition.TryGetValue(pos,out i);
            if(i == null) return null;
            if(!(i is MachineTE)) return null;
            return (MachineTE)i;
        }
    }
}
