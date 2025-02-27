using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sandbox.Engine
{
    class Text
    {
        private readonly SpriteFont _spriteFont;
        public Vector2 _position;

        private string _text;
        private Color _color;

        public Text(string fontName, string text, Vector2 position, Color color)
        {
            _text = text;
            _position = position *
                            new Vector2(Globals.GraphicsDeviceManager.PreferredBackBufferWidth,
                                        Globals.GraphicsDeviceManager.PreferredBackBufferHeight);
            _color = color;

            _spriteFont = Globals.Content.Load<SpriteFont>(fontName);
        }

        public void UpdateText(string text)
        {
            _text = text;
        }

        public void DrawText()
        {
            Globals.SpriteBatch.DrawString(_spriteFont, _text, _position, _color);
        }
    }
}
