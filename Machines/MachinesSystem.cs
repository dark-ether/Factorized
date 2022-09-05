using Factorized.Machines.Processing.Furnace;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Factorized.Utility;

namespace Factorized.Machines
{
    public class MachinesSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            Type[] types = AssemblyManager.GetLoadableTypes(Mod.Code);
            IEnumerable<MethodInfo> methods =from type in types
            where Lib.HasAncestor(type,typeof(MachineTE))
            where !type.IsAbstract
            from machineMethod in type.GetMethods()
            where machineMethod.IsStatic
            where machineMethod.GetParameters().Length == 0
            select machineMethod;
            foreach(var method in methods)
            {
                method.Invoke(null,null);
            }
        }
    }
}
