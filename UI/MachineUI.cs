using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Factorized.TE.MachineTE;
using Factorized;
using Factorized.Utility;

namespace Factorized.UI {

    class machineUI : UIState
    {
        
        protected List<UIItemSlot> inputItems;
        protected List<UIItemSlot> outputItems;
        protected UIPanel inputPanel;
        protected UIPanel outputPanel;
        protected UIPanel processingPanel;

        private void itemSlotConfig(int index,int numberofSlots, UIItemSlot itemSlot){
            itemSlot.Height.Set(30,0f);
            itemSlot.Width.Set(30,0f);
            itemSlot.HAlign = ((float)index)/((float)numberofSlots);
            itemSlot.VAlign = 0.25f;
        }

        public override void OnActivate()
        {
            Factorized.mod.Logger.Info("Called OnActivate of Machine UI");
            inputItems = new List<UIItemSlot>();
            outputItems = new List<UIItemSlot>();
            ItemSlot.OnItemTransferred += UICaller.machineSynchronizer;
            TileEntity entityInPosition;
            if(TileEntity.ByPosition.TryGetValue(
                new Point16(UICaller.machine.Position.X,UICaller.machine.Position.Y),
                out entityInPosition)  && entityInPosition is MachineTE)
            {
                MachineTE machine = (MachineTE)entityInPosition;
                for (int i = 0; i < machine.inputSlots.Length; i++)
                {
                    UIItemSlot itemSlot = new (machine.inputSlots,i,3);
                    itemSlotConfig(i,machine.outputSlots.Length,itemSlot);
                    inputItems.Add(itemSlot);
                    inputPanel.Append(itemSlot);
                }
                for (int i = 0; i < machine.outputSlots.Length;i++)
                {
                   UIItemSlot itemSlot = new UIItemSlot(machine.outputSlots,i,3);
                    itemSlotConfig(i,machine.outputSlots.Length, itemSlot);
                    outputItems.Add(itemSlot);
                    outputPanel.Append(itemSlot);
                }
            }
        }


        public override void OnDeactivate()
        {
            base.OnDeactivate();
            
            ItemSlot.OnItemTransferred -= UICaller.machineSynchronizer;
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
    
            Append(processingPanel);
            
            outputPanel = new UIPanel();
            outputPanel.Width.Set(300,0);
            outputPanel.Height.Set(150,0);
            outputPanel.HAlign = 0.1f;
            outputPanel.VAlign = 0.88f;
            Append(outputPanel);
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
