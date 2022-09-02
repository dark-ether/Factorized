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

namespace Factorized.UI{
    public class UIManager : ModSystem
    {
        internal static UserInterface machineInterface; //user interface
        internal static MachineUI currentMachineUI; // machine ui in this case a melter
        private static GameTime _lastUpdateUiGameTime;
        public static Point16 machinePos;
        public static List<MachineSlot>Copy;
        public static Item [] outputCopy;
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
                    MachineTE machine = Lib.GetMachineTE(machinePos);
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
            machinePos = new Point16(machineX,machineY);
            if (!TileEntity.ByPosition.TryGetValue(machinePos,out target)) return;
            if (!(target is MachineTE)) return;
            MachineTE machine = (MachineTE) target;
            Copy = new (machine.GetSlots());
            machineInterface?.SetState(currentMachineUI);
        }

        public static void hideMachineUI()
        {
            visibleUI = "";
            machineInterface?.SetState(null);
            machinePos = new ();
        }

        public override void OnWorldUnload()
        {
            machineInterface?.SetState(null);
        }
        public static void machineSynchronizer(FItemSlot target)
        {
            MachineTE machine = Lib.GetMachineTE(machinePos);
            int? index = null;
            for (int i=0;i< machine.GetSlots().Count;i++)
            {
                if(Copy[i].stack != machine.GetSlots()[i].stack
                    || Copy[i].IType != machine.GetSlots()[i].IType)
                {
                    index = i;
                    break;
                }
            }
            if(index != null ) {
                MessageHandler.ClientModifyTESlotSend(machine.ID,
                    (int)index,machine.GetItems()[(int)index]);
            }
            UIManager.Copy = new (machine.GetSlots());
        }
    }
}
