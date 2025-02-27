using MLEM.Ui.Elements;
using MLEM.Ui;
using Microsoft.Xna.Framework;
using MLEM.Textures;
using Microsoft.Xna.Framework.Graphics;
using Sandbox.Engine;
using MLEM.Misc;
using MLEM.Maths;

namespace Sandbox.Oyun.GUI
{
    public class SplashScreen
    {
        readonly Panel mainPanel;
        readonly Image logo;

        private ProgressBar progressBar; // Loading assets
        private Paragraph progressText;

        public bool isFinished = false;

        private float _duration;
        private float _count = 0;

        public SplashScreen(float duration)
        {
            this._duration = duration;

            mainPanel = new Panel(Anchor.Center, new Vector2(1, 1), Vector2.Zero)
            {
                DrawColor = new Color(135,135,135)
            };
            logo = new Image(Anchor.Center, new Vector2(1, 1),
                new TextureRegion(Globals.Content.Load<Texture2D>("Textures/logo_game")), scaleToImage: true) //factorioAILogoSand
            {
                ImageScale = new Vector2(2.5f,2.5f),
                PositionOffset = new Vector2(0, -75)
            };
            progressText = new Paragraph(Anchor.BottomCenter, 1, "Loading assets..", true) // Fake
            {
                TextScale = 0.65f,
                TextColor = Color.White,
                PositionOffset = new Vector2(-248, 220),
            };
            progressBar = new ProgressBar(Anchor.BottomCenter, new Vector2(0.25f), Direction2.Right, 100, 1)
            {
                ProgressColor = Color.White,
                
                PositionOffset = new Vector2(0, 250),
                Size = new Vector2(650, 15)
            };

            mainPanel.AddChild(logo);
            mainPanel.AddChild(progressText);
            mainPanel.AddChild(progressBar);
        }

        public void ShowSplashScreen()
        {
            if(!isFinished)
            {
                _count += Globals.Time;

                if(progressBar.CurrentValue <= 40)
                {
                    AddProgressbarValue(_count); // Sahte loading -> Motivation
                }
                else
                {
                    AddProgressbarValue(_count / 20);
                    //AddProgressbarValue(_count);
                }

            }
        }

        private void AddProgressbarValue(float value)
        {
            progressBar.CurrentValue += value;

            if(progressBar.CurrentValue >= 100)
            {
                ///UiSystem.RootCallback.Remove(this.mainPanel);
                mainPanel.IsHidden = true;
                isFinished = true;
            }
        }

        public Element GetElementGUI()
        {
            return mainPanel;
        }
    }
}
