using Microsoft.Xna.Framework.Graphics;

namespace Sandbox.Engine
{
    class WindowSettings
    {
        public static void IsFullScreen(bool isFullScreen)
        {
            if(isFullScreen)
            {
                Globals.GraphicsDeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Globals.GraphicsDeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Globals.GraphicsDeviceManager.IsFullScreen = isFullScreen;
                Globals.GraphicsDeviceManager.ApplyChanges();
            }
            else
            {
                SetScreenSize(1450, 920);
            }
        }

        public static void SetScreenSize(int width, int height)
        {
            Globals.GraphicsDeviceManager.PreferredBackBufferWidth = width;
            Globals.GraphicsDeviceManager.PreferredBackBufferHeight = height;
            Globals.GraphicsDeviceManager.IsFullScreen = false;
            Globals.GraphicsDeviceManager.ApplyChanges();
        }

    }
}
