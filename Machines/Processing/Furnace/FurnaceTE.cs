using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ID;
using Factorized.Utility;
using Terraria;
using System.Text.RegularExpressions;

namespace Factorized.Machines.Processing.Furnace 
{
    public class FurnaceTE : MachineTE
    {
        public override int ValidTile => ModContent.TileType<FurnaceTile>();

        public override List<MachineProcess> allProcesses => Recipes;

        public override Dictionary<MachineSlotType, int> SlotsComposition => 
            new (){
                [MachineSlotType.Input] = 2, 
                [MachineSlotType.Output] = 1
            };
        public static List<MachineProcess> Recipes;
        public static void SetupRecipes()
        {
            FurnaceTE.Recipes = new ();
            List<int> bars = new ();
            List<Item> fuels = new (){new (ItemID.Wood,5),new (ItemID.Gel,2)};
            string barRegex = ".+bar";
            foreach(var recipe in Main.recipe)
            {
                if(recipe == null) continue;
                if(!recipe.HasTile(TileID.Furnaces)) continue;
                if(Regex.Matches(barRegex,recipe.createItem.AffixName(),
                    RegexOptions.IgnoreCase).Count > 0) continue ;
                if(recipe.requiredItem.Count == 1)
                {
                    foreach(var fue in fuels){
                        MachineProcess p = new ();
                        p.Consume.Add(fue.Clone());
                        p.Consume.Add(new (recipe.requiredItem[0].type,2));
                        p.Produce.Add(new (recipe.createItem.type,1));
                        FurnaceTE.Recipes.Add(p);
                    }
                }
            }
        }

    }
}
