using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System;
using Factorized;

namespace Factorized.UI
{
    public class FItemSlot : UIElement
    {
        public Factorized.ItemReferrer getItem;
        public int itemContext;
        public delegate void ITEvent(FItemSlot target);
        public static event ITEvent PIT;
        public bool pressedRightClick = false;
        public int rightClickHoldTicks = 0;
        public int timer = 0;
        public FItemSlot (Factorized.ItemReferrer input,int context)
        {
            getItem = input;
            itemContext = context;
        }

        //PIT Stands for Post Item Transfer
        public void RaisePITEvent() {
            if(FItemSlot.PIT != null)
            {
                FItemSlot.PIT(this);
            }
            Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 position = GetDimensions().Center()
                + new Vector2(52f, 52f) * -0.5f * Main.inventoryScale;

            ItemSlot.Draw(spriteBatch,ref getItem(), itemContext, position);
        }

        //the cursor item isn't only Main.mouseItem but also player.inventory[58]
        public override void Click(UIMouseEvent evt)
        {
            if(evt.Target != this) return;
            ref Item remove = ref getItem();
            if (Main.mouseItem.IsAir)
            {
                if(!remove.IsAir)
                {
                    Main.mouseItem = remove;
                    Main.LocalPlayer.inventory[58] = remove;
                    remove = new Item ();
                }
            } else
            {
                if (remove.IsAir)
                {
                    remove = Main.mouseItem;
                    Main.mouseItem = new Item();
                    Main.LocalPlayer.inventory[58]= new Item();
                }
                else
                {
                    Item store = Main.mouseItem.Clone();
                    if(remove.type == Main.mouseItem.type)
                    {
                        if (remove.stack + Main.mouseItem.stack > remove.maxStack)
                        {
                            if (remove.stack == remove.maxStack || Main.mouseItem.stack == remove.maxStack ) {
                                Main.mouseItem = remove;
                                Main.LocalPlayer.inventory[58] = Main.mouseItem;
                                remove = store;
                            } else 
                            {
                                Main.mouseItem.stack += remove.stack - remove.maxStack;
                                remove.stack = remove.maxStack;
                                Main.LocalPlayer.inventory[58] =  Main.mouseItem;
                            }
                        }
                        else
                        {
                            remove.stack += Main.mouseItem.stack;
                            Main.mouseItem = new Item();
                            Main.LocalPlayer.inventory[58] = Main.mouseItem;
                        }
                    }
                    else
                    {
                        Main.mouseItem = new Item();
                        Main.mouseItem = remove;
                        Main.LocalPlayer.inventory[58] = Main.mouseItem;
                        remove = store;
                    }
                }
            }
            RaisePITEvent();
        }

        public override void RightClick(UIMouseEvent evt)
        {
            if(evt.Target != this) return;
            ref Item remove = ref getItem();
            if(Main.mouseItem.IsAir)
            {
                if(remove.IsAir) return;
                else {
                    remove.stack -= 1;
                    Main.mouseItem = remove.Clone();
                    Main.mouseItem.stack = 1;
                    Main.LocalPlayer.inventory[58] = Main.mouseItem;
                }
            } else
            {
                if (Main.mouseItem.type == remove.type ){
                    if (Main.mouseItem.stack >= Main.mouseItem.maxStack || remove.stack <= 0) return;
                    remove.stack -=1;
                    Main.mouseItem.stack += 1;
                    Main.LocalPlayer.inventory[58] = Main.mouseItem;
                }
            }
            RaisePITEvent();
        }
        
        public override void RightMouseDown(UIMouseEvent evt)
        {
            if(evt.Target != this) return;
            pressedRightClick = true;
            rightClickHoldTicks = 30;
        }
        public override void RightMouseUp(UIMouseEvent evt)
        {
            if(evt.Target != this) return;
            pressedRightClick = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            int timerThreshold = 30;
            if(ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if(pressedRightClick) {
                switch(rightClickHoldTicks){
                    case < 60:
                    break;
                    case < 2*60:
                    timerThreshold = 15;
                    break;
                    case < 4*60: 
                    timerThreshold = 5;
                    break;
                    default: 
                    timerThreshold = 1;
                    break;
                }
                if(timer >= timerThreshold){
                    RightClick(new UIMouseEvent(this,Main.MouseScreen));
                    timer = 0;
                }
                timer++;
                rightClickHoldTicks++;
            }
        }
    }
}