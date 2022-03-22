
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Factorized.Items.Placeables
{
	  public class MelterItem : ModItem
	  {
		    public override void SetStaticDefaults()
		    {
			      DisplayName.SetDefault("melter"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			      Tooltip.SetDefault("This is a basic modded sword.");
		    }

		    public override void SetDefaults()
		    {
            Item.maxStack = 9999;
			      Item.width = 40;
			      Item.height = 40;
			      Item.value = 10000;
			      Item.rare = 6;
			      Item.autoReuse = true;
            Item.createTile = ModContent.TileType<Tiles.Machines.MelterTile>();
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
		    }

		    public override void AddRecipes()
		    {
			      Recipe recipe = CreateRecipe();
			      recipe.AddIngredient(ItemID.DirtBlock, 1);
			      recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

		    }
	  }
}
