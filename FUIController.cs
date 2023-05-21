using System.Collections.Generic;
using Factorized.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace Factorized
{
  public class FUIController : ModSystem
  {
    private UserInterface machineInterface;
    private GameTime _lastUpdateUiGameTime;
    public Dictionary<string,Asset<Texture2D>> asset;
    public override void Load()
    {
      ((Factorized)Mod).UI = this;
      if(!Main.dedServ) {
        machineInterface = new UserInterface();//initializes user interface
      }
    }

    public override void Unload()
    {
      machineInterface = null;
    }

    public override void UpdateUI(GameTime gameTime)
    {
      _lastUpdateUiGameTime = gameTime;
      if (machineInterface?.CurrentState != null) {
        machineInterface.Update(gameTime);
      }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
      int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
      if (mouseTextIndex != -1) {
        layers.Insert(mouseTextIndex,new LegacyGameInterfaceLayer(
                        "Factorized: Machine Interface",
        delegate {
          if (_lastUpdateUiGameTime != null && machineInterface?.CurrentState != null) {
            machineInterface.Draw(Main.spriteBatch,_lastUpdateUiGameTime);
          }
          return true;
        },
        InterfaceScaleType.UI
                      ));
      }
    }
    public void showMachine(Point16 Pos) {
      machineInterface.SetState(new MachineUI(Pos));
    }
    public void removeUI() {
      machineInterface.SetState(null);
    }
    public override void OnWorldUnload(){
      removeUI();
    }
  }
}


