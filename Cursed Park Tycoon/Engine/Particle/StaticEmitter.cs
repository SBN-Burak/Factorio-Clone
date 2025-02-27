using Microsoft.Xna.Framework;

namespace Sandbox.Engine.Particle
{
    public class StaticEmitter
    {
        public Vector2 EmitPosition { get; }

        public StaticEmitter(Vector2 pos)
        {
            this.EmitPosition = pos;
        }
    }
}
