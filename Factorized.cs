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
using Factorized.Net.Server;
using Factorized.Net.Client;
using System.Diagnostics;
using System.Reflection;

namespace Factorized
{ 
    public class Factorized : Mod
    {
        public static Factorized mod = ModContent.GetInstance<Factorized>();
        public delegate ref Item ItemReferrer();
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
    }
    public delegate float FloatReferrer();
    public delegate int IntReferrer();
}

