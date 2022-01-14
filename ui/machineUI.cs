using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using factorized.TE.machineTE;

namespace factorized.ui {

    class machineUI : UIState
    {
        
        protected List<UIItemSlot> inputItems;
        protected List<UIItemSlot> outputItems;
        protected UIPanel inputPanel;
        protected UIPanel outputPanel;
        protected UIPanel processingPanel;
        private void itemSlotConfig(int index,int numberofSlots, UIItemSlot itemSlot){
        itemSlot.Height.Set(25,0f);
        itemSlot.Width.Set(25,0f);
        itemSlot.HAlign = ((float)index)/((float)numberofSlots);
        itemSlot.VAlign = 0.25f;
        }

        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
        }

        public override int CompareTo(object obj)
        {
            return base.CompareTo(obj);
        }

        public override bool ContainsPoint(Vector2 point)
        {
            return base.ContainsPoint(point);
        }

        public override void DoubleClick(UIMouseEvent evt)
        {
            base.DoubleClick(evt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override void ExecuteRecursively(UIElementAction action)
        {
            base.ExecuteRecursively(action);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override List<SnapPoint> GetSnapPoints()
        {
            return base.GetSnapPoints();
        }

        public override Rectangle GetViewCullingArea()
        {
            return base.GetViewCullingArea();
        }

        public override void MiddleClick(UIMouseEvent evt)
        {
            base.MiddleClick(evt);
        }

        public override void MiddleDoubleClick(UIMouseEvent evt)
        {
            base.MiddleDoubleClick(evt);
        }

        public override void MiddleMouseDown(UIMouseEvent evt)
        {
            base.MiddleMouseDown(evt);
        }

        public override void MiddleMouseUp(UIMouseEvent evt)
        {
            base.MiddleMouseUp(evt);
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
        }

        public override void OnActivate()
        {
            ModContent.GetInstance<factorized>().Logger.Debug("called onactivate of machineUI");
            base.OnActivate();
            inputItems = new List<UIItemSlot>();
            outputItems = new List<UIItemSlot>();
            TileEntity entityInPosition;
            if(TileEntity.ByPosition.TryGetValue(new Point16(UICaller.machineX,UICaller.machineY),out entityInPosition)  && entityInPosition is machineTE){
                ModContent.GetInstance<factorized>().Logger.Debug("found tile entity");
                machineTE machine = (machineTE)entityInPosition;
                for (int i = 0; i < machine.inputSlotsNumber; i++)
                {
                    UIItemSlot itemSlot = new UIItemSlot(machine.inputSlots,i,0);
                    itemSlotConfig(i,machine.outputSlotsNumber,itemSlot);
                    inputItems.Add(itemSlot);
                    inputPanel.Append(itemSlot);
                }
                for(int i = 0; i < machine.outputSlotsNumber;i++)
                {
                   UIItemSlot itemSlot = new UIItemSlot(machine.outputSlots,i,0);
                    itemSlotConfig(i,machine.outputSlotsNumber, itemSlot);
                    outputItems.Add(itemSlot);
                    outputPanel.Append(itemSlot);
                }
            }
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
        }

        public override void OnInitialize()
        {
            ModContent.GetInstance<factorized>().Logger.Debug("onInitialize happened"); 
            inputPanel = new UIPanel();
            inputPanel.Width.Set(300,0f);
            inputPanel.Height.Set(200,0f);
            inputPanel.VAlign = 0.6f;
            inputPanel.HAlign = 0.1f;
            
            Append(inputPanel);

            processingPanel = new UIPanel(); 
            processingPanel.Width.Set(300,0f);
            processingPanel.Height.Set(75,0f);
            processingPanel.HAlign = 0.1f;
            processingPanel.VAlign = 0.7f;
    
            Append(processingPanel);
            
            outputPanel = new UIPanel();
            outputPanel.Width.Set(300,0);
            outputPanel.Height.Set(200,0);
            outputPanel.HAlign = 0.1f;
            outputPanel.VAlign = 0.8f;
            Append(outputPanel);
        }

        public override void Recalculate()
        {
            base.Recalculate();
        }

        public override void RecalculateChildren()
        {
            base.RecalculateChildren();
        }

        public override void RightClick(UIMouseEvent evt)
        {
            base.RightClick(evt);
        }

        public override void RightDoubleClick(UIMouseEvent evt)
        {
            base.RightDoubleClick(evt);
        }

        public override void RightMouseDown(UIMouseEvent evt)
        {
            base.RightMouseDown(evt);
        }

        public override void RightMouseUp(UIMouseEvent evt)
        {
            base.RightMouseUp(evt);
        }

        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            base.ScrollWheel(evt);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void XButton1Click(UIMouseEvent evt)
        {
            base.XButton1Click(evt);
        }

        public override void XButton1DoubleClick(UIMouseEvent evt)
        {
            base.XButton1DoubleClick(evt);
        }

        public override void XButton1MouseDown(UIMouseEvent evt)
        {
            base.XButton1MouseDown(evt);
        }

        public override void XButton1MouseUp(UIMouseEvent evt)
        {
            base.XButton1MouseUp(evt);
        }

        public override void XButton2Click(UIMouseEvent evt)
        {
            base.XButton2Click(evt);
        }

        public override void XButton2DoubleClick(UIMouseEvent evt)
        {
            base.XButton2DoubleClick(evt);
        }

        public override void XButton2MouseDown(UIMouseEvent evt)
        {
            base.XButton2MouseDown(evt);
        }

        public override void XButton2MouseUp(UIMouseEvent evt)
        {
            base.XButton2MouseUp(evt);
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            base.DrawChildren(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }
    }
}
