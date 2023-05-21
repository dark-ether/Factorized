using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Factorized.Utility;

namespace Factorized.Machines
{
  internal class MachineSystem : ModSystem
  {
    public Dictionary<Type,IEnumerable<FieldInfo>> fieldsPerType = new ();
    public override void OnModLoad(){
      MachineTE.system = this;
      Dictionary<Type,IEnumerable<FieldInfo>> temp = new ();
      IEnumerable<Type> mtypes = 
        from mod in ModLoader.Mods
        from type in AssemblyManager.GetLoadableTypes(mod.Code)
        where typeof(MachineTE).IsAssignableFrom(type)
        select type;
      foreach(var mt in mtypes){
        temp[mt] = mt.GetFields(BindingFlags.NonPublic | BindingFlags.Public 
            | BindingFlags.Instance | BindingFlags.DeclaredOnly)
          .Where(elem => elem.GetCustomAttributes().Any( t => t is MachineDataAttribute));
      }
      foreach(var t in temp.Keys) {
        var mf = temp[t];
        Type bt = t;
        while(temp.ContainsKey(bt.BaseType)){
          bt = bt.BaseType;
          mf = mf.Concat(temp[bt]);
        }
        fieldsPerType[t] = mf;
      }
    }
  }
}
