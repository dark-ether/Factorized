using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Factorized.Utility
{  
    public class Functions
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
    }
}
