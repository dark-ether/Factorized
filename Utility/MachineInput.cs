using Terraria;
using System.Collections.Generic;
using System;
namespace Factorized.Utility
{ 
    public class MachineInput
    {
        public Dictionary<string,List<(int,int)>> inputItems;//first is type second is stack
        public Func<MachineState,bool> machineStateChecker;

        public MachineInput()
        {
            inputItems = new();
            machineStateChecker = machineState => true;
        }

        public bool isCompatible(Item[] inputtedItems, MachineState machineState)
        {
            if(!machineStateChecker(machineState))
            {
                return false;
            }
            return hasEnoughItems(inputtedItems,machineState);
        }
        
        public bool hasEnoughItems(Item[] inputtedItems,MachineState machineState)
        {
            foreach(var categoryListPair in inputItems)
            {
                List<Item> filteredItems = filterItemsByCategory(inputtedItems, machineState, categoryListPair.Key);
                if(!hasEnoughItemsInCategory(filteredItems,categoryListPair.Key)){
                    return false;
                }
            }
            return true;
        }

        public List<Item> filterItemsByCategory(Item[] inputtedItems, MachineState machineState, string category)
        {
            List<Item> filteredItems = new ();
            for(int i = 0; i < inputtedItems.Length; i++)
            {
                if(machineState.inputSlotsMetadata[i] == category)
                {
                    filteredItems.Add(inputtedItems[i]);
                }
            }
            return filteredItems;
        }

        public bool hasEnoughItemsInCategory(List<Item> filteredItems, string category)
        {
           foreach (var Item in inputItems[category])
            {
                bool foundItemInEnoughAmount = false;
                foreach (var itemInStorage in filteredItems)
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
