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
        public static string ToLoggable<T>(this T obj) 
        {
          return ToLoggableHelper<T>(obj, new List<Object>());
        }
        private static string ToLoggableHelper<T>(T obj, List<Object> visited)
        {
          if (obj == null) {return "null";} 
          var copies = visited.Count();
          var s = "";
          var self = typeof(Lib).GetMethod("ToLoggableHelper",BindingFlags.NonPublic);
          var ieg =  typeof(IEnumerable<T>);
          var ie = typeof(IEnumerable);
          switch (obj)
          {
            case IEnumerable<T> l:
              var lal = l.ToArray();
              for (int j = 0; j < l.Count();j++) {
                s += "element " + j + ToLoggableHelper(lal[j],visited);
              }
              s = "[" + s + "]";
              break;
            case IEnumerable l:
              var i = 0;
              foreach (var el in l) {
                s += "element " + i + ToLoggableHelper(el,visited);
              }
              s = "[" + s + "]";
              break;
            default:
              var fields = obj.GetType().GetFields(BindingFlags.NonPublic 
                | BindingFlags.Public | BindingFlags.Instance);
              foreach (var field in fields)
              { 
                Object fv = field.GetValue(obj);
                string fvts = "";
                if (visited.Contains(fv)) {
                  fvts = "[[ revisited element " + visited.IndexOf(fv).ToString() + " ]]";
                } 
                else 
                {
                  visited.Add(fv);
                  fvts = ToLoggableHelper(fv,visited);
                }
                s += field.Name + ":" + fvts;
              }
              break;
          }
          if(visited.Count() > copies) { 
            s = "[[repeated element " + visited.IndexOf(obj) + "with info" + s +" ]]";
          }
          return s;
        }
        public static IEnumerable<FieldInfo> GetFieldsIwA<T>(this Type obj) {
          return from field in obj.GetFields( BindingFlags.NonPublic 
              | BindingFlags.Public | BindingFlags.Instance)
            where field.CustomAttributes.Any(a => a is T)
            select field;
        }
        public static void TransferItPF<T1,T2>(this T1 src,T2 dst) {
          var fields = dst.GetType().GetFieldsIwA<PersistentAttribute>();
        }
        public static IEnumerable<T2> LiftE<T1,T2>(this IEnumerable<T1> col, Func<T1,T2> lifted)
        {
          return from e in col
            select lifted(e);
        }
        public static T2 FoldE<T1,T2> (this IEnumerable<T1> col,Func<T2,T1,T2> foldF,T2 initial) 
        {
          T2 result = initial;
          foreach(var e in col) {
            result = foldF(result,e);
          }
          return result;
        }
    }
}
