using Terraria.ModLoader;
using Terraria;
using Terraria.UI;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using Factorized.Machines;
using Terraria.DataStructures;
using Factorized.Utility;
using Factorized.Net;
using Terraria.ModLoader.IO;
using Factorized.UI.Elements;
using Factorized.Net.Client;

namespace Factorized.UI{
    public class UIManager : ModSystem
    {
        internal static UserInterface machineInterface; //user interface
        internal static MachineUI currentMachineUI;
        private static GameTime _lastUpdateUiGameTime;
        public static Point16 MP;
        public static List<MachineSlot>Copy;
        private static string visibleUI = "";

        public override void Load()
        {
            if(!Main.dedServ){
                machineInterface = new UserInterface();//initializes user interface

                currentMachineUI = new MachineUI();
                currentMachineUI.Initialize();
            }
        }

        public override void Unload()
        {
            currentMachineUI = null;// unloads the ui
            machineInterface = null;

        }

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (machineInterface?.CurrentState != null){// checking for nullity
                machineInterface.Update(gameTime);
                if(visibleUI == "machineUI"){
                    var machine = MachineTE.Get(MP);
                    if(!Main.playerInventory){
                    hideMachineUI();
                    }
                    if (machine == null) {
                        hideMachineUI();
                        return;
                    }
                    Vector2 melterPosition = new Vector2(machine.Position.X*16,
                    machine.Position.Y*16);//changing to world coordinates
                    float distance = Vector2.Distance(melterPosition,Main.LocalPlayer.Center);
                    if(distance > 5*16){//5 tiles
                        hideMachineUI();
                    }
                }
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if(mouseTextIndex != -1){
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Factorized: machineInterface",
                    delegate
                            {
                            if(_lastUpdateUiGameTime != null && machineInterface?.CurrentState != null){
                                    machineInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                            }
                                    return true;
                    },
                            InterfaceScaleType.UI));
            }
        }

        public static void showMachineUI(int x, int y){
            hideMachineUI();
            visibleUI = "machineUI";
            Main.playerInventory = true;
            Tile clickedTile = Main.tile[x,y];//this code finds the topleft tile of a multitile
            int machineX = x - clickedTile.TileFrameX/18;
            int machineY = y - clickedTile.TileFrameY/18;
            TileEntity target;
            MP = new Point16(machineX,machineY);
            if (!TileEntity.ByPosition.TryGetValue(MP,out target)) return;
            if (!(target is MachineTE)) return;
            MachineTE machine = (MachineTE) target;
            MIS.Pos = MP;
            CMH.Subscribe(MP);
            Copy = new (machine.GetSlots());
            machineInterface?.SetState(currentMachineUI);
        }

        public static void hideMachineUI()
        {
            visibleUI = "";
            machineInterface?.SetState(null);
            CMH.Unsubscribe();
        }

        public override void OnWorldUnload()
        {
            machineInterface?.SetState(null);
        }
    }
}
