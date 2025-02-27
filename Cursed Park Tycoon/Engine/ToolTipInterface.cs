using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MLEM.Ui;
using MLEM.Ui.Elements;

namespace Sandbox.Engine
{
    public class ToolTipInterface
    {
        public Panel mainPanel;

        public bool isItemGrabbed = false;
        public ToolTipInterface()
        {
            mainPanel = new Panel(new Anchor(), new Vector2(125, 60), false, false)
            {
                IsHidden = true,
            };
        }   
        
        public void UpdateLogic()
        {
            //== TODO: Change it relative to the viewport aspect ratio ==//
            Point mousePosition = Mouse.GetState().Position;
            mainPanel.PositionOffset = new Vector2(mousePosition.X + 10, mousePosition.Y + 10);
        }

        public Element GetElementGUI()
        {
            return mainPanel;
        }
    }
}
