
namespace Sandbox.Oyun.Inventory_System
{
    public class CraftingRecipe
    {
        public string ItemName { get; set; }
        public int RequiredAmount { get; set; }

        public CraftingRecipe(string itemName, int requiredAmount)
        {
            ItemName = itemName;
            RequiredAmount = requiredAmount;
        }
    }
}
