using Terraria;
using Terraria.ModLoader;

namespace Factorized.Runes
{
  public class RunePlayer : ModPlayer
  {
    public bool LRoV = false;

    public override void ResetEffects()
    {
      LRoV = false;
    }
    public override bool? CanAutoReuseItem(Item item)
    {
      if(LRoV) return true;
      return null;
    }
  }
}
