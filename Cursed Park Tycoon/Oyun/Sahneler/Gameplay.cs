using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Font;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;
using Sandbox.Engine;
using Sandbox.Engine.Particle;
using Sandbox.Engine.PathFinding;
using Sandbox.Engine.Scene;
using Sandbox.Oyun.Game_Entities;
using Sandbox.Oyun.GUI;
using Sandbox.Oyun.Inventory_System;
using System;
using System.Linq;
using System.Threading;

namespace Sandbox.Oyun.Sahneler
{
    public class Gameplay : Scene
    {
        Sprite mouse_selection_sprite;

        NewWorldGenerator newWorldGenerator;

        Gate enterBattleGate;

        Player player;

        private int fps;
        private int frameCount;
        private float elapsedTime;

        private float targetZoomAmount = 1.0f;
        private float currentZoomAmount = 1.0f; // initial zoom level
        private int previousScrollValue = 0; // previous scroll wheel value

        //private int rail_state = 1; // 1->YUKRARI, 2->AŞAĞI, 3->SAĞ, 4->SOL
        //private bool lock_rail_direction = false;
        //private float X_AXIS_LOCK;
        //private float Y_AXIS_LOCK;

        UiSystem UiSystem;

        GenericInfoGUI genericInfoGUI;
        DebugMenuGUI debugMenuGUI;
        EntityInfoGUI entityInfoGUI;
        MiningGUI miningGUI;

        Camera camera;
        Rectangle visibleArea;

        public RailManager railManager;

        //== A* PATHFINDING ALGORITHM ==//
        SquareGrid grid = new(100, 100);
        //  Start at (1, 1)
        Location startingLocation = new(10800 / 64, 10800 / 64);
        //  End at (10, 6)
        Location endingLocation = new(16000 / 64, 16000 / 64);
        
        AstarSearch aStar;

        Thread thread2;

        public override void Initialize()
        {
            camera = new Camera(Globals.Viewport);
            InventoryManager.Init();
            PlacementManager.Init();
            Globals.entities = SaveManager.Load<Entity>("Saves/save.json");
        }
        public override void LoadContent(Game game1)
        {
            Globals.ToolTipInterface ??= new();

            mouse_selection_sprite = new("Textures/mouse_selection", new Vector2(0, 0), Color.White, new Vector2(2, 2));
            railManager = new();
            player = new("Textures/playerAtlas", new Vector2(6400, 6400), new Vector2(1.6f, 1.6f), Color.White);

            //Globals.entities ??= SaveManager.Load<Entity>("save.json");

            newWorldGenerator = new("Textures/new_terrain_texture", "Textures/water_terrain_texture", 200, 200, 
                    new Vector2(.5f, .5f), new Color(250, 250, 174), 25); // new Color(250, 250, 174)
            newWorldGenerator.SpawnIron();
            newWorldGenerator.SpawnStone();
            newWorldGenerator.SpawnCoal();
            newWorldGenerator.SpawnTree(ref Globals.entities);

            enterBattleGate = new("Textures/gate", new Vector2(6400, 6600), new Vector2(3, 3), Color.White);

            player.SortIntersectingEntities();
            //ddd();
            //thread2 = new(ddd);
            //thread2.Start();

            foreach (var entity in Globals.entities)
            {
                switch (entity.Tag)
                {
                    //case "Iron": // Pozisyon ve 
                    //    entity.TextureSource = "Textures/iron";
                    //    entity.Scale = new Vector2(1.5f, 1.5f);
                    //    entity.IsCollidable = false;
                    //    entity.Details = "Iron";
                    //    entity.Initialize();
                    //    break;
                    case "SBN_LOGO":
                        entity.TextureSource = "Textures/sbn";
                        entity.Scale = new Vector2(2, 2);
                        entity.Details = "SBN Logo";
                        entity.Initialize();
                        break;
                    case "Tree":
                        entity.TextureSource = "Textures/tree1";
                        entity.Scale = new Vector2(1f, 1f);
                        entity.Details = "Tree";
                        entity.Color = Color.White;
                        entity.Initialize();
                        break;
                }
            }

            Globals.entities.Sort((a, b) => a.Rectangle.Bottom.CompareTo(b.Rectangle.Bottom));
            //== GUI SETTINGS ==//

            var style = new UntexturedStyle(Globals.SpriteBatch)
            {
                Font = new GenericSpriteFont(Globals.Content.Load<SpriteFont>("Font/galleryFont")),
                //ButtonTexture = new NinePatch(Globals.Content.Load<Texture2D>("ui"), padding: 1),
                TextScale = 0.6F,
                //PanelTexture = this.testPatch,
                ButtonTexture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/ui"), padding: 1),
                TextFieldTexture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/ui"), padding: 1),
                ScrollBarBackground = new NinePatch(Globals.Content.Load<Texture2D>("Textures/ui"), padding: 1),
                ScrollBarScrollerTexture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/ui"), padding: 1),
                CheckboxTexture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/ui"), padding: 1),
                CheckboxCheckmark = new TextureRegion(Globals.Content.Load<Texture2D>("Textures/ui"), 0, 0, 8, 8),
                RadioTexture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/ui"), padding: 1),
                RadioCheckmark = new TextureRegion(Globals.Content.Load<Texture2D>("Textures/ui"), 0, 0, 8, 8),

                ProgressBarTexture = new NinePatch(Globals.Content.Load<Texture2D>("Textures/ui"), padding: 1),

                TooltipBackground = new NinePatch(Globals.Content.Load<Texture2D>("Textures/ui") ,padding: 1),
                TooltipTextColor = new Color(Color.White, 1),
                TooltipTextWidth = 500,
            };
            UiSystem = new UiSystem(game1, style)
            {
                AutoScaleWithScreen = true, // Ekran boyutuna göre auto scale ediyor. Aspect Ratio olayı...
                GlobalScale = 1f
            };
            UiSystem.OnElementMouseEnter += OnMouseEnterElement;

            debugMenuGUI = new(fps, Vector2.Zero, Globals.rails);
            genericInfoGUI = new();
            entityInfoGUI = new();
            miningGUI = new();

            UiSystem.Add("DebugMenu_Paneli", debugMenuGUI.GetElementGUI());
            //UiSystem.Add("GenericInfoGUI_Paneli", genericInfoGUI.GetElementGUI());
            UiSystem.Add("EntityInfo_Paneli", entityInfoGUI.GetElementGUI());
            UiSystem.Add("Inventory_Paneli", InventoryManager.inventoryGUI.GetElementGUI());
            UiSystem.Add("Mining_Paneli", miningGUI.GetElementGUI());

            UiSystem.Add("ToolTipInterface", Globals.ToolTipInterface.GetElementGUI());

            Globals.entities.Add(player);
        }
        public override void Update(GameTime gameTime, Game game1)
        {
            Globals.ToolTipInterface.UpdateLogic();

            //if(player.isPlayerMoved)
            //{
            //    endingLocation = new((int)player._position.X / 64, (int)player._position.Y / 64);
            //    aStar.CalculatePath(startingLocation, endingLocation);
            //    newWorldGenerator.DrawPathFindingTiles(aStar._calculatedPath);
            //}

            if (InputManager.KeyPressed(Keys.Escape))
            {
                UiSystem.Remove("DebugMenu_Paneli");
                UiSystem.Remove("EntityInfo_Paneli");
                UiSystem.Remove("Inventory_Paneli");
                newWorldGenerator.drawingTiles.Clear();
                Globals.entities.Clear();
                Globals.rails.Clear();
                game1.Exit();
            }
            if(InputManager.KeyPressed(Keys.Y))
            {
                // Item name must match with the textureName
                InventoryManager.AddItem("coal", 999);
                InventoryManager.AddItem("iron", 999);
                InventoryManager.AddItem("stone", 999);
                InventoryManager.AddItem("conveyorBelt", 999);

                SaveManager.SaveInventory("Saves/inventory.json", ref InventoryManager.slots);
            }
            if (InputManager.KeyPressed(Keys.M))
            {
                Globals.entities.Sort((a, b) => a.Rectangle.Bottom.CompareTo(b.Rectangle.Bottom));
            }
            if (InputManager.KeyPressed(Keys.P))
            {
                Globals.SceneManager.SwitchScene(Scenes.Gameplay, game1);
            }
            if (InputManager.KeyPressed(Keys.O))
            {
                Globals.SceneManager.SwitchScene(Scenes.MainMenu, game1);
            }

            //====//

            int currentScrollValue = Mouse.GetState().ScrollWheelValue;
            int scrollDelta = 0; // 120,000005

            if (!Globals.IsMouseEnteredGUIElements) scrollDelta = currentScrollValue - previousScrollValue;

            previousScrollValue = currentScrollValue;
            //float zoomSpeed = 0.002f; // Adjust this value to control zoom speed
            float zoomSpeed = 0.00083f;
            float zoomChange = scrollDelta * zoomSpeed;
            zoomChange = (float)Math.Round(zoomChange, 1, MidpointRounding.AwayFromZero);
            currentZoomAmount += zoomChange;
            float minZoom = 0.1f; // 0.3f
            float maxZoom = 0.9f;
            currentZoomAmount = MathHelper.Clamp(currentZoomAmount, minZoom, maxZoom);

            targetZoomAmount = currentZoomAmount;

            //====//

            // For tile selection
            Vector2 mousePosition = new(Globals.Mouse.X, Globals.Mouse.Y);
            Vector2 transformedMousePosition = Vector2.Transform(mousePosition,
                    Matrix.Invert(camera.GetViewMatrix(targetZoomAmount, player.Position)));
            transformedMousePosition.X /= 64;
            transformedMousePosition.Y /= 64;

            mouse_selection_sprite._position = new Vector2((int)transformedMousePosition.X * 64, (int)transformedMousePosition.Y * 64);
            PlacementManager.UpdateBlueprintPos(new Vector2((int)transformedMousePosition.X * 64, (int)transformedMousePosition.Y * 64),
                    ref railManager);
            railManager.UpdateBlueprintRailPos(new Vector2((int)transformedMousePosition.X * 64, (int)transformedMousePosition.Y * 64));

            Globals.MousePoint = new Point((int)transformedMousePosition.X * 64, (int)transformedMousePosition.Y * 64);

            //== MISC ==//

            //if (!IsThereEntity(new Vector2((int)transformedMousePosition.X * 64, (int)transformedMousePosition.Y * 64)) && 
            //    Globals.Mouse.MiddleButton == ButtonState.Pressed &&
            //            !Globals.IsMouseEnteredGUIElements)
            //{
            //    WoodWarehouse woodWareHouse = new("Textures/wareHouse", new Vector2((int)transformedMousePosition.X * 64, (int)transformedMousePosition.Y * 64),
            //                new Vector2(2,2), Color.White);
            //    Globals.entities.Add(woodWareHouse);
            //}

            ParticleManager.UpdateEmitters();

            InventoryManager.Update();

            frameCount++;
            elapsedTime += Globals.Time;

            if (elapsedTime >= 1.0f)
            {
                fps = frameCount;
                frameCount = 0;
                elapsedTime = 0;
            }

            //====//

            {
                //if (InputManager.KeyPressed(Keys.R))
                //{
                //    if(!IsThereRail(new Vector2((int)transformedMousePosition.X * 64, (int)transformedMousePosition.Y * 64)))
                //    {
                //        // GUNEY yönünde rail varsa
                //        if(IsThereRail(new Vector2((int)transformedMousePosition.X * 64, (int)transformedMousePosition.Y * 64 + 64)))
                //        {
                //
                //        }
                //    }
                //    rail_state = (rail_state % 4) + 1;
                //}
                //if (Globals.Mouse.LeftButton == ButtonState.Released)
                //{
                //    lock_rail_direction = false;
                //    X_AXIS_LOCK = 0;
                //    Y_AXIS_LOCK = 0;
                //}
                //if (Globals.Mouse.LeftButton == ButtonState.Pressed)
                //{
                //    if (!IsThereRail(new Vector2((int)transformedMousePosition.X * 64, (int)transformedMousePosition.Y * 64)) &&
                //            newWorldGenerator.GetTileTexture((int)transformedMousePosition.X, (int)transformedMousePosition.Y) != newWorldGenerator._waterTexture &&
                //            !Globals.IsMouseEnteredGUIElements)
                //    {
                //        if (rail_state == 1)
                //        {
                //            if (!lock_rail_direction)
                //            {
                //                Rail rail = new("new_pistonlar", new Vector2((int)transformedMousePosition.X * 64,
                //                    (int)transformedMousePosition.Y * 64), new Vector2(2f, 2f), Color.White, Rail.RailType.YUKARI);
                //                X_AXIS_LOCK = (int)transformedMousePosition.X * 64;
                //                RailManager.rails.Add(rail);
                //
                //                lock_rail_direction = true;
                //            }
                //            else
                //            {
                //                if(!IsThereRail(new Vector2(X_AXIS_LOCK, (int)transformedMousePosition.Y * 64)))
                //                {
                //                    Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK,
                //                    (int)transformedMousePosition.Y * 64), new Vector2(2f, 2f), Color.White, Rail.RailType.YUKARI);
                //
                //                    RailManager.rails.Add(lockedRail);
                //                }
                //            }
                //
                //        }
                //        else if (rail_state == 2)
                //        {
                //            if (!lock_rail_direction)
                //            {
                //                Rail rail = new("new_pistonlar", new Vector2((int)transformedMousePosition.X * 64,
                //                    (int)transformedMousePosition.Y * 64), new Vector2(2f, 2f), Color.White, Rail.RailType.SAG);
                //                Y_AXIS_LOCK = (int)transformedMousePosition.Y * 64;
                //                RailManager.rails.Add(rail);
                //
                //                lock_rail_direction = true;
                //            }
                //            else
                //            {
                //                if (!IsThereRail(new Vector2((int)transformedMousePosition.X * 64, Y_AXIS_LOCK)))
                //                {
                //                    Rail lockedRail = new("new_pistonlar", new Vector2((int)transformedMousePosition.X * 64,
                //                    Y_AXIS_LOCK), new Vector2(2f, 2f), Color.White, Rail.RailType.SAG);
                //
                //                    RailManager.rails.Add(lockedRail);
                //                }
                //            }
                //        }
                //        else if (rail_state == 3)
                //        {
                //            if (!lock_rail_direction)
                //            {
                //                Rail rail = new("new_pistonlar", new Vector2((int)transformedMousePosition.X * 64,
                //                    (int)transformedMousePosition.Y * 64), new Vector2(2f, 2f), Color.White, Rail.RailType.ASAGI);
                //                X_AXIS_LOCK = (int)transformedMousePosition.X * 64;
                //                RailManager.rails.Add(rail);
                //
                //                lock_rail_direction = true;
                //            }
                //            else
                //            {
                //                if (!IsThereRail(new Vector2(X_AXIS_LOCK, (int)transformedMousePosition.Y * 64)))
                //                {
                //                    Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK,
                //                    (int)transformedMousePosition.Y * 64), new Vector2(2f, 2f), Color.White, Rail.RailType.ASAGI);
                //
                //                    RailManager.rails.Add(lockedRail);
                //                }
                //            }
                //        }
                //        else if (rail_state == 4)
                //        {
                //            if (!lock_rail_direction)
                //            {
                //                Rail rail = new("new_pistonlar", new Vector2((int)transformedMousePosition.X * 64,
                //                    (int)transformedMousePosition.Y * 64), new Vector2(2f, 2f), Color.White, Rail.RailType.SOL);
                //                Y_AXIS_LOCK = (int)transformedMousePosition.Y * 64;
                //                RailManager.rails.Add(rail);
                //
                //                lock_rail_direction = true;
                //            }
                //            else
                //            {
                //                if (!IsThereRail(new Vector2((int)transformedMousePosition.X * 64, Y_AXIS_LOCK)))
                //                {
                //                    Rail lockedRail = new("new_pistonlar", new Vector2((int)transformedMousePosition.X * 64,
                //                     Y_AXIS_LOCK), new Vector2(2f, 2f), Color.White, Rail.RailType.SOL);
                //
                //                    RailManager.rails.Add(lockedRail);
                //                }
                //            }
                //        }
                //    }
                //
                //    
                //
                //}
            }

            //player.Update();
            //player.OnCollisionEnter();
            player.OnCollisionEnter(Globals.rails);
            player.OnCollisionEnter(ref newWorldGenerator.drawingTiles);
            player.Mining(ref Globals.entities, ref miningGUI);

            debugMenuGUI.UpdateDebugMenuText(fps,
                    new Vector2((int)transformedMousePosition.X, (int)transformedMousePosition.Y));

            //====//

            UiSystem.Update(gameTime);

            //====//

            mouse_selection_sprite.SetVisible(false);
            entityInfoGUI.entityInstance = null;
            entityInfoGUI.UpdateEntityInfoImage(entityInfoGUI.entityInstance);

            foreach (var entity in Globals.entities.ToArray())
            {
                entity.UpdateLogic();
                entity.CheckIfSelected(Globals.MousePoint);
                entity.OnCollisionEnter(); // Stone collision için ekledim!!!!!!!!!!!!!!!!!!!!

                if (entity.IsRemoved == true)
                {
                    Globals.entities.Remove(entity);
                    continue; // Aşağı kısmı görmezden gel.
                }
                if (entity.IsHovered && !Globals.IsMouseEnteredGUIElements)
                {
                    entityInfoGUI.entityInstance = entity;
                    entityInfoGUI.UpdateEntityInfoImage(entityInfoGUI.entityInstance);

                    mouse_selection_sprite.SetVisible(true);
                }
            }
            foreach (var rail in Globals.rails.ToList())
            {
                rail.CheckIfSelectedRail(Globals.MousePoint);
                if (rail.IsRemoved == true)
                {
                    Globals.rails.Remove(rail);
                    continue;
                }
                if (rail.IsHovered && !Globals.IsMouseEnteredGUIElements)
                {
                    entityInfoGUI.entityInstance = rail;
                    entityInfoGUI.UpdateEntityInfoImage(entityInfoGUI.entityInstance);

                    mouse_selection_sprite.SetVisible(true);
                }
            }

            camera.Position = player.Position - new Vector2(camera.Viewport.Width / 2, camera.Viewport.Height / 2);

            // For Vieport Culling, last parameter is for the offset value of the viewport.
            //if(scrollDelta != 0 || Keyboard.GetState().GetPressedKeyCount() > 0)
            //{
            //    visibleArea = camera.GetVisibleArea(targetZoomAmount, player._position, new Vector2(100, 100));
            //}
            visibleArea = camera.GetVisibleArea(targetZoomAmount, player.Position, new Vector2(100, 100));

            camera.Viewport = Globals.Viewport;
        }

        public override void Draw(GameTime gameTime, Game game1)
        {
            Globals.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap,
                    transformMatrix: camera.GetViewMatrix(targetZoomAmount, player.Position));

            //== DRAW ACCORDING TO THE VIEWPORT ==//
            foreach (var tile in newWorldGenerator.drawingTiles) // Şuanda 160,000 tile'ı check ediyor o yüzden -> Spatial Partioning
            {
                if (visibleArea.Contains(new Vector2(tile.x, tile.y)))
                {
                    newWorldGenerator.DrawTile(tile);
                }
            }
            foreach (var mine in newWorldGenerator.mineEntities) // I seperated mine entities from general entities list
            {
                if (visibleArea.Contains(mine.Position))
                {
                    mine.DrawEntity();
                }
            }
            foreach (var rail in Globals.rails)
            {
                if (visibleArea.Contains(rail.Position))
                {
                    rail.DrawRail();
                }
            }
            enterBattleGate.DrawBackgroundEffect();
            foreach (var entity in Globals.entities)
            {
                if (visibleArea.Contains(entity.Position))
                {
                    if(entity.hasAnimation)
                    {
                        entity.DrawEntityAnimation();
                    }
                    else
                    {
                        entity.DrawEntity();
                    }
                }
            }
            railManager.DrawBlueprintRail();
            PlacementManager.DrawBlueprint();
            mouse_selection_sprite.DrawSprite();
            //player.DrawAnimation();
            //player.DrawEntityAnimation();
            ParticleManager.Draw();

            //newWorldGenerator.DrawSpatialPartRectangles();

            Globals.SpriteBatch.End();

            {
                //Globals.SpriteBatch.Begin(transformMatrix: camera.GetViewMatrix(targetZoomAmount, player.Position));
                //foreach (var entity in Globals.entities)
                //{
                //    if (visibleArea.Intersects(entity.Rectangle))
                //    {
                //        //Globals.DrawRectangle(Globals.Content.Load<Texture2D>("Textures/ui"), entity.Rectangle, Color.Blue, 0.5f);
                //        Globals.DrawRectangle(Globals.Content.Load<Texture2D>("Textures/ui"), entity.sortRectangle, Color.Pink, 1f);
                //    }
                //}
                //foreach (var rail in Globals.rails)
                //{
                //    if (visibleArea.Contains(rail.Position)) 
                //    {
                //        Globals.DrawRectangle(Globals.Content.Load<Texture2D>("Textures/ui"), rail.Rectangle, Color.Blue, 0.5f);
                //    }
                //}
                //foreach(var waterTile in newWorldGenerator.tiles)
                //{
                //    if(waterTile.isWall)
                //    {
                //        Globals.DrawRectangle(Globals.Content.Load<Texture2D>("Textures/ui"), waterTile.rectangle, Color.Pink, 1f);
                //    }
                //}
                //Globals.SpriteBatch.End();
            }
            
            UiSystem.Draw(gameTime, Globals.SpriteBatch);
        }

        private void OnMouseEnterElement(Element element)
        {
            // Change the global static boolean variable
            Globals.IsMouseEnteredGUIElements = true;

            // Optionally, you can subscribe to the OnElementMouseExit event to reset the variable
            UiSystem.OnElementMouseExit += OnMouseExitElement;
        }

        private void OnMouseExitElement(Element element)
        {
            // Reset the global static boolean variable when the mouse leaves the element
            Globals.IsMouseEnteredGUIElements = false;

            // Unsubscribe from the OnElementMouseExit event to avoid memory leaks
            UiSystem.OnElementMouseExit -= OnMouseExitElement;
        }

        public void ddd()
        {
            if (newWorldGenerator.GetTileFrameIndex(endingLocation.X, endingLocation.Y) != 0) // water değil ise
            {
                foreach (var tile in newWorldGenerator.drawingTiles)
                {
                    if (tile.isWall)
                    {
                        grid.walls.Add(new Location(tile.x / 64, tile.y / 64));
                    }
                }

                //  Calculate the path
                aStar = new(grid);
                aStar.CalculatePath(startingLocation, endingLocation);
                newWorldGenerator.DrawPathFindingTiles(aStar._calculatedPath);
            }
        }
    }
}
