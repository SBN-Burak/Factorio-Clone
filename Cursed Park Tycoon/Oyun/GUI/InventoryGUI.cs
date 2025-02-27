using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using Sandbox.Engine;
using System;
using System.Collections.Generic;

namespace Sandbox.Oyun.GUI
{
    public class InventoryGUI
    {
        Panel mainPanel;

        //==== Inventory Section ====//
        public Group group;
        Panel inventoryPanel;
        //===========================//

        //==== Crafting Section ====//
        public Panel craftingPanel;
        Panel craftingButtonPanel;
        Button logisticsButton;
        Button productionButton;
        Button miscButton;
        Button wheaponButton;

        Panel craftingElementsPanel;
        public List<Button> craftingElements = new();

        public bool isLogisticsActive;
        public bool isProductionActive;
        public bool isMiscActive;
        public bool isWheaponActive;
        //===========================//

        //==== Oher Section for entity interface GUI ====//
        public Panel rightPanel = new(Anchor.CenterRight, new Vector2(420, 590), Vector2.Zero, setHeightBasedOnChildren: false);
        //==============================//

        bool workOnce = false;

        public InventoryGUI()
        {
            mainPanel = new Panel(Anchor.Center, new Vector2(850, 600), Vector2.Zero, setHeightBasedOnChildren: false)
            {
                IsHidden = true,
                DrawColor = Color.DarkGray,
            };

            //===========================//
            inventoryPanel = new Panel(Anchor.CenterLeft, new Vector2(420, 590), Vector2.Zero, setHeightBasedOnChildren: false)
            {
                DrawColor = Color.DarkGray,
            };

            inventoryPanel.AddChild(new Paragraph(Anchor.TopLeft, 1, "Inventory", true)
            {
                TextScale = .85f,
                PositionOffset = new Vector2(5, 3),
            });
            group = new Group(Anchor.TopLeft, new Vector2(450, 540), false, true);

            inventoryPanel.AddChild(group);

            //===========================//

            craftingPanel = new Panel(Anchor.CenterRight, new Vector2(420, 590), Vector2.Zero, setHeightBasedOnChildren: false)
            {
                DrawColor = Color.DarkGray
            };

            craftingPanel.AddChild(new Paragraph(Anchor.TopLeft, 1, "Crafting", true)
            {
                TextScale = .85f,
                PositionOffset = new Vector2(8, 3),
            });

            craftingButtonPanel = new Panel(Anchor.TopCenter, new Vector2(390, 82))
            {
                PositionOffset = new Vector2(0, 40),
                DrawColor = new Color(140, 140, 140)
            };
            
            logisticsButton = new Button(Anchor.AutoInlineCenterIgnoreOverflow, new Vector2(75, 75), "", "Logistics")
            {
                //OnPressed = element => this.UiSystem.Remove("InfoBox"),
                OnPressed = element =>
                {
                    isLogisticsActive = true;
                    isProductionActive = false;
                    isMiscActive = false;
                    isWheaponActive = false;

                    RenewCraftingElements();
                },
                PositionOffset = new Vector2(0, 0),
                Texture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/İkonlar/logistic_ikon"), padding: 1)
            };
            productionButton = new Button(Anchor.AutoInlineCenterIgnoreOverflow, new Vector2(75, 75), "", "Production")
            {
                //OnPressed = element => this.UiSystem.Remove("InfoBox"),
                OnPressed = element =>
                {
                    isLogisticsActive = false;
                    isProductionActive = true;
                    isMiscActive = false;
                    isWheaponActive = false;

                    RenewCraftingElements();
                },
                PositionOffset = new Vector2(25, 0),
                Texture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/İkonlar/production_ikon"), padding: 1),
            };
            miscButton = new Button(Anchor.AutoInlineCenterIgnoreOverflow, new Vector2(75, 75), "", "Misc")
            {
                //OnPressed = element => this.UiSystem.Remove("InfoBox"),
                OnPressed = element =>
                {
                    isLogisticsActive = false;
                    isProductionActive = false;
                    isMiscActive = true;
                    isWheaponActive = false;
                },
                PositionOffset = new Vector2(25, 0),
                Texture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/İkonlar/misc_ikon"), padding: 1),
            };
            wheaponButton = new Button(Anchor.AutoInlineCenterIgnoreOverflow, new Vector2(75, 75), "", "Wheapon")
            {
                //OnPressed = element => this.UiSystem.Remove("InfoBox"),
                OnPressed = element =>
                {
                    isLogisticsActive = false;
                    isProductionActive = false;
                    isMiscActive = false;
                    isWheaponActive = true;
                },
                PositionOffset = new Vector2(25, 0)
            };
            
            craftingElementsPanel = new Panel(Anchor.Center, new Vector2(390, 455))
            {
                PositionOffset = new Vector2(0, 58),
                DrawColor = new Color(150, 150, 150)
            };

            craftingElements.Capacity = 42;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Button btn = new(Anchor.TopLeft, new Vector2(55, 55), "")
                    {
                        PositionOffset = new Vector2(j * 59 + 15, i * 60 + 15),
                        Texture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/İkonlar/craftingElement"), padding: 1)
                    };

                    craftingElements.Add(btn);
                    craftingElementsPanel.AddChild(btn);
                }
            }

            craftingButtonPanel.AddChild(logisticsButton);
            craftingButtonPanel.AddChild(productionButton);
            craftingButtonPanel.AddChild(miscButton);
            craftingButtonPanel.AddChild(wheaponButton);

            craftingPanel.AddChild(craftingButtonPanel);
            craftingPanel.AddChild(craftingElementsPanel);

            mainPanel.AddChild(inventoryPanel);

            mainPanel.AddChild(rightPanel);

            //== DEFAULT SETTINGS ==//

            isLogisticsActive = true;
            isProductionActive = false;
            isMiscActive = false;
            isWheaponActive = false;


            // Showing crafting panel as default.
            ResetRightPanelState();
            RenewCraftingElements();
        }

        public void CheckCraftingElementsPressed() // Check if any craftingElement is pressed. 
        {
            craftingElements[0].OnPressed = element => {
                if (isLogisticsActive)
                {
                    InventoryManager.CraftConveyorBelt();
                }
                else if (isProductionActive)
                {
                    InventoryManager.CraftFurnace();
                }
                else if (isMiscActive) { }
                else if (isWheaponActive) { }
            };

            //////////

            craftingElements[6].OnPressed = element => {

                if (isLogisticsActive) { }
                else if (isProductionActive)
                {
                    InventoryManager.CraftBurnerDriller();
                }
                else if (isMiscActive) { }
                else if (isWheaponActive) { }
            };
        }

        public void CheckCraftingElementsHovered() // Check if any craftingElement is hovered. 
        {
            craftingElements[0].OnMouseEnter = element => {

                if (!Globals.ToolTipInterface.isItemGrabbed)
                {
                    // Conveyor Belt
                    if (isLogisticsActive)
                    {
                        Globals.ToolTipInterface.mainPanel.Size = new Vector2(190, 100);
                        Globals.ToolTipInterface.mainPanel.IsHidden = false;

                        Globals.ToolTipInterface.mainPanel.AddChild(
                                new Paragraph(Anchor.TopLeft, 1, "Conveyor Belt")
                                {
                                    TextScale = .9f
                                });

                        Globals.ToolTipInterface.mainPanel.AddChild(
                                new Paragraph(Anchor.TopLeft, 1, "You need 16: ")
                                {
                                    TextScale = .5f,
                                    TextColor = Color.Yellow,
                                    PositionOffset = new Vector2(0, 45)
                                });

                        Globals.ToolTipInterface.mainPanel.AddChild(
                                new Image(Anchor.TopLeft, Vector2.One,
                                    new TextureRegion(Globals.Content.Load<Texture2D>("Textures/iron")), true)
                                {
                                    ImageScale = new Vector2(1.3f),
                                    PositionOffset = new Vector2(115, 45)
                                });
                    }

                    // Furnace
                    else if (isProductionActive)
                    {
                        Globals.ToolTipInterface.mainPanel.Size = new Vector2(190, 100);
                        Globals.ToolTipInterface.mainPanel.IsHidden = false;

                        Globals.ToolTipInterface.mainPanel.AddChild(
                                new Paragraph(Anchor.TopLeft, 1, "Furnace")
                                {
                                    TextScale = .9f
                                });

                        Globals.ToolTipInterface.mainPanel.AddChild(
                                new Paragraph(Anchor.TopLeft, 1, "You need 32: ")
                                {
                                    TextScale = .5f,
                                    TextColor = Color.Yellow,
                                    PositionOffset = new Vector2(0, 45)
                                });

                        Globals.ToolTipInterface.mainPanel.AddChild(
                                new Image(Anchor.TopLeft, Vector2.One,
                                    new TextureRegion(Globals.Content.Load<Texture2D>("Textures/stone")), true)
                                {
                                    ImageScale = new Vector2(1.3f),
                                    PositionOffset = new Vector2(115, 45)
                                });
                    }

                    else if (isMiscActive) { }
                    else if (isWheaponActive) { }
                }

            };
            craftingElements[0].OnMouseExit = element => {
                if (!Globals.ToolTipInterface.isItemGrabbed)
                {
                    Globals.ToolTipInterface.mainPanel.IsHidden = true;
                    Globals.ToolTipInterface.mainPanel.RemoveChildren();
                }
            };
            
            //////////
            
            craftingElements[6].OnMouseEnter = element => {

                if (!Globals.ToolTipInterface.isItemGrabbed)
                {

                    if (isLogisticsActive) { }

                    // Burner Driller
                    else if (isProductionActive)
                    {
                        Globals.ToolTipInterface.mainPanel.Size = new Vector2(190, 100);
                        Globals.ToolTipInterface.mainPanel.IsHidden = false;

                        Globals.ToolTipInterface.mainPanel.AddChild(
                                new Paragraph(Anchor.TopLeft, 1, "Burner Driller")
                                {
                                    TextScale = .9f
                                });

                        Globals.ToolTipInterface.mainPanel.AddChild(
                                new Paragraph(Anchor.TopLeft, 1, "You need 32: ")
                                {
                                    TextScale = .5f,
                                    TextColor = Color.Yellow,
                                    PositionOffset = new Vector2(0, 45)
                                });

                        Globals.ToolTipInterface.mainPanel.AddChild(
                                new Image(Anchor.TopLeft, Vector2.One,
                                    new TextureRegion(Globals.Content.Load<Texture2D>("Textures/stone")), true)
                                {
                                    ImageScale = new Vector2(1.3f),
                                    PositionOffset = new Vector2(115, 45)
                                });
                    }

                    else if (isMiscActive) { }
                    else if(isWheaponActive) { }
                }

            };
            craftingElements[6].OnMouseExit = element => { // Burner Driller
                if (!Globals.ToolTipInterface.isItemGrabbed)
                {
                    Globals.ToolTipInterface.mainPanel.IsHidden = true;
                    Globals.ToolTipInterface.mainPanel.RemoveChildren();
                }
            };
        }

        private void RenewCraftingElements() // Just change the icons and tooltips.
        {
            if(isLogisticsActive)
            {
                craftingElements[0].Texture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/conveyorBelt"), padding: 1);
                
                //////////
                
                craftingElements[6].Texture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/İkonlar/craftingElement"), padding: 1);
            }
            else if(isProductionActive)
            {
                craftingElements[0].Texture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/furnace"), padding: 1);

                //////////

                craftingElements[6].Texture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/burnerDriller"), padding: 1);
            }
            else
            {
                Console.WriteLine("No active crafting category");
            }
        }

        public void FlipIsHiddenBool()
        {
            mainPanel.IsHidden = !mainPanel.IsHidden;
        }

        public void ShowInventoryGUI() 
        {
            mainPanel.IsHidden = false;
        }

        public void HideInventoryGUI()
        {
            mainPanel.IsHidden = true;
        }

        public void ResetRightPanelState() // Close furnace, driller panel and open normal craftingPanel
        {
            // Showing crafting panel as default.
            rightPanel.RemoveChildren();
            rightPanel.AddChild(craftingPanel);
        }

        public Element GetElementGUI()
        {
            return mainPanel;
        }

        public Panel GetCraftingPanel()
        {
            return craftingPanel;
        }
    }
}
