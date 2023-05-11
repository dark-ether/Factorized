using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria;

namespace Factorized.Rebalance
{
  public class BalancedPlayer : ModPlayer
  {
    public override void OnEnterWorld()
    {
      if(Player.statLifeMax< 200) Player.statLifeMax = 200;
    }
  }
}
