using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using cookiefactorized.TE.machineTE;

namespace cookiefactorized.ui {

    class machineUI : UIState
    {
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
            //check if it is outside and if so close the ui

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
            base.OnActivate();
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
        }

        public override void OnInitialize()
        {
        
            UIPanel inputPanel = new UIPanel();
            inputPanel.Width.Set(0,0.20f);
            inputPanel.Height.Set(0,0.35f);
            inputPanel.VAlign = 0.6f;
            inputPanel.HAlign = 0.1f;
            
            inputPanel.Append(new UIText("number of timesClicked "));
            /*Item [] inputItems = new Item[4];
            UIItemSlot inputItemsSlot = new UIItemSlot(inputItems,1,1);
            inputItemsSlot.Width.Set(0,1f);
            inputItemsSlot.Height.Set(0,1f);
            inputPanel.Append(inputItemsSlot);*/
            Append(inputPanel);
            /*
            processingPanel.Width.Set(300,400);
            processingPanel.Height.Set(300,400);

            processingPanel.Append(new UIText(""));
            Append(processingPanel);

            outputPanel.Width.Set(100,0);
            outputPanel.Height.Set(200,0);
            
            Append(outputPanel);*/
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
            
            Elements[0].RemoveAllChildren();
            int timesClicked = 0;
            TileEntity entityInPosition;
            if(TileEntity.ByPosition.TryGetValue(new Point16(UICaller.melterX,UICaller.melterY),out entityInPosition)){
                if(entityInPosition is melterTE){
                    melterTE thisMelter = (melterTE) entityInPosition;
                    timesClicked = thisMelter.timesClicked;
                }
            }
            Elements[0].Append(new UIText($" clicked {timesClicked} times"));

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
