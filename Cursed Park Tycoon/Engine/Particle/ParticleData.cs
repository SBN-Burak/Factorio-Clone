using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sandbox.Engine.Particle
{
    public struct ParticleData
    {
        private static Texture2D _defaultTexture;
        public Texture2D texture = _defaultTexture ??= Globals.Content.Load<Texture2D>("Textures/particle_low_res");
        public float lifespan = 4f;
        public Color colorStart = Color.White;
        public Color colorEnd = Color.White;
        public float opacityStart = .4f;
        public float opacityEnd = 0f;
        public float sizeStart = 0f; //150
        public float sizeEnd = 0f; //350
        public float speed = 40f;
        public float angle = 45f;

        public ParticleData()
        {
        }
    }
}
