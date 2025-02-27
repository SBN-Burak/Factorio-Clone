using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sandbox.Engine.Particle
{
    public class Particle
    {
        public ParticleData _data;
        public Vector2 _position;
        private float _lifespanLeft;
        private float _lifespanAmount;
        private Color _color;
        private float _opacity;
        private float _scale;
        private Vector2 _origin;
        private Vector2 _direction;
        private Vector2 _particleGravity;

        private Vector2 _first_position;  // For Object Pooling
        private Vector2 _first_direction; // For Object Pooling

        public bool isActive = true;

        public Particle(Vector2 pos, ParticleData data, Vector2 particleGravity)
        {
            _particleGravity = particleGravity;
            _data = data;
            _lifespanLeft = data.lifespan;
            _lifespanAmount = 1f;
            _position = pos;
            _color = data.colorStart;
            _opacity = data.opacityStart;
            _origin = new(_data.texture.Width / 2, _data.texture.Height / 2);
            _particleGravity = particleGravity;

            _first_position = pos; // For Object Pooling

            if (data.speed != 0)
            {
                _data.angle = MathHelper.ToRadians(_data.angle);
                _direction = new Vector2((float)Math.Sin(_data.angle), -(float)Math.Cos(_data.angle));
                _first_direction = new Vector2((float)Math.Sin(_data.angle), -(float)Math.Cos(_data.angle)); // For Object Pooling
            }
            else
            {
                _direction = Vector2.Zero;
                _first_direction = Vector2.Zero; // For Object Pooling
            }
        }

        public void Update()
        {
            _lifespanLeft -= Globals.Time;
            if (_lifespanLeft <= 0f)
            {
                if (isActive)
                {
                    ResetParticleState();
                    return;
                }
            }
            
            _lifespanAmount = MathHelper.Clamp(_lifespanLeft / _data.lifespan, 0, 1);
            _color = Color.Lerp(_data.colorEnd, _data.colorStart, _lifespanAmount);
            _opacity = MathHelper.Clamp(MathHelper.Lerp(_data.opacityEnd, _data.opacityStart, _lifespanAmount), 0, 1);
            _scale = MathHelper.Lerp(_data.sizeEnd, _data.sizeStart, _lifespanAmount) / _data.texture.Width;
            
            _direction += _particleGravity;
            _position += _direction * _data.speed * Globals.Time;
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_data.texture, _position, null, _color * _opacity, 0f, _origin, _scale, SpriteEffects.None, 1f);
        }

        public void ResetParticleState()
        {
            _lifespanLeft = _data.lifespan;
            _color = _data.colorStart;
            _opacity = _data.opacityStart;
            _scale = _data.sizeStart;


            _direction = _first_direction;
            _position = _first_position;
        }
    }
}
