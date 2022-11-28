/*
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using Factorized.Machines;
using Factorized.Net;
using System;
using Factorized.Utility;
using Terraria.DataStructures;

namespace Factorized.Net.Server
{   //Server Machine Handler
    public static class SMH
    {
        private static List<MClient> clients {get; set;} = new();
        public class MClient
        {
            public int PN;
            public Point16 pos;
            public MClient(int p,Point16 pos)
            {
                PN = p;
                this.pos = pos;
            }
        }
        public enum RequestType
        {
            Swap,
            PickupIntoMouse,
            PickupAllIntoMouse,
            Deposit
        }

        public static void Subscribe(int whichClient, Point16 pos)
        {
            clients.Add(new (whichClient,pos));
            UpdateSend(pos,whichClient);
        }
        public static void Unsubscribe(int whichClient)
        {
            clients.RemoveAll((c) => c.PN == whichClient);
        }
        public static void UpdateSend(Point16 pos)
        {
            var m = MachineTE.Get(pos);
            if(m == null) return;

            foreach(var client in clients)
            {
                if(client.pos.X == pos.X && client.pos.Y == pos.Y)
                {
                    NetMessage.SendData(MessageID.TileEntitySharing,client.PN,-1,null,m.ID,pos.X,pos.Y);
                }
            }
        }
        public static void UpdateSend(Point16 pos, int whoami)
        {
            var m = MachineTE.Get(pos);
            if(m == null) return;
            NetMessage.SendData(MessageID.TileEntitySharing,whoami,-1,null,m.ID,pos.X,pos.Y);
        }
        public static void SubscribeToMachineHandler(BinaryReader reader, int whoami)
        {
            Subscribe(whoami, reader.ReadPoint16());
        }
        public static void UnsubscribeToMachineHandler(int whoami)
        {
            Unsubscribe(whoami);
        }
        public static void ModifyTESlotRequestHandler(BinaryReader reader,int whoami)
        {
            Point16 pos = reader.ReadPoint16();
            int slot = reader.ReadInt32();
            MachineSlotType type = (MachineSlotType)reader.ReadInt32();
            RequestType request = (RequestType)reader.ReadByte();
            Item slotItem = ItemIO.Receive(reader,true);
            Item mouseItem = ItemIO.Receive(reader,true);
            if(HasCorrectState(pos,slot,type,whoami,slotItem) && ValidateRequest(request,slotItem,mouseItem))
            {
                PeformRequest(request,pos,slot,type,whoami,mouseItem);
                SendPermission(whoami);
            }
            else
            {
                UpdateSend(pos,whoami);
            }
        }

        private static bool HasCorrectState(Point16 pos, int slot,MachineSlotType type,int whoami,
            Item slotItem)
        {
            var m = MachineTE.Get(pos);
            if(m == null) return false;
            var slots = m.GetSlots(type);
            if(slots.Count< slot) return false;
            var SLOT = slots[slot];
            if(Main.player.Length< whoami) return false;
            if(!(SLOT.IType == slotItem.type&& SLOT.stack == slotItem.stack)) return false;
            return true;
        }

        private static bool ValidateRequest(RequestType request, Item slotItem, Item mouseItem)
        {
            switch(request)
            {
                case RequestType.Swap:
                    return slotItem.stack > 0 && mouseItem.stack > 0 && mouseItem.type != slotItem.type;
                case RequestType.Deposit:
                    return mouseItem.stack > 0; 
                case RequestType.PickupAllIntoMouse:
                    return slotItem.stack > 0;
                case RequestType.PickupIntoMouse:
                    return slotItem.stack > 0;
                default: 
                    return false;
            }
        }

        public static void PeformRequest(RequestType request,Point16 pos,int slot,
            MachineSlotType type, int whoami,Item mouseItem)
        {
            var m = MachineTE.Get(pos);
            if (m == null) return;
            var s = m.GetSlots(type);
            if(s == null || s.Count< slot) return;
            var SLOT = s[slot];
            var player = Main.player[whoami];
            if(player == null) return;
            var inv = player.inventory;
            if(inv == null || inv.Length< 58) return;
            switch(request)
            {
                case RequestType.Swap: 
                    SLOT.SlotItem = mouseItem;
                    break;
                case RequestType.Deposit:
                    if(SLOT.IsAir) SLOT.SlotItem = mouseItem;
                    else SLOT.SlotItem.stack += mouseItem.stack;
                    break;
                case RequestType.PickupIntoMouse:
                    SLOT.SlotItem.stack -= 1;
                    break;
                case RequestType.PickupAllIntoMouse:
                    SLOT.SlotItem = new Item();
                    break;
                default:
                    break;
            }
        }

        private static void SendPermission(int whoami)
        {
            ModPacket packet = Factorized.Instance.GetPacket();
            packet.Write((int)MessageType.ServerModifyTESlot);
            packet.Send(whoami);
        }
    }
}
*/
