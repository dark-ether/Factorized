using Terraria;
using System.Collections.Generic;
using System;
namespace Factorized.Utility
{ 
    public class MachineInput
    {
        public List<(int,int)> inputItems;//first is type second is stack
        public Func<MachineState,bool> machineStateChecker;

        public MachineInput()
        {
            inputItems = new();
            machineStateChecker = machineState => true;
        }

        public bool isCompatible(Item[] inputItems, MachineState machineState)
        {
            if(machineStateChecker(machineState))
            {
                return hasEnoughItems(inputItems);
            }
            else 
            {
                return false;
            }
        }
        
        public bool hasEnoughItems(Item[] itemsToCheck)
        {
            foreach (var Item in inputItems)
            {
                bool foundItemInEnoughAmount = false;
                foreach (var itemInStorage in itemsToCheck)
                {
                    if(itemInStorage.type == Item.Item1&& itemInStorage.stack >= Item.Item2)
                    {
                        foundItemInEnoughAmount =  true;
                    } 
                }
                if(!foundItemInEnoughAmount)
                {
                    return false;
                }
            }
            return true;
        }
    }

} 
