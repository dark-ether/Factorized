using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System;

namespace Factorized.UI
{
    public class MachineItemSlot : UIElement
    {
        public delegate ref Item GetItem();
        public GetItem getItem;
        public int itemContext;
        public static Item [] dummy = new Item [11];
        public MachineItemSlot (GetItem input,int context)
        {
            getItem = input;
            itemContext = context;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 position = GetDimensions().Center() 
                + new Vector2(52f, 52f) * -0.5f * Main.inventoryScale;

            ItemSlot.Draw(spriteBatch,ref getItem(), itemContext, position);
        }
        public override void Click(UIMouseEvent evt)
        {
            if(evt.Target != this) return;
            Item remove = getItem();
            if (Main.mouseItem.IsAir)
            {
                if(!remove.IsAir)
                {
                    Main.mouseItem = remove;
                    remove = new Item ();
                }
            } else 
            {
                if (remove.IsAir)
                {
                    remove = Main.mouseItem;
                    Main.mouseItem = new Item();
                } else
                {
                    Item store = Main.mouseItem;
                    if(remove.type == Main.mouseItem.type) 
                    {
                        if (remove.stack + Main.mouseItem.stack >= remove.maxStack)
                        {
                            remove.stack = remove.maxStack;
                            Main.mouseItem.stack = remove.stack + Main.mouseItem.stack - remove.maxStack;
                        }
                        else remove.stack += Main.mouseItem.stack;
                    }
                    else 
                    {
                        Main.mouseItem = remove;
                        remove = store;
                    }
                }
            }
        }
        public override void RightClick(UIMouseEvent evt)
        {
            if(evt.Target != this) return;
            Item remove = getItem();
            if(Main.mouseItem.IsAir) 
            {
                if(remove.IsAir) return;
                else {
                    remove.stack -= 1;
                    Main.mouseItem = remove.Clone();
                    Main.mouseItem.stack = 1; 
                }
            } else 
            {
                if (Main.mouseItem.type == remove.type ){
                    remove.stack -=1;
                    Main.mouseItem.stack +=1;
                }
            }
        }
    }
}
