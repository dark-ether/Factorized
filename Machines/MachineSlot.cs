using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Factorized;
using Factorized.Utility;

namespace Factorized.Machines
{ 
    public class MachineSlot
    {
        public Item SlotItem;
        public MachineSlotType Type;
        public MachineSlot(MachineSlotType type)
        {
            Type = type;
            SlotItem = new Item();
        }
        public int IType {get => SlotItem.type;}
        public int stack {get => SlotItem.stack;}
        public int maxStack {get => SlotItem.maxStack;}
        public Factorized.ItemReferrer GetItem()
        {
            return  () => ref SlotItem;
        }
    }
    //TODO:test if enums are serialized without serializers;
    public class MachineSlotSerializer : TagSerializer<MachineSlot, TagCompound>
    {
        public override MachineSlot Deserialize(TagCompound tag)
        {
            MachineSlot saved = new ((MachineSlotType)tag.GetAsInt("Type"));
            saved.SlotItem = tag.Get<Item>("SlotItem");
            return saved;
        }

        public override TagCompound Serialize(MachineSlot value)
        {
            return new TagCompound{["SlotItem"]= value.SlotItem, ["Type"]=(int)value.Type};
        }
    }
}
