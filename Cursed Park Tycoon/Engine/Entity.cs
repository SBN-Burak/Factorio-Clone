using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sandbox.Oyun;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sandbox.Engine
{
    public class Entity
    {
        public string Tag { get; set; }
        public Vector2 Position { get; set; }
        [JsonIgnore]
        public string TextureSource { get; set; }
        [JsonIgnore]
        public Rectangle Rectangle { get; set; }
        [JsonIgnore]
        public Vector2 Scale { get; set; }
        [JsonIgnore]
        public Color Color { get; set; }
        [JsonIgnore]
        public Vector2 Origin { get; set; }
        [JsonIgnore]
        public bool IsRemoved { get; set; } = false;
        [JsonIgnore]
        public bool IsHovered { get; set; } = false;
        [JsonIgnore]
        public bool IsCollidable { get; set; } = true;
        [JsonIgnore]
        public string Details { get; set; } = "";

        //====//

        public Texture2D _texture2D;

        public bool hasAnimation = false;
        public bool hasUIInterface = false;

        // For sorting - Y position inside this rectangle
        public Rectangle sortRectangle;

        public Entity() { } // Parameterless constructor needed for deserialization

        public Entity(string source, Vector2 position, Vector2 scale, Color color)
        {
            TextureSource = source;
            _texture2D = Globals.Content.Load<Texture2D>(TextureSource);
            Position = position;
            Scale = scale;
            Color = color;

            // Snap position to 64x64 grid
            int snappedX = (int)(Math.Floor(Position.X / 64.0f) * 64);
            int snappedY = (int)(Math.Floor(Position.Y / 64.0f) * 64);

            int snappedWidth = (int)(Math.Floor((_texture2D.Width * Scale.X) / 64.0) * 64);
            int snappedHeigth = (int)(Math.Floor((_texture2D.Height * Scale.Y) / 64.0) * 64);

            if(snappedWidth < 64 || snappedHeigth < 64)
            {
                snappedWidth = 64;
                snappedHeigth = 64;
            } 

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

            sortRectangle = Rectangle;
        }

        ////////// VIRTUAL FUNCTIONS ////////////
        public virtual void UpdateLogic() { }
        public virtual void OnCollisionEnter() { }
        public virtual void CleanUp() { } // Cleanup things when the entity is removed... like particles...
        public virtual void DrawEntityAnimation() { }

        // Furnace, Driller gibi şeyler için GUI interface. Changes inventory panels right section
        public virtual void ShowUIInterface() { }
        /////////////////////////////////////////

        public void SortIntersectingEntities() // Y-Sort for Entities intersecting sortRectangle
        {
            // Create a list to store the intersecting entities
            List<Entity> intersectingEntities = new();

            // Loop through all entities and find the ones that intersect with sortRectangle
            foreach (var entity in Globals.entities)
            {
                if (sortRectangle.Intersects(entity.Rectangle))
                {
                    intersectingEntities.Add(entity);
                }
            }

            // Sort the intersecting entities by their Y-position (Rectangle.Bottom)
            intersectingEntities.Sort((a, b) => a.Rectangle.Bottom.CompareTo(b.Rectangle.Bottom));

            // Replace the intersecting entities in the original list with the sorted ones
            int intersectingIndex = 0;
            for (int i = 0; i < Globals.entities.Count; i++)
            {
                if (sortRectangle.Intersects(Globals.entities[i].Rectangle))
                {
                    Globals.entities[i] = intersectingEntities[intersectingIndex];
                    intersectingIndex++;
                }
            }
        }

        public void DrawEntity() // No Animation
        {
            Globals.SpriteBatch.Draw(_texture2D, Position, null, Color, 0f, Origin, Scale, SpriteEffects.None, 0f);
        }

        public void SetCustomOrigin(Vector2 offset)
        {
            Origin += offset;
        }
        public void ScaleRect(Vector2 scale)
        {
            // Snap position to 64x64 grid
            int snappedX = (int)(Math.Floor(Position.X / 64.0) * 64);
            int snappedY = (int)(Math.Floor(Position.Y / 64.0) * 64);

            int snappedWidth = (int)(Math.Floor((_texture2D.Width * Scale.X) / 64.0) * 64);
            int snappedHeigth = (int)(Math.Floor((_texture2D.Height * Scale.Y) / 64.0) * 64);

            if (snappedWidth < 64 || snappedHeigth < 64)
            {
                snappedWidth = 64;
                snappedHeigth = 64;
            }

            // Create the aligned rectangle
            Rectangle = new Rectangle(
                    snappedX,
                    snappedY,
                    snappedWidth,
                    snappedHeigth
            );
        }

        public void UpdateRectPos(int snappedRectWidth, int snappedRectHeigth)
        {
            // Snap position to 64x64 grid
            int snappedX = (int)(Math.Floor(Position.X / 64.0) * 64);
            int snappedY = (int)(Math.Floor(Position.Y / 64.0) * 64);

            if (snappedRectWidth < 64 || snappedRectHeigth < 64)
            {
                snappedRectWidth = 64;
                snappedRectHeigth = 64;
            }

            Rectangle = new Rectangle(
                    snappedX,
                    snappedY,
                    snappedRectWidth,
                    snappedRectHeigth
            );
        }

        public void CheckIfSelected(Point mouse)
        {
            if (Rectangle.Contains(mouse))
            {
                IsHovered = true;
                Color = Color.DarkGray;
            }
            else
            {
                IsHovered = false;
                Color = Color.White;
            }
        }

        public void Initialize()
        {
            _texture2D = Globals.Content.Load<Texture2D>(TextureSource);

            // Snap position to 64x64 grid
            int snappedX = (int)(Math.Floor(Position.X / 64.0) * 64);
            int snappedY = (int)(Math.Floor(Position.Y / 64.0) * 64);

            int snappedWidth = (int)(Math.Floor((_texture2D.Width * Scale.X) / 64.0) * 64);
            int snappedHeigth = (int)(Math.Floor((_texture2D.Height * Scale.Y) / 64.0) * 64);

            if (snappedWidth < 64 || snappedHeigth < 64)
            {
                snappedWidth = 64;
                snappedHeigth = 64;
            }

            // Snap origin to 64x64 grid
            int snappedOriginX = (int)(Math.Floor(Origin.X / 64.0f) * 64);
            int snappedOriginY = (int)(Math.Floor(Origin.Y / 64.0f) * 64);

            Origin = new Vector2(snappedOriginX, snappedOriginY);

            Rectangle = new Rectangle(snappedX,
                                      snappedY,
                                      snappedWidth,
                                      snappedHeigth);
        }

        public void Initialize(int frameX, int frameY)
        {
            _texture2D = Globals.Content.Load<Texture2D>(TextureSource);

            // Snap position to 64x64 grid
            int snappedX = (int)(Math.Floor(Position.X / 64.0) * 64);
            int snappedY = (int)(Math.Floor(Position.Y / 64.0) * 64);

            int snappedWidth = (int)(Math.Floor(((_texture2D.Width / frameX) * Scale.X) / 64.0) * 64);
            int snappedHeigth = (int)(Math.Floor(((_texture2D.Height / frameY) * Scale.Y) / 64.0) * 64);

            if (snappedWidth < 64 || snappedHeigth < 64)
            {
                snappedWidth = 64;
                snappedHeigth = 64;
            }

            // Snap origin to 64x64 grid
            int snappedOriginX = (int)(Math.Floor(Origin.X / 64.0f) * 64);
            int snappedOriginY = (int)(Math.Floor(Origin.Y / 64.0f) * 64);

            Origin = new Vector2(snappedOriginX, snappedOriginY);

            //Origin = new Vector2((_texture2D.Width / frameX) / 2, (_texture2D.Height / frameY) / 2);

            Rectangle = new Rectangle(snappedX,
                                      snappedY,
                                      snappedWidth,
                                      snappedHeigth);
        }

        public void InitializeWithMultiply(int frameX, int frameY, int multiplyRectWidth, int multiplyRectHeigth)
        {
            _texture2D = Globals.Content.Load<Texture2D>(TextureSource);

            // Snap position to 64x64 grid
            int snappedX = (int)(Math.Floor(Position.X / 64.0) * 64);
            int snappedY = (int)(Math.Floor(Position.Y / 64.0) * 64);

            int snappedWidth = (int)(Math.Floor(((_texture2D.Width / frameX) * Scale.X) / 64.0) * 64);
            int snappedHeigth = (int)(Math.Floor(((_texture2D.Height / frameY) * Scale.Y) / 64.0) * 64);

            // Snap origin to 64x64 grid
            int snappedOriginX = (int)(Math.Floor(Origin.X / 64.0f) * 64);
            int snappedOriginY = (int)(Math.Floor(Origin.Y / 64.0f) * 64);

            Origin = new Vector2(snappedOriginX, snappedOriginY);

            //Origin = new Vector2((_texture2D.Width / frameX) / 2, (_texture2D.Height / frameY) / 2);

            Rectangle = new Rectangle(snappedX,
                                      snappedY,
                                      snappedWidth  * multiplyRectWidth,
                                      snappedHeigth * multiplyRectHeigth);
        }
    }
}
