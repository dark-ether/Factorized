using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace Factorized.Content.EngineeringTable
{
  public class EngineeringTableItem : ModItem
  {
    public override string Texture => "Factorized/Content/EngineeringTable/EngineeringTable";
    public override void SetDefaults()
    {
      Item.createTile = ModContent.TileType<Content.EngineeringTable.EngineeringTableTile>();
      Item.maxStack = 9999;
      Item.value = 200;
      Item.useAnimation = 15;
      Item.useTime = 15;
      Item.consumable = true;
      Item.useStyle = ItemUseStyleID.Swing;
      Item.rare = ItemRarityID.White;
      Item.autoReuse = true;
    }
    public override void SetStaticDefaults()
    {
    }
    public override void AddRecipes()
    {
      Recipe start = CreateRecipe();
      start.AddRecipeGroup("Wood",20);
      start.Register();
    }
  }
}
