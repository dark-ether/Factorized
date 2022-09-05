using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Terraria;
using Factorized.Machines;
using Terraria.DataStructures;

namespace Factorized.Utility
{  
    public static class Lib
    {
        public static bool HasAncestor(Type searched,Type ancestor)
        {
            if(searched == typeof(System.Object)&& ancestor != typeof(System.Object)) return false;
            if(ancestor == typeof(System.Object)) return true;
            if(searched == ancestor ) return true;
            return HasAncestor(searched.BaseType,ancestor);
        }
    }
}
