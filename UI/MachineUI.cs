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

namespace Factorized.UI {

    class MachineUI : UIState
    {

        public List<FItemSlot> inputItems;
        public List<FItemSlot> outputItems;
        protected UIPanel inputPanel;
        protected UIPanel outputPanel;
        protected UIPanel processingPanel;
        protected Asset<Texture2D> fullFire;
        protected Asset<Texture2D> emptyFire;
        public bool gotEmptyFire;
        public bool gotFullfire;
        private void itemSlotConfig(int index,int numberofSlots, FItemSlot itemSlot){
            itemSlot.Height.Set(30,0f);
            itemSlot.Width.Set(30,0f);
            itemSlot.HAlign = ((float)index)/((float)numberofSlots);
            itemSlot.VAlign = 0.25f;
        }

        public override void OnActivate()
        {
            inputItems = new List<FItemSlot>();
            outputItems = new List<FItemSlot>();
            FItemSlot.PIT += UICaller.machineSynchronizer;
            ref MachineTE machine = ref UICaller.machine;
            for (int i = 0; i < machine.inputSlots.Length; i++)
            {
                FItemSlot itemSlot = new (machine.InputSlotRef(i),3);
                itemSlotConfig(i,machine.inputSlots.Length,itemSlot);
                inputItems.Add(itemSlot);
                inputPanel.Append(itemSlot);
            }
            if(gotEmptyFire && gotFullfire)
            {
                var FireImage = new FProgressBar(fullFire,emptyFire,
                    () => {
                    if(UICaller.machine?.machineState.currentProcess?.processingTime== null)
                        return 0;
                    else
                    return (int)UICaller.machine?.machineState.timer;
                    },
                    () => {
                    var limit = UICaller.machine?.machineState.currentProcess?.processingTime;
                    if(limit == null) return 1; else return (int)limit;});
                FireImage.Height.Set(40,0f);
                FireImage.Width.Set(40,0f);
                FireImage.HAlign = 0.5f;
                FireImage.VAlign = 0.5f;
                processingPanel.Append(FireImage);
            }

            for (int i = 0; i < machine.outputSlots.Length;i++)
            {
                FItemSlot itemSlot = new (machine.OutputSlotRef(i),3);
                itemSlotConfig(i,machine.outputSlots.Length, itemSlot);
                outputItems.Add(itemSlot);
                outputPanel.Append(itemSlot);
            }
        }


        public override void OnDeactivate()
        {
            base.OnDeactivate();

            FItemSlot.PIT -= UICaller.machineSynchronizer;
            inputItems = null;
            outputItems = null;
            inputPanel.RemoveAllChildren();
            processingPanel.RemoveAllChildren();
            outputPanel.RemoveAllChildren();
        }

        public override void OnInitialize()
        {
            inputPanel = new UIPanel();
            inputPanel.Width.Set(300,0f);
            inputPanel.Height.Set(150,0f);
            inputPanel.VAlign = 0.39f;
            inputPanel.HAlign = 0.1f;

            Append(inputPanel);

            processingPanel = new UIPanel();
            processingPanel.Width.Set(300,0f);
            processingPanel.Height.Set(200,0f);
            processingPanel.HAlign = 0.1f;
            processingPanel.VAlign = 0.65f;

            gotFullfire = ModContent.RequestIfExists<Texture2D>("Factorized/Assets/full_fire",out fullFire, AssetRequestMode.ImmediateLoad);
            gotEmptyFire = ModContent.RequestIfExists<Texture2D>("Factorized/Assets/empty_fire",out emptyFire,AssetRequestMode.ImmediateLoad);


            Append(processingPanel);

            outputPanel = new UIPanel();
            outputPanel.Width.Set(300,0);
            outputPanel.Height.Set(150,0);
            outputPanel.HAlign = 0.1f;
            outputPanel.VAlign = 0.88f;
            Append(outputPanel);
        }
    }
}
