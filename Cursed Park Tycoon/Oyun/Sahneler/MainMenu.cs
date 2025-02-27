using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MLEM.Font;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;
using Sandbox.Engine;
using Sandbox.Engine.Particle;
using Sandbox.Engine.Scene;
using Sandbox.Oyun.GUI;

namespace Sandbox.Oyun.Sahneler
{
    public class MainMenu : Scene
    {
        UiSystem UiSystem;
        SplashScreen splashScreenGUI;
        MainMenuGUI mainMenuGUI;

        Texture2D ui_texture;

        ///////

        NewWorldGenerator newWorldGenerator;

        Camera camera;

        Effect vignetteEffect;
        RenderTarget2D vignetteRenderTarget;

        //== MUSIC ===//
        Song backgroundSong;

        //== Shader Effects ==//
        Effect crtEffect;
        RenderTarget2D crtEffectRenderTarget;

        public override void Initialize()
        {
            camera = new Camera(Globals.Viewport)
            {
                Position = new Vector2(2240, 1600)
            };

            Globals.entities = SaveManager.Load<Entity>("Saves/menu.json");
        }
        public override void LoadContent(Game game1)
        {
            crtEffect = Globals.Content.Load<Effect>("Shaders/CRTEffect");

            crtEffect.Parameters["Resolution"].SetValue(new Vector2(Globals.Viewport.Width*4, Globals.Viewport.Height*4));
            crtEffect.Parameters["ScanlineIntensity"].SetValue(0.6f);
            crtEffect.Parameters["ScanlineSize"].SetValue(3.0f);
            crtEffect.Parameters["Curvature"].SetValue(-0.03f);

            backgroundSong = Globals.Content.Load<Song>("Audios/piano1");
            //MediaPlayer.Play(backgroundSong);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.05f;

            ////////////////// GUI MLEM //////////////////
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

                TooltipBackground = new NinePatch(Globals.Content.Load<Texture2D>("Textures/ui"), padding: 1),
                TooltipTextColor = new Color(Color.White, 1),
                TooltipTextWidth = 500,
            };

            UiSystem = new UiSystem(game1, style)
            {
                AutoScaleWithScreen = true, // Ekran boyutuna göre auto scale ediyor. Aspect Ratio olayı...
                GlobalScale = 1f
            };
            UiSystem.OnElementMouseEnter += OnMouseEnterElement;

            splashScreenGUI = new(6); // 1 Seconds Splash Screen
            mainMenuGUI = new(game1);

            UiSystem.Add("SplashScreen_Paneli", splashScreenGUI.GetElementGUI());
            UiSystem.Add("MainMenu_Paneli", mainMenuGUI.GetElementGUI());

            //============= ENTITIES =============//

            ui_texture = Globals.Content.Load<Texture2D>("Textures/ui");

            //Globals.entities = SaveManager.Load<Entity>("menu.json");

            newWorldGenerator = new("Textures/new_terrain_texture", "Textures/water_terrain_texture", 70, 50, new Vector2(.5f, .5f),
                    Color.LawnGreen, 10); // new Color(250, 250, 174)
            //Color.LightYellow
            newWorldGenerator.SpawnIron();
            newWorldGenerator.SpawnStone();
            newWorldGenerator.SpawnCoal();
            newWorldGenerator.SpawnTree(ref Globals.entities);

            Globals.entities.Sort((a, b) => a.Position.Y.CompareTo(b.Position.Y));

            foreach (var entity in Globals.entities)
            {
                switch (entity.Tag)
                {
                    case "SBN_LOGO":
                        entity.TextureSource = "Textures/sbn";
                        entity.Scale = new Vector2(2f, 2f);
                        entity.Initialize();
                        break;
                    case "Tree":
                        entity.TextureSource = "Textures/tree1";
                        entity.Scale = new Vector2(1.5f, 1.5f);
                        entity.Initialize();
                        entity.SetCustomOrigin(new Vector2(-entity.Origin.X + 22f, entity.Origin.Y / 2));
                        entity.ScaleRect(new Vector2(.8f, .4f));
                        break;
                    case "Furnace":
                        entity.TextureSource = "Textures/furnace";
                        entity.Scale = new Vector2(2f, 2f);
                        entity.Initialize();
                        //entity.SetCustomOrigin(new Vector2(-entity.Origin.X / 10 - 20, entity.Origin.Y / 2 - 5));
                        //entity.ScaleRect(new Vector2(2f, .8f));
                        break;
                }
            }

            //== SHADERS ==//

            vignetteEffect = Globals.Content.Load<Effect>("Shaders/VignetteEffect");
            // Create a render target
            vignetteRenderTarget = new RenderTarget2D(game1.GraphicsDevice,
                game1.GraphicsDevice.PresentationParameters.BackBufferWidth, game1.GraphicsDevice.PresentationParameters.BackBufferHeight);

            vignetteEffect.Parameters["screenCenter"].SetValue(new Vector2(0.5f, 0.5f)); // Center of the screen
            vignetteEffect.Parameters["radius"].SetValue(.6f); // Radius of the vignette effect
            vignetteEffect.Parameters["intensity"].SetValue(0.25f); // Intensity of the vignette effect

            //crtEffectRenderTarget = new RenderTarget2D(game1.GraphicsDevice,
            //    game1.GraphicsDevice.PresentationParameters.BackBufferWidth, game1.GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public override void Update(GameTime gameTime, Game game1)
        {
            splashScreenGUI.ShowSplashScreen();
            mainMenuGUI.ShowMainMenuGUI();

            if (splashScreenGUI.isFinished)
            {
                mainMenuGUI.isHidden = false;
                UiSystem.Remove("SplashScreen_Paneli");
            }

            if (mainMenuGUI.newGameButtonPressed)
            {
                CleanUp();
                Globals.SceneManager.SwitchScene(Scenes.Gameplay, game1);
                //Globals.SceneManager.SwitchScene(Scenes.CinematicEntry, game1);
            }
            mainMenuGUI.InputChecks();

            // For tile selection
            Vector2 mousePosition = new(Globals.Mouse.X, Globals.Mouse.Y);
            Vector2 transformedMousePosition = Vector2.Transform(mousePosition,
                    Matrix.Invert(camera.GetViewMatrix(.5f, camera.Position)));
            transformedMousePosition.X /= 64;
            transformedMousePosition.Y /= 64;

            ParticleManager.UpdateEmitters();

            UiSystem.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, Game game1)
        {
            game1.GraphicsDevice.SetRenderTarget(vignetteRenderTarget);
            game1.GraphicsDevice.Clear(Color.Black);

            Globals.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap,
                transformMatrix: camera.GetViewMatrix(.5f, camera.Position));

            newWorldGenerator.DrawWorldTiles();

            foreach (var mine in newWorldGenerator.mineEntities) // I seperated mine entities from general entities list
            {
                mine.DrawEntity();
            }

            foreach (var entity in Globals.entities)
            {
                if (entity.hasAnimation)
                {
                    entity.DrawEntityAnimation();
                }
                else
                {
                    entity.DrawEntity();
                }
            }

            ParticleManager.Draw();
            Globals.SpriteBatch.End();

            UiSystem.Draw(gameTime, Globals.SpriteBatch);

            game1.GraphicsDevice.SetRenderTarget(null);

            //game1.GraphicsDevice.SetRenderTarget(crtEffectRenderTarget);
            UiSystem.Draw(gameTime, Globals.SpriteBatch);
            game1.GraphicsDevice.SetRenderTarget(null);

            Globals.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            vignetteEffect.CurrentTechnique.Passes[0].Apply();
            Globals.SpriteBatch.Draw(vignetteRenderTarget, Vector2.Zero, Color.White);
            Globals.SpriteBatch.End();

            //Globals.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //crtEffect.CurrentTechnique.Passes[0].Apply();
            //Globals.SpriteBatch.Draw(crtEffectRenderTarget, Vector2.Zero, Color.White);
            //Globals.SpriteBatch.End();

            {
                //Globals.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap,
                //transformMatrix: camera.GetViewMatrix(.5f, camera.Position));
                //foreach (var entity in Globals.entities)
                //{
                //    Globals.DrawRectangle(Globals.Content.Load<Texture2D>("Textures/ui"), entity.Rectangle, Color.Blue, 0.5f);
                //}
                //Globals.SpriteBatch.End();
            }
        }
        private void CleanUp()
        {
            UiSystem.Remove("SplashScreen_Paneli");
            UiSystem.Remove("MainMenu_Paneli");
            newWorldGenerator.drawingTiles.Clear();
            Globals.entities.Clear();
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
    }
}
