using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sandbox.Engine.PathFinding;

namespace Sandbox.Engine
{
    enum TileType
    {
        Ice,
        Sand,
        Sand_Grassy,
        Stone
    }
    public class Tile
    {
        public Texture2D texture;
        public Color color;
        public int x;
        public int y;

        // SpriteSheet Frame Index
        public int frameIndex;

        // Collision rectangle
        public Rectangle rectangle;

        public bool isWall; // For A* pathfinding

        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    enum BitmaskDirections
    {
        YUKARI = 1,
        SOL = 2,
        SAG = 4,
        ALT = 8
    }

    class NewWorldGenerator
    {
        public List<Tile> drawingTiles = new();

        public Tile[,] tiles;
        public float width;
        public float height;

        public Texture2D _tileTexture;
        public Texture2D _waterTexture;

        private Vector2 _scale;
        public Color _color;

        readonly int frameWidth;
        readonly int frameHeight;
        readonly int pathFrameWidth;
        readonly int pathFrameHeight;

        // For spritesheet
        private readonly List<Rectangle> _frameBorderRectangles = new();
        private readonly List<Rectangle> _waterFrameBorderRectangles = new();

        readonly OpenSimplexNoise oSimplexNoise = new();

        //== SPATIAL PARTIONING RECTANGELS ==//
        //private readonly List<Rectangle> _spatialPartRectangels = new();

        //== STONE, COAL, IRON <-- I first render these for sorting 2d depth ==//
        public List<Entity> mineEntities = new();

        public NewWorldGenerator(string source, string pathTileSource, int width, int height, Vector2 tileScale, Color color,
                int perlinNoiseResolution)
        {
            _tileTexture = Globals.Content.Load<Texture2D>(source);
            _waterTexture = Globals.Content.Load<Texture2D>(pathTileSource);
            _scale = tileScale;
            _color = color;

            // 3 tane tile old için 3'e böldüm
            frameWidth = (_tileTexture.Width) / 3;
            frameHeight = _tileTexture.Height;

            // 16 tane tile old için 16'ya böldüm
            pathFrameWidth = (_waterTexture.Width) / 16;
            pathFrameHeight = _waterTexture.Height;

            // Her frame için frame'in. Horizontal bir sprite atlas olmalı... Y'si 0 çünkü..
            for (int i = 0; i < 3; i++)
                _frameBorderRectangles.Add(new(i * (frameWidth), 0, frameWidth, frameHeight));

            for (int i = 0; i < 16; i++)
                _waterFrameBorderRectangles.Add(new(i * (pathFrameWidth), 0, pathFrameWidth, pathFrameHeight));


            InitWorldTiles(width, height, perlinNoiseResolution);
            MakeWaterGreatAgain(width, height);
        }

        private void InitWorldTiles(int width, int height, int perlinNoiseResolution)
        {
            this.width = width;
            this.height = height;
            tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = new Tile((int)(x * (_tileTexture.Width / 3) * _scale.X),
                            (int)(y * _tileTexture.Height * _scale.Y));
                    tiles[x, y].texture = _tileTexture;

                    // Generate noise with a specific resolution factor
                    double fNoise = oSimplexNoise.Evaluate((double)x / perlinNoiseResolution, (double)y / perlinNoiseResolution);
                    // Map the noise value from [-1, 1] to [0, 3]
                    fNoise = (fNoise + 1) / 2 * 3; // Scale to [0, 3]
                    
                    tiles[x, y].frameIndex = (int)Math.Floor(fNoise);
                    tiles[x, y].color = Color.MediumPurple; //Color.LawnGreen // MistyRose güzel gibi default olarak?

                    

                    drawingTiles.Add(tiles[x, y]);
                }
            }

            //for (int x = 0; x < 5; x++)
            //{
            //    for (int y = 0; y < 5; y++)
            //    {
            //        // 2560*2x2560*2 sized Rectanges total now: (world is 400x400(world size) )
            //        _spatialPartRectangels.Add(new Rectangle(x * 2560*2, y * 2560*2, 2560*2, 2560*2));
            //    }
            //}
        }
        public int GetTileFrameIndex(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                return tiles[x, y].frameIndex;
            }

            return 0;
        }

        public Texture2D GetTileTexture(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                return tiles[x, y].texture;
            }

            return null;
        }

        public void GetPathTileAt(int x, int y) // WATER
        {
            if ((x >= 0 && x < width && y >= 0 && y < height))
            {
                tiles[x, y].texture = _waterTexture;
                tiles[x, y].frameIndex = GetBitMaskValue(x, y);

                UpdateNeighbour(x, y - 1);
                UpdateNeighbour(x + 1, y);
                UpdateNeighbour(x, y + 1);
                UpdateNeighbour(x - 1, y);
            }
        }

        public int GetBitMaskValue(int x, int y)
        {
            int bitmask = 0;
            if (IsTileWater(x, y - 1)) bitmask |= (int)BitmaskDirections.YUKARI;
            if (IsTileWater(x + 1, y)) bitmask |= (int)BitmaskDirections.SAG;
            if (IsTileWater(x, y + 1)) bitmask |= (int)BitmaskDirections.ALT;
            if (IsTileWater(x - 1, y)) bitmask |= (int)BitmaskDirections.SOL;
            return bitmask;
        }

        public bool IsTileWater(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
                return tiles[x, y].texture == _waterTexture;

            return false;
        }

        private void UpdateNeighbour(int x, int y)
        {
            if ((x >= 0 && x < width && y >= 0 && y < height) && 
                    tiles[x, y].texture == _waterTexture)
            {
                tiles[x, y].frameIndex = GetBitMaskValue(x, y);
            }
        }

        public void DrawWorldTiles()
        {
            foreach (var tile in drawingTiles)
            {
                if (tile.texture == _tileTexture) // Ground tile
                {
                    Globals.SpriteBatch.Draw(tile.texture, new Vector2(tile.x, tile.y),
                        _frameBorderRectangles[tile.frameIndex], tile.color, 0f, Vector2.Zero,
                        _scale, SpriteEffects.None, 0f);
                }
                else if (tile.texture == _waterTexture) // Water tile
                {
                    Globals.SpriteBatch.Draw(tile.texture, new Vector2(tile.x, tile.y),
                        _waterFrameBorderRectangles[tile.frameIndex], tile.color, 0f, Vector2.Zero,
                        _scale, SpriteEffects.None, 0f);
                }
            }
        }

        public void DrawTile(Tile tile)
        {
            if (tile.texture == _tileTexture) // Ground tile
            {
                Globals.SpriteBatch.Draw(tile.texture, new Vector2(tile.x, tile.y),
                    _frameBorderRectangles[tile.frameIndex], tile.color, 0f, Vector2.Zero,
                    _scale, SpriteEffects.None, 0f);
            }
            else if (tile.texture == _waterTexture) // Water tile
            {
                Globals.SpriteBatch.Draw(tile.texture, new Vector2(tile.x, tile.y),
                    _waterFrameBorderRectangles[tile.frameIndex], tile.color, 0f, Vector2.Zero,
                    _scale, SpriteEffects.None, 0f);
            }
        }

        public void DrawWaterTile(Tile tile) // For applying shader. That's why I sepperated it.
        {
            if (tile.texture == _waterTexture) // Water tile
            {
                Globals.SpriteBatch.Draw(tile.texture, new Vector2(tile.x, tile.y),
                    _waterFrameBorderRectangles[tile.frameIndex], tile.color, 0f, Vector2.Zero,
                    _scale, SpriteEffects.None, 0f);
            }
        }

        //public void DrawSpatialPartRectangles()
        //{
        //    foreach(var rect in _spatialPartRectangels)
        //    {
        //        Globals.DrawRectangle(Globals.Content.Load<Texture2D>("Textures/ui"), rect, Color.Blue, 0.75f);
        //    }
        //}

        public void SpawnIron()
        {
            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    double fNoise = oSimplexNoise.Evaluate((double)x / 5, (double)y / 5);

                    // Map the noise value from [-1, 1] to [0, 1]
                    fNoise = (fNoise + 1) / 2; // Scale to [0, 1] // Yani var ya da yok gibi

                    float ironThreshold = 0.85f;

                    if (fNoise > ironThreshold && (tiles[x, y].texture != _waterTexture)) // Suyun üstüne spawn etmiyoruz yani
                    {
                        Entity iron = new("Textures/iron", new Vector2(x * 64, y * 64), new Vector2(1.5f, 1.5f), Color.White)
                        {
                            IsCollidable = false,
                            //iron.SetCustomOrigin(new Vector2(-iron.Origin.X, iron.Origin.Y - 42));
                            Tag = "Iron",
                            Details = "Iron",
                        };

                        mineEntities.Add(iron);
                    }
                }
            }
        }
        public void SpawnStone()
        {
            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    double fNoise = oSimplexNoise.Evaluate((double)x / 10, (double)y / 10);

                    // Map the noise value from [-1, 1] to [0, 1]
                    fNoise = (fNoise + 1) / 2; // Scale to [0, 1] // Yani var ya da yok gibi

                    float stoneThreshold = 0.83f;

                    if (fNoise > stoneThreshold && 
                        (tiles[x, y].texture != _waterTexture) && 
                        !IsPositionOccupied(new Vector2(x * 64, y * 64), mineEntities))
                    {
                        Entity stone = new("Textures/stone", new Vector2(x * 64, y * 64), new Vector2(1.5f, 1.5f), Color.White);

                        stone.IsCollidable = false;
                        //stone.SetCustomOrigin(new Vector2(-stone.Origin.X, stone.Origin.Y - 42));
                        stone.Tag = "Stone";
                        stone.Details = "Stone";

                        mineEntities.Add(stone);
                    }
                }
            }
        }
        public void SpawnCoal()
        {
            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    double fNoise = oSimplexNoise.Evaluate((double)x / 4, (double)y / 4);

                    // Map the noise value from [-1, 1] to [0, 1]
                    fNoise = (fNoise + 1) / 2; // Scale to [0, 1] // Yani var ya da yok gibi

                    float coalThreshold = 0.86f;

                    if (fNoise > coalThreshold &&
                        (tiles[x, y].texture != _waterTexture) &&
                        !IsPositionOccupied(new Vector2(x * 64, y * 64), mineEntities))
                    {
                        Entity coal = new("Textures/coal", new Vector2(x * 64, y * 64), new Vector2(1.5f, 1.5f), Color.White);

                        coal.IsCollidable = false;
                        //coal.SetCustomOrigin(new Vector2(-coal.Origin.X, coal.Origin.Y - 52));
                        coal.Tag = "Coal";
                        coal.Details = "Coal";

                        mineEntities.Add(coal);
                    }
                }
            }
        }
        public void SpawnTree(ref List<Entity> entities)
        {
            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    double fNoise = oSimplexNoise.Evaluate((double)x / 6, (double)y / 6);

                    // Map the noise value from [-1, 1] to [0, 1]
                    fNoise = (fNoise + 1) / 2; // Scale to [0, 1] // Yani var ya da yok gibi

                    float coalThreshold = 0.8f;

                    if (fNoise > coalThreshold &&
                        (tiles[x, y].texture != _waterTexture) &&
                        !IsPositionOccupied(new Vector2(x * 64, y * 64), entities))
                    {
                        Entity kuru_agac = new("Textures/tree1", new Vector2(x * 64, y * 64), new Vector2(2f, 2f), Color.White);

                        //kuru_agac.SetCustomOrigin(new Vector2(-kuru_agac.Origin.X + 22f, kuru_agac.Origin.Y / 2));
                        //kuru_agac.ScaleRect(new Vector2(.8f, .4f));
                        kuru_agac.Tag = "Tree1";
                        kuru_agac.Details = "Tree1";

                        kuru_agac.Rectangle = new Rectangle(kuru_agac.Rectangle.X, kuru_agac.Rectangle.Y + 128, 64, 64);

                        entities.Add(kuru_agac);

                        //tiles[(int)kuru_agac.Position.X/64, (int)kuru_agac.Position.X / 64].isWall = true;
                    }
                }
            }
        }

        private void MakeWaterGreatAgain(int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (GetTileFrameIndex(x, y) == 0) // WATER
                    {
                        GetPathTileAt(x, y);
                        tiles[x, y].isWall = true;
                    }
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (tiles[x, y].frameIndex != 15 && tiles[x, y].texture == _waterTexture)
                    {
                        tiles[x, y].rectangle = new Rectangle(x * 64, y * 64, 64, 64);
                    }
                }
            }
        }

        private bool IsPositionOccupied(Vector2 position, List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.Position == position)
                {
                    return true;
                }
            }
            return false;
        }

        public void DrawPathFindingTiles(List<Location> calculatedPaths)
        {
            foreach (var calculatedPath in calculatedPaths)
            {
                tiles[calculatedPath.X, calculatedPath.Y].color = Color.Red;
            }
        }
    }
}
