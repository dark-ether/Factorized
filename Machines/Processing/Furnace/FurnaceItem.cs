using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace Factorized.Machines.Processing.Furnace
{
  public class FurnaceItem : ModItem
  {
    public override string Texture => "Factorized/Machines/Processing/Furnace/Furnace";

    public override void SetStaticDefaults()
    {
    }
    public override void SetDefaults()
    {
      Item.value = 100;
      Item.rare = ItemRarityID.White;
      Item.autoReuse = true;
      Item.maxStack = 9999;
      Item.createTile = ModContent.TileType<FurnaceTile>();
      Item.consumable = true;
      Item.useStyle = ItemUseStyleID.Swing;
      Item.useTime = 15;
      Item.useAnimation = 15;
      Item.rare =  2;

    }
    public override void AddRecipes()
    {
      CreateRecipe()
      .AddIngredient(ItemID.Wood, 100)
      .AddIngredient(ItemID.Torch,50)
      .AddIngredient(ItemID.StoneBlock, 100)
      .AddTile<Content.EngineeringTable.EngineeringTableTile>();
    }
  }
}
