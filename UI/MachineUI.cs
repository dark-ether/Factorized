using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Factorized.Machines;
using Factorized;
using Factorized.UI.Elements;
using Factorized.Utility;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;

namespace Factorized.UI {

  public class MachineUI : UIState
  {

    public Point16 MP;
    public MachineUI() {}
    public MachineUI(Point16 pos) {
      MP = pos;
    }
    public override void OnActivate()
    {
      var machine = MachineTE.Get(MP);
      if (machine == null) return;
      machine?.GenerateUI(this);
    }

    public override void OnDeactivate()
    {
      base.OnDeactivate();
      this.RemoveAllChildren();
    }
  }
}
