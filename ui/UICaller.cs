using Terraria.ModLoader;
using Terraria;
using Terraria.UI;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace cookiefactorized.ui 
{
    public class UICaller : ModSystem
    {
        internal static UserInterface machineInterface; //user interface
        internal static machineUI melterUI; // melter ui
        private static GameTime _lastUpdateUiGameTime;
        public static int melterX;
        public static int melterY;
        private static string visibleUI = "";
        public override string Name => base.Name;
        
        public override void Load()
        {
            if(!Main.dedServ){
                machineInterface = new UserInterface();//initializes user interface

                melterUI = new machineUI();
                melterUI.Activate();
            }
        }
        
        public override void Unload()
        {
            melterUI = null;// unloades the ui
        }
        
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (machineInterface?.CurrentState != null){// checking for nullity
                machineInterface.Update(gameTime);
                if(visibleUI == "melterUI"){
                    if(!Main.playerInventory){
                        hideUI();
                    }
                    Vector2 melterPosition = new Vector2(melterX*16,melterY*16);//changing to world coordinates
                    float distance = Vector2.Distance(melterPosition,Main.LocalPlayer.Center);
                    if(distance > 5*16){//5 tiles
                        hideUI();
                    }
                }

            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if(mouseTextIndex != -1){
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "cookiefactorized: machineInterface",
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

        public static void showMelterUI(int x , int y)
        {
            hideUI();
            visibleUI = "melterUI";
            melterX = x;
            melterY = y;
            machineInterface?.SetState(melterUI);    
        }

        public static void hideUI()
        {
            visibleUI = "";
            machineInterface?.SetState(null);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return base.IsLoadingEnabled(mod);
        }

        protected override void Register()
        {
            base.Register();
        }

        public override void OnModLoad()
        {
            base.OnModLoad();
        }

        public override void OnWorldLoad()
        {
            base.OnWorldLoad();
        }
        
        public override void OnWorldUnload()
        {
            base.OnWorldUnload();
            machineInterface.SetState(null);
        }

        public override void ModifyScreenPosition()
        {
            base.ModifyScreenPosition();
        }

        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            base.ModifyTransformMatrix(ref Transform);
        }

        public override void PreUpdateTime()
        {
            base.PreUpdateTime();
        }

        public override void PostUpdateTime()
        {
            base.PostUpdateTime();
        }

        public override void PreUpdateWorld()
        {
            base.PreUpdateWorld();
        }

        public override void PostUpdateWorld()
        {
            base.PostUpdateWorld();
        }
       
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            base.PostDrawInterface(spriteBatch);
        }

        public override void PostDrawFullscreenMap(ref string mouseText)
        {
            base.PostDrawFullscreenMap(ref mouseText);
        }

        public override void PostUpdateInput()
        {
            base.PostUpdateInput();
        }

        public override void PostDrawTiles()
        {
            base.PostDrawTiles();
        }

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            base.TileCountsAvailable(tileCounts);
        }
    }
}
