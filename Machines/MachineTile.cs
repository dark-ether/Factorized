using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ObjectData;
using Terraria.Enums;
using Microsoft.Xna.Framework;
using Factorized.UI;
using Factorized.Utility;
using System.Linq;

namespace Factorized.Machines{
    public abstract class MachineTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
			      Main.tileBlockLight[Type] = true;
			      Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
	          //ItemDrop = ModContent.ItemType<content.items.placeables.melter>();
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.Origin = new Point16(0,0);
            modifyObjectData();
            MachineTE myMachine = getTileEntity();//get tile entity makes the tile call the correct tile entity
            if(myMachine is not null){
                TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(myMachine.Hook_AfterPlacement
                ,-1,0,false);
            }
            TileObjectData.addTile(Type);
        }
        /* 
        public override bool CanKillTile(int i,int j,ref bool blockDamaged)
        {
            return !(getTileEntityInLocation(i,j) ==null) && !getTileEntityInLocation(i,j).hasItems() 
                && !getTileEntityInLocation(i,j).machineState.IsProcessing();
        }
        */
        public virtual MachineTE getTileEntityInLocation(int i ,int j)
        {
            if(!TileEntity.ByPosition.ContainsKey(TileUtils.GetTileOrigin(i,j))) return null;
            MachineTE tileEntity = (MachineTE) TileEntity.ByPosition[TileUtils.GetTileOrigin(i,j)];
            return tileEntity;
        }

        public virtual void modifyObjectData()
        {
        }

        public override bool RightClick(int x, int y)
        {
            Vector2 playerPosition = Main.LocalPlayer.Center;
            Vector2 tilePosition = new(x * 16, y * 16);//remember that tile coordinates are 1/16 world coordinates
            Player player = Main.LocalPlayer;

            //Should your tile entity bring up a UI, this line is useful to prevent item slots from misbehaving
            Main.mouseRightRelease = false;

            //The following four (4) if-blocks are recommended to be used if your multitile opens a UI when right clicked:
            if (player.sign > -1)
            {
                //TODO: Fix this 
                //SoundEngine.PlaySound(11, -1, -1, 1);
                player.sign = -1;
                Main.editSign = false;
                Main.npcChatText = string.Empty;
            }
            
            if (Main.editChest)
            {
                //TODO: Fix this
                //SoundEngine.PlaySound(12, -1, -1, 1);
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }
            
            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
                player.editedChestName = false;
            }
            
            if (player.talkNPC > -1)
            {
                Main.npcChatCornerItem = 0;
                Main.npcChatText = string.Empty;
            }
            
            if(Vector2.Distance(playerPosition,tilePosition)< 5 *16){
                Factorized.Instance.UI.showMachine(TileUtils.GetTileOrigin(x,y));
                return true;
            }
            return false;
        }

        public override void KillMultiTile(int i, int j ,int FrameX , int FrameY)
        {
            Point16 tileOrigin = TileUtils.GetTileOrigin(i,j);
            Item.NewItem(new EntitySource_TileBreak(i, j),i * 16 , j * 16,48,32,getItemType());
            MachineTE dead = getTileEntityInLocation(i,j);
            foreach(var item in dead.GetItems())
            {
                if(!item.IsAir)
                {
                    //TODO: switch to the new newItem overload which takes an item
                    Item.NewItem(new EntitySource_TileBreak(i,j),i*16,j*16,48,32,item.type,item.stack);
                }
            }

        }
        public abstract MachineTE getTileEntity();
        public abstract int getItemType();
    }
}
