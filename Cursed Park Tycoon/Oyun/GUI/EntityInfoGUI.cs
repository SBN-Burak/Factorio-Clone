using MLEM.Ui.Elements;
using MLEM.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Textures;
using Sandbox.Engine;

namespace Sandbox.Oyun.GUI
{
    public class EntityInfoGUI
    {
        readonly Panel mainPanel;
        readonly Panel imagePanel;

        Image displayImage;
        Paragraph infoText;

        public Entity entityInstance;

        Texture2D uiTexture; // For showing the ui texture -> nothing basically blank image

        public EntityInfoGUI()
        {
            uiTexture = Globals.Content.Load<Texture2D>("Textures/ui");

            entityInstance = new("Textures/ui", Vector2.Zero, Vector2.One, Color.White)
            {
                Details = ""
            };

            mainPanel = new Panel(Anchor.TopRight, new Vector2(120, 160), Vector2.Zero, setHeightBasedOnChildren: false)
            {
                DrawColor = new Color(135, 135, 135)
            };
            imagePanel = new Panel(Anchor.TopCenter, new Vector2(100, 100), Vector2.Zero, setHeightBasedOnChildren: false)
            {
                DrawColor = new Color(135, 135, 135)
            };

            displayImage = new Image(Anchor.TopCenter, new Vector2(1f, 1f),
                new TextureRegion(entityInstance._texture2D), scaleToImage: false)
            {
                PositionOffset = new Vector2(0, 0)
            };
            imagePanel.AddChild(displayImage);
            ////////////////////

            infoText = new Paragraph(Anchor.TopCenter, 1, entityInstance.Details, true)
            {
                PositionOffset = new Vector2(0, 110),
                TextScale = .75f
            };

            mainPanel.AddChild(imagePanel);
            mainPanel.AddChild(infoText);
        }

        public Element GetElementGUI()
        {
            return mainPanel;
        }

        public void UpdateEntityInfoImage(Entity entity)
        {
            if (entityInstance == null)
            {
                displayImage.Texture = new TextureRegion(uiTexture);
                infoText.Text = "";
            }
            else if (entityInstance.IsHovered)
            {
                displayImage.Texture = new TextureRegion(entity._texture2D);
                infoText.Text = entity.Details;
            }
        }
    }
}
