using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.ID;
using Factorized.Machines;
using Factorized.Utility;
using System;

namespace Factorized.Net 
{ 
    public static class MessageHandler 
    {
        public static void ClientModifyTESlotHandler(BinaryReader reader,int whoami)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                return;
            }
            int id = reader.ReadInt32();
            TileEntity target;
            if(!TileEntity.ByID.TryGetValue(id, out target))
            {
                return;
            }
            if (!(target is MachineTE))
            {
                return;
            }
            MachineTE interacted = (MachineTE)target;
            int slotNumber = reader.ReadInt32();
            Item myItem = ItemIO.Receive(reader, true);
            interacted.TryAddItemToSlot(slotNumber, myItem);
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null,
                interacted.ID, interacted.Position.X, interacted.Position.Y);
        }
        public static void ClientModifyTESlotSend(int TEID,int slotNumber,Item myItem)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                return;
            }
            ModPacket packet = Factorized.mod.GetPacket();
            packet.Write((int)MessageType.ClientModifyTESlot);
            packet.Write(TEID);
            packet.Write(slotNumber);
            ItemIO.Send(myItem,packet,true);
            packet.Send();
        }

        public static void ClientRequestUpdateSend(Point16 position,int id)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient) return;
            ModPacket packet = Factorized.mod.GetPacket();
            packet.Write((int)MessageType.ClientRequestUpdate);
            packet.Write(position.X);
            packet.Write(position.Y);
            packet.Write(id);
            packet.Send();
        }
        public static void ClientRequestUpdateHandler(BinaryReader reader, int whoami)
        {
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();
            int ID = reader.ReadInt32();
            NetMessage.SendData(MessageID.TileEntitySharing,-1,-1,null,ID,x,y);
        }
    }
}
