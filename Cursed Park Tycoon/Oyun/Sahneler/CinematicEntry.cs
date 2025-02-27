using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Font;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Style;
using Sandbox.Engine;
using Sandbox.Engine.Scene;
using Sandbox.Oyun.GUI;

namespace Sandbox.Oyun.Sahneler
{
    public class CinematicEntry : Scene
    {
        UiSystem UiSystem;
        StartingCinematicGUI startingCinematicGUI;

        
        public override void Initialize()
        {
            startingCinematicGUI = new(5);
        }

        public override void LoadContent(Game game1)
        {
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

            UiSystem.Add("StartingCinematic_Paneli", startingCinematicGUI.GetElementGUI());
            startingCinematicGUI.PlayCinematic();
        }

        public override void Update(GameTime gameTime, Game game1)
        {
            if (InputManager.KeyPressed(Keys.P))
            {
                Globals.SceneManager.SwitchScene(Scenes.CinematicEntry, game1);
            }
            if (InputManager.KeyPressed(Keys.O))
            {
                Globals.SceneManager.SwitchScene(Scenes.MainMenu, game1);
            }
            if (InputManager.KeyPressed(Keys.K))
            {
                Globals.SceneManager.SwitchScene(Scenes.Gameplay, game1);
            }

            UiSystem.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, Game game1)
        {
            UiSystem.Draw(gameTime, Globals.SpriteBatch);
        }
    }
}
