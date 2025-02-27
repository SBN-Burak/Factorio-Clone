using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Sandbox.Engine.Particle
{
    public class ParticleEmitter
    {
        public List<Particle> _particles = new();

        private readonly ParticleEmitterData _data;
        private float _intervalLeft;
        private readonly IEmitter _emitter;
        private readonly StaticEmitter _staticEmitter;
        private Vector2 _particleGravity;

        private readonly Random rnd = new();

        private bool emitterEnabled = false;

        ////// Particle için bir nevi object pooling yapıyorum. Sürekli yeni particle eklemesin ve listeden isFinished'den
        ////// sonra silmesin diye. _totalParticles = 30 yaparsak bu emitter için hep aynı 30 partikülü kullanacak.
        ////// bitince tekrar en baştaki özelliklerine dönüp yine baştan başlıyacak. Sanki hep yenileniyormuş gibi.
        private int _totalParticles;
        private int particleAmount;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ParticleEmitter(IEmitter emitter, ParticleEmitterData data, Vector2 particleGravity, int totalParticles)
        {
            this._data = data;
            this._intervalLeft = data.interval;
            this._emitter = emitter;
            this._particleGravity = particleGravity;
            this._totalParticles = totalParticles;
        }

        public ParticleEmitter(StaticEmitter emitter, ParticleEmitterData data, Vector2 particleGravity, int totalParticles)
        {
            this._data = data;
            this._intervalLeft = data.interval;
            this._staticEmitter = emitter;
            this._particleGravity = particleGravity;
            this._totalParticles = totalParticles;
        }

        public void StopEmitter()
        {
            emitterEnabled = false;

            foreach(var particle in _particles)
            {
                particle.isActive = false;
            }
        }

        public void StartEmitter()
        {
            emitterEnabled = true;

            foreach (var particle in _particles)
            {
                particle.isActive = true;
            }
        }

        private void Emit(Vector2 pos)
        {

            ParticleData d = _data.particleData;
            d.lifespan = rnd.Next((int)_data.lifespanMin, (int)_data.lifespanMax);
            d.speed = rnd.Next((int)_data.speedMin, (int)_data.speedMax);
            d.angle = rnd.Next((int)(_data.angle - _data.angleVariance), (int)(_data.angle + _data.angleVariance));

            d.sizeStart = 0;
            //d.sizeEnd = 100;
            
            Particle p = new(pos, d, this._particleGravity);
            _particles.Add(p);
        }

        public void Update()
        {
            if (emitterEnabled)
            {
                _intervalLeft -= Globals.Time;
                while (_intervalLeft <= 0f)
                {
                    _intervalLeft += _data.interval;
                    var pos = _staticEmitter.EmitPosition;
                    for (int i = 0; i < _data.emitCount; i++)
                    {
                        if (particleAmount <= _totalParticles)
                        {
                            Emit(pos);
                            particleAmount++;
                        }
                    }
                }
            }

            // Update each particle
            foreach (var particle in _particles)
            {
                particle.Update();
            }
        }

        public void CleanupParticles()
        {
            _particles.Clear();
            particleAmount = 0;
        }
    }
}
