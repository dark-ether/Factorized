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
      return ToLoggableHelper<T>(obj, new List<Object>(), 0);
    }
    private static string ToLoggableHelper<T>(T obj, List<Object> visited,int indent)
    {
      if (obj.IsLoggable()) return obj.ToString();
      if (obj is null) return "null";
      string ii = "";
      for(int i = 0; i < indent; i++) ii += " ";
      string endl = "\n" + ii;
      var copies = visited.Count();
      var s = "";
      switch (obj)
      {
      case IEnumerable<T> l:
        var lal = l.ToArray();
        for (int j = 0; j < l.Count(); j++) {
          s += endl + "  element " + j + ": " + ToLoggableHelper(lal[j] , visited, indent + 2);
        }
        s = "[" + s + endl +"]";
        break;
      case IEnumerable l:
        var i = 0;
        foreach (var el in l) {
          s += endl + "  element " + i + ": " + ToLoggableHelper(el, visited, indent + 2);
          i++;
        }
        s = "[" + s + endl + "]";
        break;
      default:
        var fields = obj.GetType().GetFields(BindingFlags.NonPublic
          | BindingFlags.Public | BindingFlags.Instance);
        s += "{" + endl;
        foreach (var field in fields)
        {
          Object fv = field.GetValue(obj);
          if (fv is null) s += field.Name + ": null" + endl;
          if (fv.IsLoggable()){
            s += field.Name +": "+ fv.ToString() + endl; 
            continue;
          } 
          string fvts = "";
          if (visited.Contains(fv)) {
            fvts += "%% revisited #" + visited.IndexOf(fv) + " %%";
          }
          else
          {
            fvts += " #"+ visited.Count() + " ";
            visited.Add(fv);
            fvts +=  ToLoggableHelper(fv,visited, indent+2);
          }
          s += "  " + field.Name + ": " + fvts + endl;
        }
        s+= "}";
        break;
      }
      // TODO: why?
      /*if(visited.Count() > copies) {
        s = "%% repeated element " + visited.IndexOf(obj) + "with info" + s +" %%";
      }*/
      return s;
    }
    public static IEnumerable<FieldInfo> GetFieldsIwA<T>(this Type obj) {
      return from field in obj.GetFields( BindingFlags.NonPublic
                                          | BindingFlags.Public | BindingFlags.Instance)
             where Attribute.GetCustomAttributes(field).Any(f => f is T)
             select field;
    }
    public static T2 FoldE<T1,T2> (this IEnumerable<T1> col,Func<T2,T1,T2> foldF,T2 initial)
    {
      T2 result = initial;
      foreach(var e in col) {
        result = foldF(result,e);
      }
      return result;
    }
    public static bool IsLoggable<T>(this T obj) {
      switch(obj){
      case sbyte:
      case byte:
      case short:
      case ushort:
      case int:
      case uint:
      case long:
      case ulong:
      case nint:
      case nuint:
      case float:
      case double:
      case decimal:
      case bool:
      case char:
      case string ss:
        return true;
      default:
        return false;
      }
    }
    public static IEnumerable<T> LogAll<T>(this IEnumerable<T> list) {
      Factorized.Instance.Logger.DebugFormat("{0}",list.ToLoggable());
      return list;
    }
  }
}
