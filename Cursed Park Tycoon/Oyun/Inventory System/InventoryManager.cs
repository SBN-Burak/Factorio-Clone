using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sandbox.Engine;
using Sandbox.Oyun.GUI;
using Sandbox.Oyun.Inventory_System;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sandbox.Oyun
{
    public static class InventoryManager
    {
        public static List<ItemSlot> slots; // Item seçme ve koyma işini de buradan.

        public static InventoryGUI inventoryGUI; // Crafting sistemlerini buradan yapıcam.

        private static readonly List<string> items = new()
        {
            "stone",
            "coal",
            "iron",
            "conveyorBelt",
            "furnace",
            "burnerDriller"
        };

        // Inventory itemSlot's
        public static void Init()
        {
            if (slots == null)
            {
                slots = new()
                {
                    Capacity = 63
                };

                for (int i = 1; i <= 9; i++) 
                {
                    for (int j = 1; j <= 7; j++)
                    {
                        ItemSlot itemSlot = new(j-1, i-1);

                        itemSlot.Item = "";
                        itemSlot.Count = (0).ToString();
                        itemSlot.DefaultTexture();

                        slots.Add(itemSlot);
                    }
                }
            }

            if(inventoryGUI == null)
            {
                inventoryGUI = new();

                foreach (var slot in slots)
                {
                    inventoryGUI.group.AddChild(slot.slotButton);
                }
            }

            //== savedSlots -> json dosyadan çektiğim slotlar ==//
            List<ItemSlot> savedSlots = SaveManager.LoadInventory<ItemSlot>("Saves/inventory.json");
            foreach (var slot in savedSlots)
            {
                if (slot.Item != "")
                {
                    foreach (var item in items)
                    {
                        if(slot.Item == item)
                        {
                            AddItem(slot.Item, Convert.ToInt16(slot.Count));
                            break;
                        }
                    }
                }
            }
            //=================================================//
        }

        public static void Update()
        {
            inventoryGUI.CheckCraftingElementsHovered();
            inventoryGUI.CheckCraftingElementsPressed();

            KeyboardInput();
        }

        private static void KeyboardInput()
        {
            if (InputManager.KeyPressed(Keys.E))
            {
                inventoryGUI.FlipIsHiddenBool();
                inventoryGUI.ResetRightPanelState();
            }
        }

        public static void AddItem(string itemName, int addAmount)
        {
            foreach (var slot in slots)
            {
                if (slot.item.count >= slot.maxCount)
                    continue;

                if (!slot.isItemPlaced || slot.item.itemName == itemName)
                {
                    slot.isItemPlaced = true;
                    int spaceAvailable = slot.maxCount - slot.item.count;
                    if (addAmount <= spaceAvailable)
                    {
                        slot.item.count += addAmount;
                        slot.item.itemName = itemName;

                        slot.Item = itemName;
                        slot.Count = (slot.item.count).ToString();

                        slot.itemAmountText.Text = slot.item.count.ToString();
                        slot.SetToolTipText(itemName);
                        slot.SetTexture(itemName);
                        return;
                    }
                    else
                    {
                        slot.item.count = slot.maxCount;
                        slot.item.itemName = itemName;
                        slot.itemAmountText.Text = slot.item.count.ToString();
                        slot.SetToolTipText(itemName);
                        slot.SetTexture(itemName);
                        addAmount -= spaceAvailable;
                    }
                }
            }
        }

        //== Crafting Part ==//
        public static bool HasRequiredItems(CraftingRecipe recipe)
        {
            int totalAmount = 0;
            foreach (var slot in slots)
            {
                if (slot.isItemPlaced && slot.item.itemName == recipe.ItemName)
                {
                    totalAmount += slot.item.count;
                }
            }
            return totalAmount >= recipe.RequiredAmount;
        }

        // FOR PLACEMENT MANAGER. WE CHECK IF WE HAVE ENOUGH FROM TOOLTIPINTERFACE
        public static bool HasRequiredItemsInToolTipInterface()
        {
            return Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount") > 0;
        }

        public static void RemoveItems(CraftingRecipe recipe)
        {
            int amountToRemove = recipe.RequiredAmount;
            foreach (var slot in slots)
            {
                if (slot.isItemPlaced && slot.item.itemName == recipe.ItemName)
                {
                    if (slot.item.count >= amountToRemove)
                    {
                        slot.item.count -= amountToRemove;
                        if (slot.item.count == 0)
                        {
                            slot.isItemPlaced = false;
                            slot.item.itemName = "";
                            slot.itemAmountText.Text = "";
                            slot.DefaultTexture();
                            slot.DefaultTooltip();
                        }
                        else
                        {
                            slot.itemAmountText.Text = slot.item.count.ToString();
                        }
                        break;
                    }
                    else
                    {
                        amountToRemove -= slot.item.count;
                        slot.item.count = 0;
                        slot.isItemPlaced = false;
                        slot.item.itemName = null;
                        slot.itemAmountText.Text = "";
                    }
                }
            }
        }

        public static void CraftItem(CraftingRecipe recipe, string craftedItemName)
        {
            if (HasRequiredItems(recipe))
            {
                RemoveItems(recipe);
                AddItem(craftedItemName, 1);
            }
            else
            {
                Debug.WriteLine("Not enough materials to craft the item.");
            }
        }

        public static void CraftFurnace()
        {
            CraftingRecipe furnaceRecipe = new("stone", 32);
            CraftItem(furnaceRecipe, "furnace");
        }

        public static void CraftBurnerDriller()
        {
            CraftingRecipe furnaceRecipe = new("stone", 32);
            CraftItem(furnaceRecipe, "burnerDriller");
        }

        public static void CraftConveyorBelt()
        {
            CraftingRecipe furnaceRecipe = new("iron", 16);
            CraftItem(furnaceRecipe, "conveyorBelt");
        }

    }
}
