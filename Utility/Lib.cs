using System.Collections.Generic;
using System.Collections;
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
        public static string DToString<T>(this T obj) 
        {
          var self = typeof(Lib).GetMethod("DToString");
          var s = "";
          var ieg =  typeof(IEnumerable<T>);
          var ie = typeof(IEnumerable);
          Type t = obj.GetType();
          switch (obj)
          {
            case IEnumerable<T> l:
              foreach (var f in l) {
                s += f.DToString();
              }
            break;
            case IEnumerable l:
              foreach (var f in l) {
                s += f.DToString();
              }
            break;
            default:
            var seq =
              from field in obj.GetType().GetFields()
              where !field.IsStatic
              let instantiated = self.MakeGenericMethod(field.FieldType)
              select instantiated.Invoke(null,new Object [] {field.GetValue(obj)});
            foreach (var ss in seq) {
              s += ss;
            }
            break;
          }
          return s;
        }
    }
}
