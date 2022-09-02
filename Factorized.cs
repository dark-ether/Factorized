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
        public delegate ref Item ItemReferrer();
        public override void HandlePacket(BinaryReader reader, int whoami)
        {
            MessageType type = (MessageType) reader.ReadInt32();
            switch(type)
            {
                case MessageType.ClientModifyTESlot:
                    MessageHandler.ClientModifyTESlotHandler(reader,whoami);
                    break;
                case MessageType.ClientRequestUpdate:
                    MessageHandler.ClientRequestUpdateHandler(reader,whoami);
                    break;
                default:
                    break;
            }
        }
    }
    public delegate float FloatReferrer();
    public delegate int IntReferrer();
}

