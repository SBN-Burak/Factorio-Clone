using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sandbox.Engine
{
    class Sprite
    {
        public readonly Texture2D _texture2D;
        public Rectangle _rectangle;
        public Vector2 _position;
        public Vector2 _scale;
        public Color _color;

        private Vector2 _origin;

        private bool isVisible = true;

        public Sprite(string source, Vector2 position, Color color, Vector2 scale)
        {
            _texture2D = Globals.Content.Load<Texture2D>(source);
            _position = position;
            _scale = scale;
            _color = color;

            _origin = new(_texture2D.Width / 2, _texture2D.Height / 2);

            _rectangle = new((int)_position.X - ((int)_origin.X * (int)_scale.X) - (int)(_texture2D.Width / 2 * _scale.X),
                             (int)_position.Y - ((int)_origin.Y * (int)_scale.Y) - (int)(_texture2D.Height / 2 * _scale.Y),
                             (int)(_texture2D.Width * _scale.X), (int)(_texture2D.Height * _scale.Y));
        }

        public void SetVisible(bool isVisible)
        {
            this.isVisible = isVisible;
        }

        public bool IsVisible()
        {
            return this.isVisible;
        }

        public void DrawSprite()
        {
            if(isVisible)
                Globals.SpriteBatch.Draw(_texture2D, _position, null, _color, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        public void DrawSpriteOriginApplied()
        {
            if(isVisible)
                Globals.SpriteBatch.Draw(_texture2D, _position, null, _color, 0f, _origin, _scale, SpriteEffects.None, 0f);
        }

        public void UpdateSpriteRectPos()
        {
            _rectangle = new((int)_position.X - ((int)_origin.X * (int)_scale.X),
                             (int)_position.Y - ((int)_origin.Y * (int)_scale.Y),
                        _texture2D.Width * (int)_scale.X, _texture2D.Height * (int)_scale.Y);
        }
    }
}
