using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Maths;
using MLEM.Misc;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using Sandbox.Oyun.Inventory_System;
using System;
using System.Collections.Generic;

namespace Sandbox.Engine
{
    public class EntityInterface // Entity ingame statistic GUI
    {
        public Panel mainPanel;
        private Panel imagePanel;
        private Panel contentPanel; // Contains Crafting and Fuel elements.

        public Image entityImage;

        public Panel craftingPanel;
        public ItemSlot inputCraftingSlot = new(5, 0, 45);
        //public ItemSlot inputCraftingSlot = new(5, 0, 45, {"coal", "tree"});
        private Panel craftingProgressBarPanel;
        public ProgressBar craftingProgressBar;
        public ItemSlot outputCraftingSlot = new(330, 0, 45);

        public Panel fuelPanel;
        public ItemSlot fuelSlot = new(5, 0, 45);
        private Panel fuelProgressBarPanel;
        public ProgressBar fuelProgressBar;

        public EntityInterface(string panelTitle, string imageTextureSource) 
        {
            mainPanel = new Panel(Anchor.CenterRight, new Vector2(420, 590), Vector2.Zero, setHeightBasedOnChildren: false)
            {
                DrawColor = Color.DarkGray
            };
            mainPanel.AddChild(new Paragraph(Anchor.TopLeft, 1, panelTitle, true)
            {
                TextScale = .85f,
                PositionOffset = new Vector2(12, 3),
            });

            //====//

            imagePanel = new Panel(Anchor.TopCenter, new Vector2(390, 280))
            {
                PositionOffset = new Vector2(0, 40),
                DrawColor = new Color(140, 140, 140)
            };
            entityImage = new Image(Anchor.TopCenter, new Vector2(1f, 1f),
                new TextureRegion(Globals.Content.Load<Texture2D>(imageTextureSource)), scaleToImage: true)
            {
                PositionOffset = new Vector2(0, 40),
                ImageScale = new Vector2(2f, 2f)
            };
            imagePanel.AddChild(entityImage);

            //====//

            contentPanel = new Panel(Anchor.Center, new Vector2(390, 365))
            {
                PositionOffset = new Vector2(0, 100),
                DrawColor = new Color(150, 150, 150)
            };

            //====//

            craftingPanel = new Panel(Anchor.Center, new Vector2(390, 55))
            {
                PositionOffset = new Vector2(0, -50),
                DrawColor = new Color(140, 140, 140)
            };
            craftingPanel.AddChild(inputCraftingSlot.slotButton);
            craftingProgressBarPanel = new Panel(Anchor.Center, new Vector2(250, 25), false, false, false);
            craftingPanel.AddChild(craftingProgressBarPanel);
            craftingProgressBar = new ProgressBar(Anchor.Center, Vector2.One, Direction2.Right, 100, 60)
            {
                ProgressColor = new Color(40, 40, 40),
                Color = new Color(150, 150, 150)
            };
            craftingProgressBarPanel.AddChild(craftingProgressBar);
            craftingPanel.AddChild(outputCraftingSlot.slotButton);

            //====//

            fuelPanel = new Panel(Anchor.Center, new Vector2(390, 55))
            {
                PositionOffset = new Vector2(0, 100),
                DrawColor = new Color(140, 140, 140)
            };
            fuelPanel.AddChild(fuelSlot.slotButton);
            fuelProgressBarPanel = new Panel(Anchor.Center, new Vector2(250, 25), false, false, false);
            fuelPanel.AddChild(fuelProgressBarPanel);
            craftingProgressBar = new ProgressBar(Anchor.Center, Vector2.One, Direction2.Right, 100, 60)
            {
                ProgressColor = new Color(40, 40, 40),
                Color = new Color(150, 150, 150)
            };
            fuelProgressBarPanel.AddChild(craftingProgressBar);

            //====//

            contentPanel.AddChild(craftingPanel);
            contentPanel.AddChild(fuelPanel);

            mainPanel.AddChild(imagePanel);
            mainPanel.AddChild(contentPanel);
        }
    }
}
