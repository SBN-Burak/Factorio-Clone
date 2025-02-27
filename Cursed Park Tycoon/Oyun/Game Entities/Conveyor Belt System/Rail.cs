using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sandbox.Engine;
using System;
using System.Collections.Generic;

namespace Sandbox.Oyun
{
    public class Rail : Entity
    {
        public int railType;

        private readonly List<Rectangle> _frameBorderRectangles = new();
        readonly int frameWidth;
        readonly int frameHeight;

        //== To avoid checking every rail's direction even it is the same... ==//
        //== Saving CPU time to avoid unneccesary collision checks.. ==//
        public bool hasNewDirection = false;

        //== We manipulate this int variable from burnerDriller to activate the rails 'hasNewDirection=true'  ==//
        //== Because driller will spawn mines and it needs to move and for that we need to activate this bool ==//
        public int neighbourDrillerCount = 0;

        public Color renk;

        public enum RailType // Rail Direction
        {
            YUKARI,
            SAG,
            SOL,
            ASAGI,

            SAG_ALT_YUKARI,
            SAG_ALT_ASAGI,

            SAG_UST_YUKARI,
            SAG_UST_ASAGI,

            SOL_ALT_ASAGI,
            SOL_ALT_YUKARI,

            SOL_UST_ASAGI,
            SOL_UST_YUKARI

        }
        public Rail(string source, Vector2 position, Vector2 scale, Color color, RailType railType) 
            : base(source, position, scale, color)
        {

            this.railType = (int)railType;
            this.renk = color;

            Tag = "Rail";

            // 12 tane rail old. için 12'ye böldüm.
            frameWidth = (_texture2D.Width) / 12;
            frameHeight = _texture2D.Height;

            // Her frame için frame'in. Horizontal bir sprite atlas olmalı... Y'si 0 çünkü..
            for (int i = 0; i < 12; i++)
                _frameBorderRectangles.Add(new(i * frameWidth, 0, frameWidth, frameHeight));

            // Snap position to 64x64 grid
            int snappedX = (int)(Math.Floor(Position.X / 64.0) * 64);
            int snappedY = (int)(Math.Floor(Position.Y / 64.0) * 64);

            int snappedWidth = (int)(Math.Floor((_texture2D.Width / 12 * Scale.X) / 64.0) * 64);
            int snappedHeigth = (int)(Math.Floor((_texture2D.Height * Scale.Y) / 64.0) * 64);

            // Snap origin to 64x64 grid
            int snappedOriginX = (int)(Math.Floor(Origin.X / 64.0f) * 64);
            int snappedOriginY = (int)(Math.Floor(Origin.Y / 64.0f) * 64);

            Origin = new Vector2(snappedOriginX, snappedOriginY);

            // Create the aligned rectangle
            Rectangle = new Rectangle(
                    snappedX,
                    snappedY,
                    snappedWidth,
                    snappedHeigth
            );
        }

        public void CheckIfSelectedRail(Point mouse)
        {
            if (this.Rectangle.Contains(mouse))
            {
                IsHovered = true;
                Color = Color.Pink;
                if (Globals.Mouse.RightButton == ButtonState.Pressed)
                    IsRemoved = true;
            }
            else
            {
                IsHovered = false;
                Color = Color.White;
            }
        }

        public void DrawRail()
        {
            Globals.SpriteBatch.Draw(_texture2D, (Position + (Origin * Scale)),
                        _frameBorderRectangles[railType], renk, 0f, Origin,
                        Scale, SpriteEffects.None, 0f);
        }

        public int GetRailType()
        {
            return railType;
        }
    }
}
