using Terraria.ModLoader;
using Terraria;
using Factorized.Utility;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Factorized.UI;

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
                    }
                }
            }
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
    } 
}
