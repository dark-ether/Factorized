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
    public class UICaller : ModSystem
    {
        internal static UserInterface machineInterface; //user interface
        internal static MachineUI currentMachineUI; // machine ui in this case a melter
        private static GameTime _lastUpdateUiGameTime;
        public static MachineTE machine;
        public static Item [] inputCopy;
        public static Item [] outputCopy;
        private static string visibleUI = "";
        public override string Name => base.Name;

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
                    if(!Main.playerInventory){
                    hideMachineUI();
                    }
                    if (machine == null) {
                        hideMachineUI();
                        return;
                    }
                    Vector2 melterPosition = new Vector2(UICaller.machine.Position.X*16,
                        UICaller.machine.Position.Y*16);//changing to world coordinates
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
            if (!TileEntity.ByPosition.TryGetValue(new Point16(machineX,machineY),out target)) return;
            if (!(target is MachineTE)) return;
            machine = (MachineTE) target;
            inputCopy = Lib.cloneItemArray(machine.inputSlots);
            outputCopy = Lib.cloneItemArray(machine.outputSlots);
            machineInterface?.SetState(currentMachineUI);
        }

        public static void hideMachineUI()
        {
            visibleUI = "";
            machineInterface?.SetState(null);
            machine = null;
        }

        public override void OnWorldUnload()
        {
            machineInterface?.SetState(null);
        }
        public static void machineSynchronizer(FItemSlot target)
        {
            int? index = null;
            MachineSlotType? slotType = null;
            for (int i=0;i< machine.inputSlots.Length;i++)
            {
                if(inputCopy[i].stack != machine.inputSlots[i].stack
                    || inputCopy[i].type != machine.inputSlots[i].type)
                {
                    index = i;
                    slotType = MachineSlotType.InputSlot;
                    break;
                }
            }
            if(index== null) {
                for(int i=0; i< machine.outputSlots.Length;i++)
                {
                    if(outputCopy[i].stack != machine.outputSlots[i].stack
                        ||outputCopy[i].type != machine.outputSlots[i].type)
                    {
                        index = i;
                        slotType = MachineSlotType.OutputSlot;
                        break;
                    }

                }
            }
            if(index != null ) {
                switch ((MachineSlotType)slotType) {
                    case MachineSlotType.InputSlot:
                        MessageHandler.ClientModifyTESlotSend(machine.ID,
                            (int)MachineSlotType.InputSlot,
                            (int)index,machine.inputSlots[(int)index]);
                        break;
                    case MachineSlotType.OutputSlot:
                        MessageHandler.ClientModifyTESlotSend(machine.ID,
                            (int)MachineSlotType.OutputSlot,
                            (int)index,machine.outputSlots[(int)index]);
                        break;
                }
            }
            UICaller.inputCopy = Lib.cloneItemArray(UICaller.machine.inputSlots);
            UICaller.outputCopy = Lib.cloneItemArray(UICaller.machine.outputSlots);
        }
    }
}
