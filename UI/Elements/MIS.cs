using Terraria.UI;
using Factorized.Machines;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria.ID;
using Factorized.Net.Client;
using Factorized.Net.Server;
using Factorized.Utility;
using Terraria.DataStructures;

namespace Factorized.UI.Elements {
    public class MIS : UIElement
    {
        public enum CO
        {
            Swap,
            PickupIntoMouse,
            PickupAllIntoMouse,
            Deposit,
            Nothing,
        }
        public static Point16 Pos;
        public int slot = 0;
        public MachineSlotType type;
        public int itemContext = ItemSlot.Context.InventoryItem;
        public static Item[] dummy = new Item[11];
        public int LheldTicks = 0;
        public bool Lheld = false;
        public int LTimer = 0;
        public int RheldTicks = 0;
        public bool Rheld = false;
        public int RTimer = 0;
        static MIS()
        {
            for (int i =0; i< dummy.Length; i++)
            {
                dummy[i] = new Item ();
            }
        }

        public MachineSlot getSlot()
        {
            var m = MachineTE.Get(Pos);
            if(m == null) return null;
            return m.GetSlots(type)[slot];
        }

        public MIS(int slot,MachineSlotType type ,int context)
        {
            this.slot = slot;
            this.type = type;
            this.itemContext = context;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            dummy[10] = getSlot().SlotItem;
            Vector2 position = GetDimensions().Center()
                + new Vector2(52f, 52f) * -0.5f * Main.inventoryScale;

            ItemSlot.Draw(spriteBatch,dummy,itemContext,10, position);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            int threshold;
            if(ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if(Lheld)
            {
                switch(LheldTicks)
                {
                    case <30:
                        threshold = 30;
                        break;
                    case < 60:
                        threshold = 15;
                        break;
                    case < 90:
                        threshold = 8;
                        break;
                    case < 120: 
                        threshold = 4;
                        break;
                    default: 
                        threshold =1;
                        break;
                }
                if(LTimer >= threshold)
                {
                    LTimer = 0;
                    Click(new UIMouseEvent(this, Main.MouseScreen));
                }
                else
                {
                    LTimer++;
                }
            }
            if(Rheld)
            {
                switch(RheldTicks)
                {
                    case <30:
                        threshold = 30;
                        break;
                    case < 60:
                        threshold = 15;
                        break;
                    case < 90:
                        threshold = 8;
                        break;
                    case < 120: 
                        threshold = 4;
                        break;
                    default: 
                        threshold = 1;
                        break;
                }
                if(RTimer >= threshold)
                {
                    RTimer = 0;
                    RightClick(new UIMouseEvent(this, Main.MouseScreen));
                }
                else
                {
                    RTimer++;
                }
            }
            
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            LheldTicks = 0;
            Lheld = true;
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            Lheld = false;
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
        }

        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            
            switch(FindClickOperation())
            {
                case CO.Swap:
                    CMH.Request(Pos,slot,type,SMH.RequestType.Swap);
                    break;
                case CO.PickupAllIntoMouse:
                    CMH.Request(Pos,slot,type,SMH.RequestType.PickupAllIntoMouse);
                    break;
                case CO.Deposit:
                    CMH.Request(Pos,slot,type,SMH.RequestType.Deposit);
                    break;
                default:
                    break;
            }
        }

        private CO FindClickOperation()
        {
            if(Main.mouseItem.IsAir)
            {
                if(getSlot().IsAir) return CO.Nothing;
                else return CO.PickupAllIntoMouse;
            }
            else
            {
                if(getSlot().IsAir) return CO.Deposit;
                else if(getSlot().IType == Main.mouseItem.type 
                    && getSlot().stack + Main.mouseItem.stack <= getSlot().maxStack) return CO.Deposit;
                else return CO.Swap;
            }
        }
        private bool canPickupIntoMouse()
        {
            return (Main.mouseItem.IsAir || (Main.mouseItem.type == getSlot().IType 
                && Main.mouseItem.stack + getSlot().stack <= Main.mouseItem.maxStack))
                && !getSlot().IsAir;
        }

        public override void RightMouseDown(UIMouseEvent evt)
        {
            base.RightMouseDown(evt);
            RheldTicks = 0;
            Rheld = true;
        }

        public override void RightMouseUp(UIMouseEvent evt)
        {
            base.RightMouseUp(evt);
            Rheld = false;
        }

        public override void RightClick(UIMouseEvent evt)
        {
            base.RightClick(evt);
            if(canPickupIntoMouse())
            {
                CMH.Request(Pos,slot,type,SMH.RequestType.PickupIntoMouse);
            }
        }
    }
}
