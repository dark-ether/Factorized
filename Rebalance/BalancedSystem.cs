using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Factorized.Rebalance {
    public class BalancedSystem : ModSystem
    {
        public override void AddRecipes()
        {
            Recipe magicMirror = Recipe.Create(ItemID.MagicMirror);
            magicMirror.AddIngredient(ItemID.RecallPotion, 5);
            magicMirror.AddIngredient(ItemID.Glass, 5);
            magicMirror.AddIngredient(ItemID.StoneBlock,10);
            magicMirror.AddTile<Content.EngineeringTable.EngineeringTableTile>();
            magicMirror.Register();
        }
    }
}
