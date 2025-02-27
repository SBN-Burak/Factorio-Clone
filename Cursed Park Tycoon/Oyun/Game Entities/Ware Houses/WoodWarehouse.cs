using Microsoft.Xna.Framework;
using Sandbox.Engine;

namespace Sandbox.Oyun.Game_Entities.Ware_Houses
{
    public class WoodWarehouse : Entity
    {
        public EntityInterface entityInterface;

        public WoodWarehouse(string source, Vector2 position, Vector2 scale, Color color) : base(source, position, scale, color)
        {
            Tag = "Wood Warehouse";
            Details = "Wood Warehouse";

            //== Entity Interface Setup ==//
            hasUIInterface = true;
            entityInterface = new("Wood Warehouse", "Textures/wareHouse");
        }

        public override void ShowUIInterface()
        {
            InventoryManager.inventoryGUI.ShowInventoryGUI();

            InventoryManager.inventoryGUI.rightPanel.RemoveChildren();

            this.entityInterface.craftingPanel.IsHidden = true;
            this.entityInterface.fuelPanel.IsHidden = true;
            InventoryManager.inventoryGUI.rightPanel.AddChild(this.entityInterface.mainPanel);

            base.ShowUIInterface();
        }
    }
}
