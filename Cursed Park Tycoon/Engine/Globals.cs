using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sandbox.Engine.Scene;
using Sandbox.Oyun;
using System;
using System.Collections.Generic;

namespace Sandbox.Engine
{
    public static class Globals
    {
        public static float Time { get; set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public static MouseState Mouse { get; set; }
        public static KeyboardState Keyboard { get; set; }

        // For entity selection purposes
        public static Point MousePoint { get; set; }

        public static bool IsMouseEnteredGUIElements = false;
        public static Viewport Viewport { get; set; }
        public static SceneManager SceneManager { get; set; }

        public static Random Random = new();

        public static ToolTipInterface ToolTipInterface { get; set; }

        public static SoundEffect soundEffect;

        // *************** //

        public static List<Entity> entities = new();
        public static List<Rail> rails = new();

        public static void Update(GameTime time)
        {
            Time = (float)time.ElapsedGameTime.TotalSeconds;
        }

        //== DEBUGGING PURPOSES ==//
        public static void DrawRectangle(Texture2D texture, Rectangle rectangle, Color color, float alpha)
        {
            SpriteBatch.Draw(texture,
                new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height), 
                    new Color(color, alpha));
        }
    }
}
