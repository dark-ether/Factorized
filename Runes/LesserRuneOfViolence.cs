using Terraria;
using Terraria.ModLoader;

namespace Factorized.Runes
{
  // TODO: rework item
  public class LesserRuneOfViolence : ModItem
  {
    public override void SetStaticDefaults()
    {
    }

    public override void SetDefaults()
    {
      Item.rare = 0;
      Item.maxStack = 1;
      Item.buyPrice(0,0,0,10);
      Item.accessory = true;
    }
  }
}
