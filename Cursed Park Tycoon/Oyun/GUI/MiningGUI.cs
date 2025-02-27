using Microsoft.Xna.Framework;
using MLEM.Maths;
using MLEM.Misc;
using MLEM.Ui;
using MLEM.Ui.Elements;

namespace Sandbox.Oyun.GUI
{
    public class MiningGUI
    {
        Panel mainPanel;

        public ProgressBar progressBar;

        public bool isHidden;

        public MiningGUI() 
        {
            mainPanel = new Panel(Anchor.BottomCenter, new Vector2(700, 50), false, false, false)
            {
                IsHidden = true,
            };

            progressBar = new ProgressBar(Anchor.Center, Vector2.One, Direction2.Right, 100, 0)
            {
                ProgressColor = new Color(40, 40, 40),
            };

            mainPanel.AddChild(progressBar);
        }
        public void Hide()
        {
            mainPanel.IsHidden = true;
        }
        public void Show()
        {
            mainPanel.IsHidden = false;
        }
        public void AddProgressbarValue(int value)
        {
            progressBar.CurrentValue = (progressBar.CurrentValue + value) % 100;
        }
        public Element GetElementGUI()
        {
            return mainPanel;
        }
    }
}
