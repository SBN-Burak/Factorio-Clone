using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Ui.Elements;
using Sandbox.Oyun;
using Sandbox.Oyun.Game_Entities;
using Sandbox.Oyun.Game_Entities.Driller;
using System.Linq;

namespace Sandbox.Engine
{
    public static class PlacementManager
    {
        // TO INITIALIZE ONCE AND AVOID DUPLICATION WHEN RELOADING SCENE
        private static bool isInitialized = false;

        public static bool isActive = false;

        public static string selectedItem;

        private static Entity blueprint;

        private static Color blueprintColor = Color.Green;
        private static Texture2D drawingRectangleTexture; // For debug rectangle draw

        //----------------------//

        public static bool workOnce = false;

        public static void Init()
        {
            if(!isInitialized)
            {
                blueprint = new Entity("Textures/ui", Vector2.Zero, Vector2.One, Color.White)
                {
                    Color = new Color(Color.White, 0.5f),
                    Tag = "Blueprint",
                };
                drawingRectangleTexture = Globals.Content.Load<Texture2D>("Textures/ui");

                isInitialized = true;
            }
        }

        public static void UpdateBlueprintPos(Vector2 position, ref RailManager railManager)
        {
            if(isActive)
            {
                if(!InventoryManager.HasRequiredItemsInToolTipInterface())
                {
                    return;
                }
                else
                {
                    if (!workOnce)
                    {
                        switch (selectedItem)
                        {
                            case "conveyorBelt":
                                blueprint.TextureSource = "Textures/" + selectedItem; // texture2D ataması blueprint.Initialize(); yapılıyor
                                blueprint.Initialize(12, 1);
                                blueprint.Scale = new Vector2(2f, 2f);
                                blueprint.Rectangle = new Rectangle(blueprint.Rectangle.X, blueprint.Rectangle.Y, 64, 64);
                                break;
                            case "furnace":
                                blueprint.TextureSource = "Textures/furnace"; // texture2D ataması blueprint.Initialize(); yapılıyor
                                blueprint.Initialize();
                                blueprint.Scale = new Vector2(2.25f, 2.25f);
                                blueprint.Rectangle = new Rectangle(blueprint.Rectangle.X, blueprint.Rectangle.Y, 128, 128);
                                break;
                            case "burnerDriller":
                                blueprint.TextureSource = "Textures/burnerDriller"; // texture2D ataması blueprint.Initialize(); yapılıyor
                                blueprint.InitializeWithMultiply(5, 1, 2, 2);
                                blueprint.Scale = new Vector2(2.25f, 2.25f);
                                blueprint.Rectangle = new Rectangle(blueprint.Rectangle.X, blueprint.Rectangle.Y, 128, 128);
                                break;
                        }

                        workOnce = true;
                    }

                    if (!IsThereEntity() && !IsThereRail() &&
                    Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements)
                    {
                        switch (selectedItem)
                        {
                            case "conveyorBelt":
                                blueprint.Color = new Color(Color.White, 0f);

                                railManager.Activate();

                                //== Decrease by 1 everytime we place an item. ==//
                                Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount",
                                        Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount") - 1);

                                //== TODO: Also update the text inside tooltipInterface ==//
                                Globals.ToolTipInterface.mainPanel.GetChildren<Paragraph>().First().Text =
                                    Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount").ToString();

                                //== Checking if 'ItemCount' is 0 then close and reset the tooltipInterface ==//
                                if (Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount") <= 0)
                                {
                                    selectedItem = "";
                                    isActive = false;

                                    railManager.Deactivate();

                                    Globals.ToolTipInterface.mainPanel.SetData<string>("ItemName", "");
                                    Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", 0);

                                    Globals.ToolTipInterface.mainPanel.IsHidden = true;
                                    Globals.ToolTipInterface.mainPanel.RemoveChildren();
                                    Globals.ToolTipInterface.isItemGrabbed = false;
                                }
                                break;
                            
                            case "furnace":
                                blueprint.TextureSource = "Textures/furnace";
                                blueprint.Color = new Color(Color.White, 0.5f);

                                Furnace fn = new("Textures/furnace1", blueprint.Position, blueprint.Scale, Color.White);
                                Globals.entities.Add(fn);

                                //blueprint.Rectangle = fn.Rectangle;

                                //== Decrease by 1 everytime we place an item. ==//
                                Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount",
                                        Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount") - 1);

                                //== TODO: Also update the text inside tooltipInterface ==//
                                Globals.ToolTipInterface.mainPanel.GetChildren<Paragraph>().First().Text =
                                    Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount").ToString();

                                //== Checking if 'ItemCount' is 0 then close and reset the tooltipInterface ==//
                                if (Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount") <= 0)
                                {
                                    selectedItem = "";
                                    isActive = false;
                                
                                    Globals.ToolTipInterface.mainPanel.SetData<string>("ItemName", "");
                                    Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", 0);
                                
                                    Globals.ToolTipInterface.mainPanel.IsHidden = true;
                                    Globals.ToolTipInterface.mainPanel.RemoveChildren();
                                    Globals.ToolTipInterface.isItemGrabbed = false;
                                }
                                break;
                            
                            case "burnerDriller":
                                blueprint.TextureSource = "Textures/burnerDriller1";
                                blueprint.Color = new Color(Color.LightGray, 0.5f);

                                BurnerDriller bd = new(blueprint.TextureSource, blueprint.Position, blueprint.Scale, Color.White);
                                bd.OnCollisionEnter();
                                Globals.entities.Add(bd);

                                //blueprint.Rectangle = bd.Rectangle;

                                //== Decrease by 1 everytime we place an item. ==//
                                Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount",
                                        Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount") - 1);

                                //== TODO: Also update the text inside tooltipInterface ==//
                                Globals.ToolTipInterface.mainPanel.GetChildren<Paragraph>().First().Text =
                                    Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount").ToString();

                                //== Checking if 'ItemCount' is 0 then close and reset the tooltipInterface ==//
                                if (Globals.ToolTipInterface.mainPanel.GetData<int>("ItemCount") <= 0)
                                {
                                    selectedItem = "";
                                    isActive = false;

                                    Globals.ToolTipInterface.mainPanel.SetData<string>("ItemName", "");
                                    Globals.ToolTipInterface.mainPanel.SetData<int>("ItemCount", 0);

                                    Globals.ToolTipInterface.mainPanel.IsHidden = true;
                                    Globals.ToolTipInterface.mainPanel.RemoveChildren();
                                    Globals.ToolTipInterface.isItemGrabbed = false;
                                }
                                break;
                        }
                    }


                    if(!IsThereEntity())
                    {
                        blueprintColor = Color.Green;
                    }
                    else
                    {
                        blueprintColor = Color.Red;
                    }
               
                    
                }

                blueprint.Position = position;

                blueprint.UpdateRectPos(blueprint.Rectangle.Width, blueprint.Rectangle.Height);
            }
        }

        public static void DrawBlueprint()
        {
            if (isActive)
            {
                blueprint.DrawEntity();
                Globals.DrawRectangle(drawingRectangleTexture, blueprint.Rectangle, blueprintColor, .5f);
            }
        }


        private static bool IsThereEntity()
        {
            foreach (var entity in Globals.entities)
            {
                if (entity.Rectangle.Intersects(blueprint.Rectangle))
                {
                    // There is entity except if it is a mine. So we can deploy any entity on top of mines
                    if (entity.Tag != "Stone" && entity.Tag != "Iron" && entity.Tag != "Coal")
                    {
                        return true;
                    }
                }
            }

            return false;
        }



        private static bool IsThereRail()
        {
            foreach (var rail in Globals.rails)
            {
                if (rail.Rectangle.Intersects(blueprint.Rectangle))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
