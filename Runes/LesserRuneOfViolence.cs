using Terraria;
using Terraria.ModLoader;

namespace Factorized.Runes
{
    public class LesserRuneOfViolence : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("TODO:ADD suitable description");
        }

        public override void SetDefaults()
        {
            Item.rare = 0;
            Item.maxStack = 1;
            Item.buyPrice(0,0,0,10);
            Item.accessory = true;
        }
        public override void UpdateEquip(Player player)
        {
            RunePlayer p = player.GetModPlayer<RunePlayer>();
            p.LRoV = true;
        }
    }
}
