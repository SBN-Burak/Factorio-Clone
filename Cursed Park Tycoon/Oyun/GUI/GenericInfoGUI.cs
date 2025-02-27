using MLEM.Ui.Elements;
using MLEM.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Textures;
using Sandbox.Engine;

namespace Sandbox.Oyun.GUI
{
    public class GenericInfoGUI
    {
        readonly Panel professorPanel;

        Image displayImage;

        //AnimationManager animationManager;
        float animationTick = 0;

        public GenericInfoGUI()
        {
            //professorAnim = new(Globals.Content.Load<Texture2D>("Textures/professor"), 4, 0, 1, 0.1f);
            //animationManager.AddAnimation(0, new(Globals.Content.Load<Texture2D>("Textures/professor"), 4, 1, 1, 0.1f));

            professorPanel = new Panel(Anchor.TopLeft, new Vector2(200, 240), Vector2.Zero, setHeightBasedOnChildren: false);

            displayImage = new Image(Anchor.TopCenter, new Vector2(1f, 1f),
                new TextureRegion(Globals.Content.Load<Texture2D>("Textures/logo"), new Rectangle(Point.Zero, 
                    new Point(85, 91))), scaleToImage: false)
            {
                PositionOffset = new Vector2(0, 0),
            };
            professorPanel.AddChild(displayImage);
        }

        public void UpdateAnimation()
        {
            //animationManager.Update(0);
        }

        public Element GetElementGUI()
        {
            return professorPanel;
        }
    }
}
