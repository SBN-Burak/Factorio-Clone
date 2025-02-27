
using Microsoft.Xna.Framework;

namespace Sandbox.Engine.Scene
{
    public abstract class Scene
    {
        public abstract void Initialize();
        public abstract void LoadContent(Game game1);
        public abstract void Update(GameTime gameTime, Game game1);
        public abstract void Draw(GameTime gameTime, Game game1);
    }
}
