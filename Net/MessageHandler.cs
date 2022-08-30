using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.ID;
using Factorized;
using Factorized.TE.MachineTE;
using Factorized.Utility;

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
            MachineSlotType slotType = (MachineSlotType)reader.ReadInt32();
            int slotNumber = reader.ReadInt32();
            Item myItem = ItemIO.Receive(reader, true);
            if (!interacted.TryAddItemToSlot(slotType, slotNumber, myItem))
            {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null,
                        interacted.ID, interacted.Position.X, interacted.Position.Y);
                //TODO: fix edge case where players remove/add items at the same time
            }
        }
        public static void ClientModifyTESlotSend(int TEID,int slotType ,int slotNumber,Item myItem)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                return;
            }
            ModPacket packet = Factorized.mod.GetPacket();
            packet.Write((int)MessageType.ClientModifyTESlot);
            packet.Write(TEID);
            packet.Write(slotType);
            packet.Write(slotNumber);
            ItemIO.Send(myItem,packet,true);
            packet.Send();
        }
    }
}
