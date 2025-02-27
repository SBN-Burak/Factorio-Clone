
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Sandbox.Engine
{
    class AnimationManager
    {
        public readonly Dictionary<object, Animation> _anims = new();

        public object _lastKey;
        public Object _key;

        public void AddAnimation(Object key, Animation animation)
        {
            _anims.Add(key, animation);
            _lastKey ??= key;
        }

        public void UpdateAnimation(Object key)
        {
            this._key = key;

            if (_anims.TryGetValue(this._key, out Animation value))
            {
                value.Start();
                _anims[this._key].UpdateAnimation();
                _lastKey = this._key;
            }
            else
            {
                _anims[this._key].Stop();
                _anims[this._key].Reset();
            }
        }

        public void UpdateAnimationWithOffset(Object key, int offset) // Give offset to skip certain range of frameindex and then loop..
        {
            this._key = key;

            if (_anims.TryGetValue(this._key, out Animation value))
            {
                value.Start();
                _anims[this._key].UpdateAnimationOffset(offset);
                _lastKey = this._key;
            }
            else
            {
                _anims[this._key].Stop();
                _anims[this._key].Reset();
            }
        }

        public void SetAnimationIndex(Object key, int index)
        {
            _anims[key].SetIndex(index);
        }

        public void StopAnimation()
        {
            _anims[this._key].Stop();
            _anims[this._key].Reset();
        }

        public void Draw(Vector2 pos, Vector2 scale, Color color)
        {
            _anims[_lastKey].DrawAnimation(pos, scale, color);
        }
    }
}
