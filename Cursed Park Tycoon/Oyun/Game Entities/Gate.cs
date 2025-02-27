using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sandbox.Engine;

namespace Sandbox.Oyun.Game_Entities
{
    public class Gate : Entity
    {
        private Texture2D backgroundEffectTexture;

        public Gate(string source, Vector2 position, Vector2 scale, Color color) : base(source, position, scale, color)
        {
            Tag = "Gate";
            Details = "Gate";

            backgroundEffectTexture = Globals.Content.Load<Texture2D>("Textures/white");

            Rectangle = new Rectangle(Rectangle.X + 128,
                                      Rectangle.Y + 128*2,
                                      Rectangle.Width/2 * 2/3,
                                      Rectangle.Height/3 / 2);

            Globals.entities.Add(this);
        }

        public void DrawBackgroundEffect()
        {
            Globals.SpriteBatch.Draw(backgroundEffectTexture, Position + new Vector2(140, 85), null, new Color(Color.LawnGreen, 0.75f), 0f,
                    Origin, new Vector2(Scale.X*20, Scale.Y*35), SpriteEffects.None, 0f);
        }

    }
}
