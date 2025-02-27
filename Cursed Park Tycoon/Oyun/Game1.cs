using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sandbox.Engine;
using Sandbox.Engine.Scene;

namespace Sandbox.Oyun
{
    public class Game1 : Game
    {
        private Texture2D _cursorTexture;
        private Vector2 _cursorPosition;

        public Game1()
        {
            Globals.GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            Globals.Content = Content;
            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.Viewport = GraphicsDevice.Viewport;
            Globals.SceneManager = new SceneManager();

            //===================//

            WindowSettings.IsFullScreen(false);
            Window.Title = "Cursed Park Tycoon";
            Window.AllowUserResizing = false;

            Globals.SceneManager.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _cursorTexture = Content.Load<Texture2D>("Textures/İkonlar/cursor");

            Globals.SceneManager.LoadContent(this);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.Update(gameTime);
            Globals.Mouse = Mouse.GetState();
            _cursorPosition = new(Globals.Mouse.X, Globals.Mouse.Y);
            InputManager.Update();
            Globals.Viewport = GraphicsDevice.Viewport;
            Globals.SceneManager.Update(gameTime, this);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Globals.SceneManager.Draw(gameTime, this);

            Globals.SpriteBatch.Begin();
            Globals.SpriteBatch.Draw(_cursorTexture, _cursorPosition, Color.White);
            Globals.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
