
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sandbox.Engine
{
    class Animation
    {
        // Texture for Sprite Atlas
        private readonly Texture2D _texture;
        private readonly List<Rectangle> _frameBorderRectangles = new();
        private readonly int _totalFrames;
        private int _frameIndex;

        // For knowing how much time takes to change frames.
        private readonly float _frameTime;
        private float _frameTimeLeft;

        private bool _active = true;

        public Animation(Texture2D texture, int framesX, int framesY, int row, float frameTime)
        {
            _texture = texture;
            _frameTime = frameTime;
            _frameTimeLeft = _frameTime;

            int frameWidth = texture.Width / framesX;
            int frameHeight;

            if (framesY == 0)
            {
                _totalFrames = framesX;
                frameHeight = texture.Height;
            }
            else
            {
                _totalFrames = framesX * framesY;
                frameHeight = texture.Height / framesY;
            }

            // Her frame için frame'in
            for (int i = 0; i < _totalFrames; i++)
                _frameBorderRectangles.Add(new(i * frameWidth, (row - 1) * frameHeight, frameWidth, frameHeight));
        }

        public void Start()
        {
            _active = true;
        }
        public void Stop()
        {
            _active = false;
        }
        public void Reset()
        {
            _frameIndex = 0;
            _frameTimeLeft = _frameTime;
        }
        public void UpdateAnimation()
        {
            if (!_active) return;

            // _frameTime'ın değerini 0.1(100ms) yaptık constructor tanımlayınca.
            // bu _frameTimeLeft'nın olayı her frame'i eşit sürede geçmesi için.
            // Yani frame geçerken farklı sürelerde geçerse mesela bu UpdateAnimation()'a bağlı olursa
            // o zaman bi hızlı geçer bi yavaş geçer saçma sapan bir şey olur.
            _frameTimeLeft -= Globals.Time;

            if (_frameTimeLeft <= 0)
            {
                _frameTimeLeft += _frameTime;

                // Frame Loop yapıyor % operatörü sayesinde.
                _frameIndex = (_frameIndex + 1) % _totalFrames;
            }
        }

        public void UpdateAnimationOffset(int offset)
        {
            if (!_active) return;

            // _frameTime'ın değerini 0.1(100ms) yaptık constructor tanımlayınca.
            // bu _frameTimeLeft'nın olayı her frame'i eşit sürede geçmesi için.
            // Yani frame geçerken farklı sürelerde geçerse mesela bu UpdateAnimation()'a bağlı olursa
            // o zaman bi hızlı geçer bi yavaş geçer saçma sapan bir şey olur.
            _frameTimeLeft -= Globals.Time;

            if (_frameTimeLeft <= 0)
            {
                _frameTimeLeft += _frameTime;

                // Frame Loop yapıyor % operatörü sayesinde.

                _frameIndex = (_frameIndex + 1) % _totalFrames;

                if (_frameIndex == 0)
                {
                    _frameIndex = offset;
                }
            }
        }

        public void SetIndex(int index)
        {
            Stop();
            Reset();

            _frameIndex = index;
        }


        public void DrawAnimation(Vector2 pos, Vector2 scale, Color color)
        {
            Globals.SpriteBatch.Draw(_texture, pos, _frameBorderRectangles[_frameIndex], color, 0f, Vector2.Zero,
                    scale, SpriteEffects.None, 0f);
        }
    }
}
