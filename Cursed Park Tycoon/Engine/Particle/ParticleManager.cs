using System.Collections.Generic;

namespace Sandbox.Engine.Particle
{
    public static class ParticleManager
    {
        public static List<Particle> _particles = new();
        private static List<ParticleEmitter> _particleEmitters = new();

        public static void AddParticle(Particle p)
        {
            _particles.Add(p);
        }

        public static void AddParticleEmitter(ParticleEmitter e)
        {
            _particleEmitters.Add(e);
        }

        public static void RemoveParticleEmitter(ParticleEmitter e)
        {
            e.CleanupParticles();  // Clean up particles before removing the emitter
            _particleEmitters.Remove(e);
        }

        //public static void UpdateParticles()
        //{
        //    foreach (var particle in _particles)
        //    {
        //        particle.Update();
        //    }
        //
        //    //_particles.RemoveAll(p => p.isFinished);
        //}

        public static void UpdateEmitters()
        {
            foreach (var emitter in _particleEmitters)
            {
                emitter.Update();
            }
        }

        public static void Update()
        {
            //UpdateParticles();
            UpdateEmitters();
        }

        public static void Draw()
        {
            //foreach (var particle in _particles)
            //{
            //    particle.Draw();
            //}

            foreach (var emitter in _particleEmitters)
            {
                // Draw particles within each emitter
                foreach (var particle in emitter._particles)
                {
                    particle.Draw();
                }
            }
        }
    }
}
