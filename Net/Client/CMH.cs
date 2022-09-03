using Factorized.Net.Server;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Factorized.Utility;
using Factorized.Machines;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

namespace Factorized.Net.Client {
    public static class CMH
    {
        public static Point16 pos;
        public static int slot;
        public static MachineSlotType type;
        public static SMH.RequestType request;
        public static void Subscribe(Point16 pos)
        {
            if(Main.netMode == NetmodeID.SinglePlayer) return;
            ModPacket packet = Factorized.mod.GetPacket();
            packet.Write((int)MessageType.ClientSubscribeToMachine);
            packet.Write(pos);
            packet.Send();
        }
        public static void Unsubscribe()
        {
            if(Main.netMode == NetmodeID.SinglePlayer) return;
            ModPacket packet = Factorized.mod.GetPacket();
            packet.Write((int)MessageType.ClientUnsubscribeToMachine);
            packet.Send();
        }
        public static void Request(Point16 pos, int slot,MachineSlotType type, SMH.RequestType request)
        {
            CMH.pos = pos;
            CMH.slot = slot;
            CMH.type = type;
            CMH.request = request;
            var m = MachineTE.Get(pos);
            Item slotItem = m.GetSlots(type)[slot].SlotItem;
            Item mouseItem = Main.mouseItem;
            if(Main.netMode == NetmodeID.SinglePlayer){UpdateItems(); return;}
            ModPacket packet = Factorized.mod.GetPacket();
            packet.Write((int)MessageType.ClientModifyTESlotRequest);
            packet.Write(pos);
            packet.Write(slot);
            packet.Write((int)type);
            packet.Write((byte)request);
            ItemIO.Send(slotItem, packet,true);
            ItemIO.Send(mouseItem,packet,true);
            packet.Send();

        }
        public static void UpdateItems()
        { 
            var m = MachineTE.Get(pos);
            var slot = m.GetSlots(type)[CMH.slot];
            var mi = Main.mouseItem;
            switch(request)
            {
                case SMH.RequestType.Swap:
                    Main.mouseItem = slot.SlotItem;
                    Main.LocalPlayer.inventory[58] = slot.SlotItem;
                    slot.SlotItem = mi;
                    break;
                case SMH.RequestType.Deposit:
                    if(slot.IsAir) slot.SlotItem = mi;
                    else slot.SlotItem.stack += mi.stack;
                    Main.mouseItem = new Item();
                    Main.LocalPlayer.inventory[58] = new Item();
                    break;
                case SMH.RequestType.PickupAllIntoMouse:
                    if(mi.IsAir)
                    {
                        Main.mouseItem = slot.SlotItem;
                        Main.LocalPlayer.inventory[58]= slot.SlotItem;
                    }
                    else 
                    {
                        Main.mouseItem.stack += slot.stack;
                        Main.LocalPlayer.inventory[58].stack += slot.stack;
                    }
                    slot.SlotItem = new Item ();
                    break;
                case SMH.RequestType.PickupIntoMouse:
                    var copy  = slot.SlotItem.Clone();
                    copy.stack = 1;
                    if(mi.IsAir)
                    {
                        Main.mouseItem = copy;
                        Main.LocalPlayer.inventory[58] = copy;
                    }
                    else
                    {
                        Main.mouseItem.stack += 1;
                        Main.LocalPlayer.inventory[58].stack += 1; 
                    }
                    slot.SlotItem.stack -=1;
                    if(slot.stack <=0 )
                    {
                        slot.SlotItem = new Item ();
                    }
                    break;

            }
        }
    }
}
