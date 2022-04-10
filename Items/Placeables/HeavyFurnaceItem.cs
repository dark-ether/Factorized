
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Factorized.Items.Placeables
{
	  public class HeavyFurnaceItem : ModItem
	  {
		    public override void SetStaticDefaults()
		    {
			      DisplayName.SetDefault("Heavy Furnace"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			      Tooltip.SetDefault("A Furnace that consumes Fuel to generate Bars at a more efficient rate,\n can only Smelt Hellstone with Hellstone bars as Fuel");
		    }

		    public override void SetDefaults()
		    {
                Item.maxStack = 999;
			    Item.width = 40;
			    Item.height = 40;
			    Item.value = 10000;
			    Item.rare = 6;
			    Item.autoReuse = true;
                Item.createTile = ModContent.TileType<Tiles.Machines.HeavyFurnaceTile>();
                Item.useAnimation = 15;
                Item.useTime = 15;
                Item.maxStack = 999;
                Item.consumable = true;
                Item.useStyle = ItemUseStyleID.Swing;
		    }

		    public override void AddRecipes()
		    {
			    Recipe recipe = CreateRecipe();
			    recipe.AddIngredient(ItemID.DemoniteBar,100);
                recipe.AddIngredient(ItemID.StoneBlock,1000);
                recipe.Register();
                Recipe recipe2 = CreateRecipe();
                recipe2.AddIngredient(ItemID.CrimtaneBar,100);
                recipe2.AddIngredient(ItemID.StoneBlock,1000);
                recipe2.Register();
		    }
	  }
}
