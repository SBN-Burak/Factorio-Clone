using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using Sandbox.Engine;

namespace Sandbox.Oyun.GUI
{
    public class StartingCinematicGUI
    {
        Panel mainPanel;
        Image cinematicImage;

        public bool isFinished = false;
        private float _duration;

        public StartingCinematicGUI(float duration)
        {
            this._duration = duration;

            mainPanel = new Panel(Anchor.Center, new Vector2(1, 1), Vector2.Zero)
            {
                DrawColor = Color.Black,
            };
            cinematicImage = new Image(Anchor.Center, new Vector2(1, 1),
                new TextureRegion(Globals.Content.Load<Texture2D>("Textures/Cinematic/watching_news")), scaleToImage: true)
            {
                ImageScale = new Vector2(1.9f, 1.9f)
            };

            mainPanel.AddChild(cinematicImage);
        }

        public void PlayCinematic()
        {
            if (!isFinished)
            {
                _duration -= Globals.Time;

                if (_duration <= 0)
                {
                    isFinished = true;
                    mainPanel.IsHidden = true;
                }
            }
        }

        public Element GetElementGUI()
        {
            return mainPanel;
        }
    }
}
