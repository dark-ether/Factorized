using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using cookiefactorized.ui;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using cookiefactorized.content.TE.machineTE;

namespace cookiefactorized.tiles.machines
{
	public class melterTile : ModTile
	{
        public override string Name => base.Name;

        public override string Texture => base.Texture;

        public override string HighlightTexture => base.HighlightTexture;

        public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
			//ItemDrop = ModContent.ItemType<content.items.placeables.melter>();
            
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            
            TileObjectData.addTile(Type);
			AddMapEntry(new Color(200, 200, 200));
		}
        
        public override void KillMultiTile(int i, int j ,int FrameX , int FrameY)
        {
            Item.NewItem(i * 16 , j * 16,48,32,ModContent.ItemType<content.items.placeables.melter>());
        }

        public override bool RightClick(int x ,int y){
            //Point16 pos = new(x,y);
            //TileEntity = TileEntity.ByPosition[pos];
            //code for acessing tile entity
            
            UICaller.showMelterUI(x, y);
            Main.playerInventory = true;
            TileEntity thisMelter = TileEntity.ByPosition[new Point16(x,y)];
            TagCompound info = new TagCompound();
            info.Add("type","sendRightClick");
            thisMelter.SaveData(info);
            return true;
        
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void Load()
        {
            base.Load();
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return base.IsLoadingEnabled(mod);
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override ushort GetMapOption(int i, int j)
        {
            return base.GetMapOption(i, j);
        }

        public override bool KillSound(int i, int j)
        {
            return base.KillSound(i, j);
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            base.NumDust(i, j, fail, ref num);
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return base.CreateDust(i, j, ref type);
        }

        public override bool CanPlace(int i, int j)
        {
            return base.CanPlace(i, j);
        }

        public override bool CanExplode(int i, int j)
        {
            return base.CanExplode(i, j);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return base.PreDraw(i, j, spriteBatch);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, spriteBatch);
        }

        public override void RandomUpdate(int i, int j)
        {
            base.RandomUpdate(i, j);
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            base.PlaceInWorld(i, j, item);
            melterTE thisMelter = new melterTE(i,j);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            base.ModifyLight(i, j, ref r, ref g, ref b);
        }

        public override void PostSetDefaults()
        {
            base.PostSetDefaults();
        }

        public override bool HasSmartInteract()
        {
            return base.HasSmartInteract();
        }

        public override void DropCritterChance(int i, int j, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance)
        {
            base.DropCritterChance(i, j, ref wormChance, ref grassHopperChance, ref jungleGrubChance);
        }

        public override bool Drop(int i, int j)
        {
            return base.Drop(i, j);
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return base.CanKillTile(i, j, ref blockDamaged);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            base.NearbyEffects(i, j, closer);
        }

        public override float GetTorchLuck(Player player)
        {
            return base.GetTorchLuck(player);
        }

        public override bool Dangersense(int i, int j, Player player)
        {
            return base.Dangersense(i, j, player);
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            base.SetSpriteEffects(i, j, ref spriteEffects);
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            base.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tileFrameX, ref tileFrameY);
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            base.AnimateTile(ref frame, ref frameCounter);
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            base.AnimateIndividualTile(type, i, j, ref frameXOffset, ref frameYOffset);
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            base.DrawEffects(i, j, spriteBatch, ref drawData);
        }

        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.SpecialDraw(i, j, spriteBatch);
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return base.TileFrame(i, j, ref resetFrame, ref noBreak);
        }

        public override void MouseOver(int i, int j)
        {
            base.MouseOver(i, j);
        }

        public override void MouseOverFar(int i, int j)
        {
            base.MouseOverFar(i, j);
        }

        public override bool AutoSelect(int i, int j, Item item)
        {
            return base.AutoSelect(i, j, item);
        }

        public override void HitWire(int i, int j)
        {
            base.HitWire(i, j);
        }

        public override bool Slope(int i, int j)
        {
            return base.Slope(i, j);
        }

        public override void FloorVisuals(Player player)
        {
            base.FloorVisuals(player);
        }

        public override bool HasWalkDust()
        {
            return base.HasWalkDust();
        }

        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            base.WalkDust(ref dustType, ref makeDust, ref color);
        }

        public override void ChangeWaterfallStyle(ref int style)
        {
            base.ChangeWaterfallStyle(ref style);
        }

        public override int SaplingGrowthType(ref int style)
        {
            return base.SaplingGrowthType(ref style);
        }

        public override bool IsLockedChest(int i, int j)
        {
            return base.IsLockedChest(i, j);
        }

        public override bool UnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual)
        {
            return base.UnlockChest(i, j, ref frameXAdjustment, ref dustType, ref manual);
        }
    }
}
