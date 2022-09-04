using Terraria.ModLoader;
using Terraria;
using Factorized.Utility;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Factorized.UI;
using System.IO;


namespace Factorized.Machines
{
    public abstract class FueledMTE : MachineTE
    {
        public int fuel {get; protected set;} = 0;
        public abstract Dictionary<int,int> fuelTypes {get;}

        protected override void OnPreUpdate()
        {
            if(fuel <= 0)
            {
                tryRefreshFuel();
            }else
            {
                fuel--;
            }
        }
        protected void tryRefreshFuel()
        {
            foreach(var fuelType in fuelTypes)
            {
                foreach(var slot in GetSlots(MachineSlotType.Fuel))
                {
                    if(slot.IType == fuelType.Key)
                    {
                        fuel += fuelType.Value;
                        slot.SlotItem.stack -= 1;
                        if(slot.stack <= 0) slot.SlotItem = new Item();
                        goto OUTSIDE;
                    }
                }
            }
        OUTSIDE:;
        }
        public override void GenerateUI(MachineUI UI)
        {
            base.GenerateUI(UI);
            UIPanel fuelPanel = new ();
            fuelPanel.Width.Set(100,0f);
            fuelPanel.Height.Set(100,0f);
            fuelPanel.HAlign = 0.125f;
            fuelPanel.VAlign = 0.65f;
            UIText t = new ("TODO: implement showing fuel for fueled machines");
            t.Height.Set(30,0f);
            t.Width.Set(100,0f);
            fuelPanel.Append(t);
            UI.Append(fuelPanel);
        }
        public override void NetSend(BinaryWriter writer)
        {
            base.NetSend(writer);
            writer.Write(fuel);
        }
        public override void NetReceive(BinaryReader reader)
        {
            base.NetReceive(reader);
            fuel = reader.ReadInt32();
        }
        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag["fuel"] = fuel;
        }
        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            if(tag.ContainsKey("fuel")) fuel = tag.GetInt("fuel");
            else fuel = 0;
        }
        protected override bool CanProgress()
        {
            return base.CanProgress() && fuel > 0;
        }
    } 
}
