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
using System.Diagnostics;
using System.Reflection;

namespace Factorized
{
    public class Factorized : Mod
    {
        public static Factorized Instance = ModContent.GetInstance<Factorized>();
        public FUIController UI;
        public FNetController Net;
        /*
        public override void HandlePacket(BinaryReader reader, int whoami)
        {
            
            MessageType type = (MessageType) reader.ReadInt32();
            Logger.DebugFormat("received message of type{0}",type);
            try{
            switch(type)
            {
                case MessageType.ClientSubscribeToMachine:
                    SMH.SubscribeToMachineHandler(reader,whoami);
                    break;
                case MessageType.ClientUnsubscribeToMachine:
                    SMH.UnsubscribeToMachineHandler(whoami);
                    break;
                case MessageType.ClientModifyTESlotRequest:
                    SMH.ModifyTESlotRequestHandler(reader,whoami);
                    break;
                case MessageType.ServerModifyTESlot:
                    CMH.UpdateItems();
                    break;
                default:
                    break;
            }
            }catch(Exception e)
            {
                StackTrace stack = new StackTrace();
                Logger.DebugFormat("caught {0} {1}",e,stack);
                throw e;
            }
        }
        */
    }

    public delegate ref float FloatReferrer();
    public delegate ref int IntReferrer();
    public delegate ref Item ItemReferrer();
}

