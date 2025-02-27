using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using Sandbox.Engine;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox.Oyun.Inventory_System
{
    public class ItemSlot
    {
        //== For Saving Inside Json File ==//
        public string Item { get; set; }
        public string Count { get; set; }
        //======//

        public Button slotButton;
        public Image slotImage;
        public Paragraph itemAmountText;

        public Item item;

        public bool isItemPlaced = false;

        public readonly int maxCount = 999;

        // For leftpanel inventory

        public ItemSlot() { } // Parameterless constructor needed for deserialization
        public ItemSlot(int x, int y) 
        {
            Globals.soundEffect ??= Globals.Content.Load<SoundEffect>("Audios/button_hover");

            item = new();

            InitializeSlotImage();

            InitializeItemAmountText();

            slotButton = new Button(Anchor.TopLeft, new Vector2(55, 55), "")
            {
                OnPressed = element =>
                {
                    CheckItemName();
                    Globals.soundEffect.Play(0.1f, .85f, 0);
                },
                OnMouseEnter = element =>
                {
                    slotImage.ImageScale = new Vector2(1.2f, 1.2f);
                    itemAmountText.TextScale = .7f;
                },
                OnMouseExit = element =>
                {
                    slotImage.ImageScale = new Vector2(1, 1);
                    itemAmountText.TextScale = .65f;
                },
                PositionOffset = new Vector2(x * 59, y * 60 + 40),
                Texture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/İkonlar/inventoryElement"), padding: 1),
            };

            slotButton.AddChild(slotImage);
            slotButton.AddChild(itemAmountText);
        }

        // For rightPanel's -> furnace, driller interface itemSlot
        public ItemSlot(int x, int y, int buttonSize)
        {
            Globals.soundEffect = Globals.Content.Load<SoundEffect>("Audios/button_hover");

            item = new();

            itemAmountText = new Paragraph(Anchor.BottomCenter, 1, "", true)
            {
                PositionOffset = new Vector2(0, 0)
            };

            slotImage = new Image(Anchor.Center, new Vector2(.75f, .75f),
                new TextureRegion(Globals.Content.Load<Texture2D>("Textures/İkonlar/inventoryElement")), scaleToImage: false)
            {
                DrawAlpha = 0f
            };

            slotButton = new Button(Anchor.TopLeft, new Vector2(buttonSize, buttonSize), "")
            {
                OnPressed = element =>
                {
                    CheckItemName();
                    Globals.soundEffect.Play(0.1f, .85f, 0);
                },
                OnMouseEnter = element =>
                {
                    slotImage.ImageScale = new Vector2(1.2f, 1.2f);
                    itemAmountText.TextScale = .7f;
                },
                OnMouseExit = element =>
                {
                    slotImage.ImageScale = new Vector2(1, 1);
                    itemAmountText.TextScale = .65f;
                },
                PositionOffset = new Vector2(x, y),
                Texture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/İkonlar/inventoryElement"), padding: 1),
            };

            slotButton.AddChild(slotImage);
            slotButton.AddChild(itemAmountText);
        }

        public void InitializeSlotButton()
        {
            slotButton = new Button(Anchor.TopLeft, new Vector2(55, 55), "")
            {
                OnPressed = element =>
                {
                    CheckItemName();
                    Globals.soundEffect.Play(0.1f, .85f, 0);
                },
                OnMouseEnter = element =>
                {
                    slotImage.ImageScale = new Vector2(1.2f, 1.2f);
                    itemAmountText.TextScale = .7f;
                },
                OnMouseExit = element =>
                {
                    slotImage.ImageScale = new Vector2(1, 1);
                    itemAmountText.TextScale = .65f;
                },
                Texture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/İkonlar/inventoryElement"), padding: 1),
            };
        }

        public void InitializeSlotImage()
        {
            slotImage = new Image(Anchor.Center, new Vector2(.75f, .75f),
                new TextureRegion(Globals.Content.Load<Texture2D>("Textures/İkonlar/inventoryElement")), scaleToImage: false)
            {
                DrawAlpha = 0f
            };
        }

        public void InitializeItemAmountText()
        {
            itemAmountText = new Paragraph(Anchor.BottomRight, 1, "", true)
            {
                PositionOffset = new Vector2(2, 2),
                TextScale = .65f,
            };
        }

        private void CheckItemName()
        {
            PlacementManager.isActive = false;

            // If itemSlot has an 'item'
            if (item.itemName != "")
            {
                if (Globals.ToolTipInterface.isItemGrabbed)
                {
                    // THERE IS 2 OPTION: SWAP ITEMS OR STACK ITEMS

                    // STACKING ITEMS
                    if(item.itemName == Globals.ToolTipInterface.mainPanel.GetData<string>("ItemName"))
                    {
                        if((item.count + Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount")) > maxCount)
                        {
                            int tasanSayi = (item.count + Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount")) - maxCount;

                            item.count += Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount") - tasanSayi;
                            itemAmountText.Text = item.count.ToString();

                            Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", tasanSayi);

                            Globals.ToolTipInterface.mainPanel.GetChildren<Paragraph>().First().Text = 
                                    Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount").ToString();
                        }
                        else
                        {
                            item.count += Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount");
                            itemAmountText.Text = item.count.ToString();

                            Globals.ToolTipInterface.mainPanel.SetData<string>("ItemName", "");
                            Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", 0);

                            Globals.ToolTipInterface.mainPanel.IsHidden = true;
                            Globals.ToolTipInterface.mainPanel.RemoveChildren();
                            Globals.ToolTipInterface.isItemGrabbed = false;
                        }



                    }
                    // SWAP ITEMS
                    else
                    {
                        // yedek tooltip datası temp swapping için lazım
                        string tempItemName = Globals.ToolTipInterface.mainPanel.GetData<string>("ItemName");
                        int tempCount = Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount");

                        // ItemSlottakini tooltipe koy.
                        {
                            Globals.ToolTipInterface.mainPanel.SetData<string>("ItemName", item.itemName);
                            Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", item.count);

                            Globals.ToolTipInterface.mainPanel.RemoveChildren();
                            Globals.ToolTipInterface.mainPanel.Size = new Vector2(60, 60);
                            Globals.ToolTipInterface.mainPanel.IsHidden = false;
                            Globals.ToolTipInterface.mainPanel.AddChild(
                                new Panel(Anchor.TopCenter, new Vector2(40, 40), true)
                                .AddChild(
                                    new Image(Anchor.Center, Vector2.One,
                                        new TextureRegion(Globals.Content.Load<Texture2D>(
                                            $"Textures/{item.itemName}")), false)

                                    {
                                        ImageScale = new Vector2(1f),
                                    }));
                            Globals.ToolTipInterface.mainPanel.AddChild(
                                    new Paragraph(Anchor.BottomCenter, 1,
                                        item.count.ToString())

                                    {
                                        TextScale = .60f,
                                    });


                        }

                        // temp dataları itemslota koyacağız.
                        {
                            item.itemName = tempItemName;
                            item.count = tempCount;

                            slotImage.Texture = new TextureRegion(Globals.Content.Load<Texture2D>($"Textures/{item.itemName}"));
                            slotImage.DrawAlpha = 1f;
                            itemAmountText.Text = item.count.ToString();
                        }
                    }
                }
                else
                {
                    // PICK UP ITEM

                    // ToolTipInterface'e itemSlot'un datalarını koyduk.
                    {
                        Globals.ToolTipInterface.mainPanel.SetData<string>("ItemName", item.itemName);
                        Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", item.count);

                        Globals.ToolTipInterface.mainPanel.RemoveChildren();
                        Globals.ToolTipInterface.mainPanel.Size = new Vector2(60, 60);
                        Globals.ToolTipInterface.mainPanel.IsHidden = false;
                        Globals.ToolTipInterface.mainPanel.AddChild(
                            new Panel(Anchor.TopCenter, new Vector2(40, 40), true)
                            .AddChild(
                                new Image(Anchor.Center, Vector2.One,
                                    new TextureRegion(Globals.Content.Load<Texture2D>(
                                        $"Textures/{Globals.ToolTipInterface.mainPanel.GetData<string>("ItemName")}")), false)

                                {
                                    ImageScale = new Vector2(1f),
                                }));
                        Globals.ToolTipInterface.mainPanel.AddChild(
                                new Paragraph(Anchor.BottomCenter, 1,
                                Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount").ToString())

                                {
                                    TextScale = .60f,
                                });
                    }

                    // Şimdi itemSlotu resetleyeceğiz.
                    {
                        item.itemName = "";
                        item.count = 0;
                        isItemPlaced = false;

                        slotImage.Texture = new TextureRegion(Globals.Content.Load<Texture2D>("Textures/İkonlar/inventoryElement"));
                        slotImage.DrawAlpha = 0f;
                        itemAmountText.Text = "";
                    }

                    Globals.ToolTipInterface.isItemGrabbed = true;
                }
            }
            else // If itemSlot is empty
            {
                if (Globals.ToolTipInterface.isItemGrabbed)
                {
                    // DEPLOY ITEM

                    // ItemSlot'a tooltipInterface'deki dataları koyacağız (İleride maxCount'a da bakıcaz STACK)
                    {
                        item.itemName = Globals.ToolTipInterface.mainPanel.GetData<string>("ItemName");
                        item.count = Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount");
                        isItemPlaced = true;

                        slotImage.Texture = new TextureRegion(Globals.Content.Load<Texture2D>($"Textures/{item.itemName}"));
                        slotImage.DrawAlpha = 1f;
                        itemAmountText.Text = item.count.ToString();
                    }

                    // Şimdi tooltipInterface'i kapat (İleride maxCount'a da bakıcaz STACK)
                    {
                        Globals.ToolTipInterface.mainPanel.SetData<string>("ItemName", "");
                        Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", 0);

                        Globals.ToolTipInterface.mainPanel.IsHidden = true;
                        Globals.ToolTipInterface.mainPanel.RemoveChildren();
                        Globals.ToolTipInterface.isItemGrabbed = false;
                    }

                    Globals.ToolTipInterface.isItemGrabbed = false;
                }
            }
        
            // Checking if this pressed slotButton is placable item or not
            // If so then activate PlacementManager
            if(Globals.ToolTipInterface.isItemGrabbed && Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount") > 0)
            {
                // IF PLACABLE ITEM THEN ACTIVATE PLACEMENTMANAGER

                switch (Globals.ToolTipInterface.mainPanel.GetData<string>("ItemName"))
                {
                    case "conveyorBelt":
                        PlacementManager.isActive = true;
                        PlacementManager.workOnce = false;
                        PlacementManager.selectedItem = "conveyorBelt";
                        break;
                    case "furnace":
                        PlacementManager.isActive = true;
                        PlacementManager.workOnce = false;
                        PlacementManager.selectedItem = "furnace";
                        break;
                    case "burnerDriller":
                        PlacementManager.isActive = true;
                        PlacementManager.workOnce = false;
                        PlacementManager.selectedItem = "burnerDriller";
                        break;
                }
            }
        }

        //== For input fuel itemSlot and other itemslots that needs specific items ==//
        private void CheckSpecificItemName(List<string> specificItems)
        {
            PlacementManager.isActive = false;

            // If itemSlot has an 'item'
            if (item.itemName != "")
            {

                if (Globals.ToolTipInterface.isItemGrabbed)
                {
                    // THERE IS 2 OPTION: SWAP ITEMS OR STACK ITEMS

                    // STACKING ITEMS
                    if (item.itemName == Globals.ToolTipInterface.mainPanel.GetData<string>("ItemName"))
                    {
                        if ((item.count + Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount")) > maxCount)
                        {
                            int tasanSayi = (item.count + Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount")) - maxCount;

                            item.count += Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount") - tasanSayi;
                            itemAmountText.Text = item.count.ToString();

                            Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", tasanSayi);

                            Globals.ToolTipInterface.mainPanel.GetChildren<Paragraph>().First().Text =
                                    Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount").ToString();
                        }
                        else
                        {
                            item.count += Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount");
                            itemAmountText.Text = item.count.ToString();

                            Globals.ToolTipInterface.mainPanel.SetData<string>("ItemName", "");
                            Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", 0);

                            Globals.ToolTipInterface.mainPanel.IsHidden = true;
                            Globals.ToolTipInterface.mainPanel.RemoveChildren();
                            Globals.ToolTipInterface.isItemGrabbed = false;
                        }



                    }
                    // SWAP ITEMS
                    else
                    {
                        // yedek tooltip datası temp swapping için lazım
                        string tempItemName = Globals.ToolTipInterface.mainPanel.GetData<string>("ItemName");
                        int tempCount = Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount");

                        // ItemSlottakini tooltipe koy.
                        {
                            Globals.ToolTipInterface.mainPanel.SetData<string>("ItemName", item.itemName);
                            Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", item.count);

                            Globals.ToolTipInterface.mainPanel.RemoveChildren();
                            Globals.ToolTipInterface.mainPanel.Size = new Vector2(60, 60);
                            Globals.ToolTipInterface.mainPanel.IsHidden = false;
                            Globals.ToolTipInterface.mainPanel.AddChild(
                                new Panel(Anchor.TopCenter, new Vector2(40, 40), true)
                                .AddChild(
                                    new Image(Anchor.Center, Vector2.One,
                                        new TextureRegion(Globals.Content.Load<Texture2D>(
                                            $"Textures/{item.itemName}")), false)

                                    {
                                        ImageScale = new Vector2(1f),
                                    }));
                            Globals.ToolTipInterface.mainPanel.AddChild(
                                    new Paragraph(Anchor.BottomCenter, 1,
                                        item.count.ToString())

                                    {
                                        TextScale = .60f,
                                    });


                        }

                        // temp dataları itemslota koyacağız.
                        {
                            item.itemName = tempItemName;
                            item.count = tempCount;

                            slotImage.Texture = new TextureRegion(Globals.Content.Load<Texture2D>($"Textures/{item.itemName}"));
                            slotImage.DrawAlpha = 1f;
                            itemAmountText.Text = item.count.ToString();
                        }
                    }
                }
                else
                {
                    // PICK UP ITEM

                    // ToolTipInterface'e itemSlot'un datalarını koyduk.
                    {
                        Globals.ToolTipInterface.mainPanel.SetData<string>("ItemName", item.itemName);
                        Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", item.count);

                        Globals.ToolTipInterface.mainPanel.RemoveChildren();
                        Globals.ToolTipInterface.mainPanel.Size = new Vector2(60, 60);
                        Globals.ToolTipInterface.mainPanel.IsHidden = false;
                        Globals.ToolTipInterface.mainPanel.AddChild(
                            new Panel(Anchor.TopCenter, new Vector2(40, 40), true)
                            .AddChild(
                                new Image(Anchor.Center, Vector2.One,
                                    new TextureRegion(Globals.Content.Load<Texture2D>(
                                        $"Textures/{Globals.ToolTipInterface.mainPanel.GetData<string>("ItemName")}")), false)

                                {
                                    ImageScale = new Vector2(1f),
                                }));
                        Globals.ToolTipInterface.mainPanel.AddChild(
                                new Paragraph(Anchor.BottomCenter, 1,
                                Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount").ToString())

                                {
                                    TextScale = .60f,
                                });
                    }

                    // Şimdi itemSlotu resetleyeceğiz.
                    {
                        item.itemName = "";
                        item.count = 0;
                        isItemPlaced = false;

                        slotImage.Texture = new TextureRegion(Globals.Content.Load<Texture2D>("Textures/İkonlar/inventoryElement"));
                        slotImage.DrawAlpha = 0f;
                        itemAmountText.Text = "";
                    }

                    Globals.ToolTipInterface.isItemGrabbed = true;
                }
            }
            else // If itemSlot is empty
            {
                //for()
                //{
                //
                //}

                if (Globals.ToolTipInterface.isItemGrabbed)
                {
                    // DEPLOY ITEM

                    // ItemSlot'a tooltipInterface'deki dataları koyacağız (İleride maxCount'a da bakıcaz STACK)
                    {
                        item.itemName = Globals.ToolTipInterface.mainPanel.GetData<string>("ItemName");
                        item.count = Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount");
                        isItemPlaced = true;

                        slotImage.Texture = new TextureRegion(Globals.Content.Load<Texture2D>($"Textures/{item.itemName}"));
                        slotImage.DrawAlpha = 1f;
                        itemAmountText.Text = item.count.ToString();
                    }

                    // Şimdi tooltipInterface'i kapat (İleride maxCount'a da bakıcaz STACK)
                    {
                        Globals.ToolTipInterface.mainPanel.SetData<string>("ItemName", "");
                        Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", 0);

                        Globals.ToolTipInterface.mainPanel.IsHidden = true;
                        Globals.ToolTipInterface.mainPanel.RemoveChildren();
                        Globals.ToolTipInterface.isItemGrabbed = false;
                    }

                    Globals.ToolTipInterface.isItemGrabbed = false;
                }
            }

            // Checking if this pressed slotButton is placable item or not
            // If so then activate PlacementManager
            if (Globals.ToolTipInterface.isItemGrabbed && Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount") > 0)
            {
                // IF PLACABLE ITEM THEN ACTIVATE PLACEMENTMANAGER

                switch (Globals.ToolTipInterface.mainPanel.GetData<string>("ItemName"))
                {
                    case "conveyorBelt":
                        PlacementManager.isActive = true;
                        PlacementManager.workOnce = false;
                        PlacementManager.selectedItem = "conveyorBelt";
                        break;
                    case "furnace":
                        PlacementManager.isActive = true;
                        PlacementManager.workOnce = false;
                        PlacementManager.selectedItem = "furnace";
                        break;
                    case "burnerDriller":
                        PlacementManager.isActive = true;
                        PlacementManager.workOnce = false;
                        PlacementManager.selectedItem = "burnerDriller";
                        break;
                }
            }
        }

        public void SetToolTipText(string text)
        {
            slotButton.Tooltip = new Tooltip(text, slotButton);
        }

        public void SetTexture(string textureName)
        {
            slotImage.Texture = new TextureRegion(Globals.Content.Load<Texture2D>("Textures/" + textureName));
            slotImage.DrawAlpha = 1f;
        }

        public void DefaultTooltip()
        {
            slotButton.Tooltip = null;
        }

        public void DefaultTexture()
        {
            slotImage.DrawAlpha = 0f;
            slotImage.Texture = new TextureRegion(Globals.Content.Load<Texture2D>("Textures/ui"));
        }

    }
}
