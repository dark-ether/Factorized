using Terraria;

namespace Factorized.Utility{
    public static class itemUtils{
        public static bool CanInsert(Item slot,Item toPickup)
        {
            return slot.IsAir || (slot.type == toPickup.type 
                && slot.maxStack >= slot.stack + toPickup.stack);
        }
        public static void Insert(ref Item slot,ref Item toPickup)
        {
            if(slot.IsAir)
            {
                slot = toPickup;
            }
            else
            {
                slot.stack += toPickup.stack;
            }
            toPickup = new ();
        }
        public static void Swap(ref Item i1, ref Item i2)
        {
            Item store = i1;
            i1 = i2;
            i2 = store;
        }
    }
}
