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

        public Asset<Texture2D> fullFire;
        public Asset<Texture2D> emptyFire;
        public bool gotEmptyFire;
        public bool gotFullfire;
        private void itemSlotConfig(int index,int numberofSlots, FItemSlot itemSlot){
            
        }

        public override void OnActivate()
        {
            var machine = MachineTE.Get(UIManager.MP);
            if (machine == null) return;
            machine?.GenerateUI(this);
        }


        public override void OnDeactivate()
        {
            base.OnDeactivate();
            this.RemoveAllChildren();
        }

        public override void OnInitialize()
        {
            gotFullfire = ModContent.RequestIfExists<Texture2D>(
                    "Factorized/Assets/full_fire",out fullFire, AssetRequestMode.ImmediateLoad);
            gotEmptyFire = ModContent.RequestIfExists<Texture2D>(
                    "Factorized/Assets/empty_fire",out emptyFire,AssetRequestMode.ImmediateLoad);

        }
    }
}
