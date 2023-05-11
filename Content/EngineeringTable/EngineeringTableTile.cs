using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Factorized.Content.EngineeringTable
{
  public class EngineeringTableTile : ModTile
  {
    public override string Texture => "Factorized/Content/EngineeringTable/EngineeringTable";
    public override void SetStaticDefaults()
    {
      Main.tileSolid[Type] = false;
      Main.tileBlockLight[Type] = false;
      Main.tileFrameImportant[Type] = true;

      TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
      TileObjectData.newTile.Height = 4;
      TileObjectData.newTile.Width = 4;
      TileObjectData.newTile.UsesCustomCanPlace = true;
      TileObjectData.newTile.CoordinateHeights = new int[] {16,16,16,16};
      TileObjectData.newTile.LavaDeath = false;
      TileObjectData.newTile.CoordinatePadding = 0;
      TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
      TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
      TileObjectData.newTile.Origin = new Point16(2,2);
      TileObjectData.newTile.Direction = TileObjectDirection.None;
      TileObjectData.newTile.Style = 0;
      TileObjectData.addTile(Type);
    }
    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
      Item.NewItem(new EntitySource_TileBreak(i,j),i*16,j*16,64,64,ModContent.ItemType<EngineeringTableItem>(),1);
    }
  }
}
