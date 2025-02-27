using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sandbox.Engine
{
    public class Camera
    {
        public Vector2 Position { get; set; }
        public Viewport Viewport { get; set; }
        public float Zoom { get; set; } = 1.0f;

        public Camera(Viewport viewport)
        {
            Viewport = viewport;
        }

        public Matrix GetViewMatrix(float zoomAmount, Vector2 playerPos)
        {
            var screenWidth = Viewport.Width;
            var screenHeight = Viewport.Height;
            var screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);

            // Calculate the translation based on the zoom level and player position
            var dx = (int)(screenCenter.X - (playerPos.X * zoomAmount));
            var dy = (int)(screenCenter.Y - (playerPos.Y * zoomAmount));


            // Create the translation matrix
            Matrix translationMatrix = Matrix.CreateTranslation(dx, dy, 0);

            var zoomScale = Matrix.CreateScale(zoomAmount);

            return zoomScale * translationMatrix;
        }

        public Rectangle GetVisibleArea(float zoomAmount, Vector2 playerPos, Vector2 offsetCullingSpace)
        {
            var inverseViewMatrix = Matrix.Invert(GetViewMatrix(zoomAmount, playerPos));
            var tl = Vector2.Transform(Vector2.Zero + new Vector2(-offsetCullingSpace.X, -offsetCullingSpace.Y), inverseViewMatrix);
            var tr = Vector2.Transform(new Vector2(Viewport.Width, 0) + new Vector2(offsetCullingSpace.X, -offsetCullingSpace.Y), inverseViewMatrix);
            var bl = Vector2.Transform(new Vector2(0, Viewport.Height) + new Vector2(-offsetCullingSpace.X, offsetCullingSpace.Y), inverseViewMatrix);
            var br = Vector2.Transform(new Vector2(Viewport.Width, Viewport.Height) + new Vector2(offsetCullingSpace.X, offsetCullingSpace.Y), inverseViewMatrix);

            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y)))
            );
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y)))
            );

            Rectangle rectangle = new(min.ToPoint(), (max - min).ToPoint());
            rectangle.Inflate(100, 200);

            return rectangle;
        }
    }

}
