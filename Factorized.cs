using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using ReLogic.Content.Sources;
using System.IO;
using Factorized.Net;

namespace Factorized
{ 
    public class Factorized : Mod
    {
        public static Factorized mod = ModContent.GetInstance<Factorized>();
        public override void HandlePacket(BinaryReader reader, int whoami)
        {
            Logger.Info("received packet");
            MessageType type = (MessageType) reader.ReadInt32();
            Factorized.mod.Logger.Info("received packet");
            switch(type)
            {
                case MessageType.ClientModifyTESlot:
                    MessageHandler.ClientModifyTESlotHandler(reader,whoami);
                    break;
                /*
                case MessageType.ServerRejectTEModify:
                    MessageHandler.ServerRejectTEModifyHandler(reader,whoami);
                    break;
                */
                default:
                    Factorized.mod.Logger.Warn("unknown message type");
                    break;
            }
        }
    }
}

