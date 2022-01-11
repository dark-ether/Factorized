using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using cookiefactorized.tiles.machines;
using cookiefactorized;
namespace cookiefactorized.TE.machineTE
{
    class melterTE : ModTileEntity
    {
        public int timesClicked;
        
        public melterTE(){
            timesClicked = 0;
            ModContent.GetInstance<cookiefactorized>().Logger.Debug("created melterTE");
        }
        public melterTE(int x, int y){
            timesClicked = 0;
            Place(x,y);
            ModContent.GetInstance<cookiefactorized>().Logger.Debug($"position {x},{y}");
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string GetItemGamepadInstructions(int slot = 0)
        {
            return base.GetItemGamepadInstructions(slot);
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            return base.Hook_AfterPlacement(i, j, type, style, direction, alternate);
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return base.IsLoadingEnabled(mod);
        }
        
        public override bool IsTileValidForEntity(int x, int y)
        {//abstract class has to be overrided
            /* get tile
                Main[x,y];
                
                tiletype to mod/tilename
                ModContent.getModTile(tile.type) is ModTile modTile;

                mod/tilename to tile.type
                mod.Find<ModTile>(name).Type;
            */ 
            Tile myTile = Main.tile[x,y];
            if((ModContent.GetModTile(myTile.type)) is melterTile){
                return true;
            }
            return false;
        }

        public override void Load(Mod mod)
        {
            base.Load(mod);
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            timesClicked = tag.GetInt("timesClicked");
        }

        public override void NetPlaceEntityAttempt(int i, int j)
        {
            base.NetPlaceEntityAttempt(i, j);
        }

        public override void NetReceive(BinaryReader reader)
        {
            base.NetReceive(reader);
        }

        public override void NetSend(BinaryWriter writer)
        {
            base.NetSend(writer);
        }

        public override void OnInventoryDraw(Player player, SpriteBatch spriteBatch)
        {
            base.OnInventoryDraw(player, spriteBatch);
        }

        public override void OnKill()
        {
            base.OnKill();
        }

        public override void OnNetPlace()
        {
            base.OnNetPlace();
        }

        public override void OnPlayerUpdate(Player player)
        {
            base.OnPlayerUpdate(player);
        }

        public override bool OverrideItemSlotHover(Item[] inv, int context = 0, int slot = 0)
        {
            return base.OverrideItemSlotHover(inv, context, slot);
        }

        public override bool OverrideItemSlotLeftClick(Item[] inv, int context = 0, int slot = 0)
        {
            return base.OverrideItemSlotLeftClick(inv, context, slot);
        }

        public override void PostGlobalUpdate()
        {
            base.PostGlobalUpdate();
        }

        public override void PreGlobalUpdate()
        {
            base.PreGlobalUpdate();
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag.Add("timesClicked",timesClicked);
            Mod.Logger.Debug($" increased counter of times clicked");
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool TryGetItemGamepadOverrideInstructions(Item[] inv, int context, int slot, out string instruction)
        {
            return base.TryGetItemGamepadOverrideInstructions(inv, context, slot, out instruction);
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
