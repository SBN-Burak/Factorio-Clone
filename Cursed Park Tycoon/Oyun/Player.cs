using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Sandbox.Engine;
using Sandbox.Engine.Particle;
using Sandbox.Oyun.GUI;

namespace Sandbox.Oyun
{
    public class Player : Entity
    {
        public Texture2D playerTexture;
        private Vector2 _direction = Vector2.Zero;
        private float _speed = 600;

        private readonly AnimationManager _anims = new();

        // For viewport culling. If we move then it will update the culling rather than checking it every frame.
        public bool isPlayerMoved = false;

        private bool isAnimationPlayed = true; // Idle olsun diye. Yoksa hareket frame'inde duruyordu

        // Particles //
        private Particle miningParticle;
        private ParticleData _particleData;

        // Textures For Mining //
        private Texture2D furnaceTexture; // Tekli olan

        public Player(string source, Vector2 position, Vector2 scale, Color color) : base(source, position, scale, color)
        {
            Tag = "Player";
            Details = "Player";

            hasAnimation = true;
            hasUIInterface = false;

            Position = position;
            Scale = scale;

            playerTexture = Globals.Content.Load<Texture2D>(source);
            _anims.AddAnimation(new Vector2(0, 1),   new(playerTexture, 8, 8, 1, .07f));
            _anims.AddAnimation(new Vector2(-1, 0),  new(playerTexture, 8, 8, 2, .07f));
            _anims.AddAnimation(new Vector2(1, 0),   new(playerTexture, 8, 8, 3, .07f));
            _anims.AddAnimation(new Vector2(0, -1),  new(playerTexture, 8, 8, 4, .07f));
            _anims.AddAnimation(new Vector2(-1, 1),  new(playerTexture, 8, 8, 5, .07f));
            _anims.AddAnimation(new Vector2(-1, -1), new(playerTexture, 8, 8, 6, .07f));
            _anims.AddAnimation(new Vector2(1, 1),   new(playerTexture, 8, 8, 7, .07f));
            _anims.AddAnimation(new Vector2(1, -1),  new(playerTexture, 8, 8, 8, .07f));

            // Neden 8'e böldüm? Çünkü sprite atlas kanzi kendine gel!!!
            Origin = new((playerTexture.Width / 2) / 8, (playerTexture.Height / 2) / 8);

            CalculateRectangle();

            _particleData = new()
            {
                sizeStart = 50,
                sizeEnd = 50,
                lifespan = 1.5f,
                speed = 0.25f,
                opacityStart = .75f,
                opacityEnd = .75f,
            };

            miningParticle = new(Vector2.Zero, _particleData, new Vector2(0.2f, -0.5f));

            /////////////

            furnaceTexture = Globals.Content.Load<Texture2D>("Textures/furnace");
        }

        public override void UpdateLogic()
        {
            _direction = Vector2.Zero;
            isPlayerMoved = false;

            if (InputManager.KeyDown(Keys.W))
            {
                _direction.Y--;
                SortIntersectingEntities();
            }
            if (InputManager.KeyDown(Keys.A)) _direction.X--;
            if (InputManager.KeyDown(Keys.S))
            {
                _direction.Y++;
                SortIntersectingEntities();
            }
            if (InputManager.KeyDown(Keys.D)) _direction.X++;

            if (InputManager.KeyDown(Keys.LeftShift)) _speed = 1000;
            else
            {
                _speed = 600;
            }

            Position += _direction * Globals.Time * _speed;

            if (_direction != Vector2.Zero)
            {
                _anims.UpdateAnimation(_direction);
                isPlayerMoved = true;
                isAnimationPlayed = false;
            }
            else if(!isAnimationPlayed)
            {
                _anims.StopAnimation();
                isAnimationPlayed = true;
            }

            CalculateRectangle();

            base.UpdateLogic();
        }

        public void Mining(ref List<Entity> entities, ref MiningGUI miningGUI)
        {
            //miningParticle.Update();

            foreach (var entity in entities)
            {
                if(entity.IsHovered)
                {
                    if(Globals.Mouse.RightButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements) //Globals.Mouse.RightButton == ButtonState.Pressed
                    {
                        //TODO: AddItem() after slider is full
                        //TODO: In a certain range make it work
                        //TODO: Diğerlerine (furnace gibi) direkt slider ile yıksın. Tabi daha hızlı olsun

                        switch (entity.Tag)
                        {
                            case "Iron":
                                miningGUI.Show();
                                miningGUI.AddProgressbarValue(2);
                                miningParticle._data.texture = entity._texture2D;

                                if (miningGUI.progressBar.CurrentValue == 0)
                                {
                                    miningParticle._position = entity.Position + new Vector2(0, -5);
                                    InventoryManager.AddItem("iron", 1);
                                }
                                break;

                            case "Stone":
                                miningGUI.Show();
                                miningGUI.AddProgressbarValue(2);
                                miningParticle._data.texture = entity._texture2D;

                                if (miningGUI.progressBar.CurrentValue == 0)
                                {
                                    miningParticle._position = entity.Position + new Vector2(0, -5);

                                    InventoryManager.AddItem("stone", 1);
                                }
                                break;

                            case "Coal":
                                miningGUI.Show();
                                miningGUI.AddProgressbarValue(2);
                                miningParticle._data.texture = entity._texture2D;

                                if (miningGUI.progressBar.CurrentValue == 0)
                                {
                                    miningParticle._position = entity.Position;
                                    InventoryManager.AddItem("coal", 1);
                                }
                                break;

                            case "Tree1":
                                miningGUI.Show();
                                miningGUI.AddProgressbarValue(2);
                                miningParticle._data.texture = entity._texture2D;

                                if (miningGUI.progressBar.CurrentValue == 0)
                                {
                                    miningParticle._position = entity.Position;
                                    InventoryManager.AddItem("tree1", 1);
                                    entity.CleanUp();
                                    entities.Remove(entity);
                                }
                                break;

                            case "Furnace":
                                miningGUI.Show();
                                miningGUI.AddProgressbarValue(5);
                                miningParticle._data.texture = furnaceTexture;

                                if (miningGUI.progressBar.CurrentValue == 0)
                                {
                                    miningParticle._position = entity.Position;
                                    InventoryManager.AddItem("furnace1", 1);
                                    entity.CleanUp();
                                    entities.Remove(entity);
                                }
                                break;
                            case "Burner Driller":
                                miningGUI.Show();
                                miningGUI.AddProgressbarValue(2);
                                miningParticle._data.texture = entity._texture2D;

                                if (miningGUI.progressBar.CurrentValue == 0)
                                {
                                    miningParticle._position = entity.Position;
                                    InventoryManager.AddItem("burnerDriller", 1);
                                    entity.CleanUp();
                                    entities.Remove(entity);
                                }
                                break;
                        }

                        break;
                    }
                    else if(Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements)
                    {
                        if(entity.hasUIInterface && !Globals.ToolTipInterface.isItemGrabbed) // Furnace, Driller gibi şeyler için GUI interface
                        {
                            entity.ShowUIInterface();
                            break;
                        }
                    }
                }
                else
                {
                    //miningGUI.progressBar.CurrentValue = 1;
                    miningGUI.Hide();
                }
            }
        }

        public override void DrawEntityAnimation()
        {
            _anims.Draw(new Vector2(Position.X, Position.Y), Scale, Color.White);
        }

        public override void OnCollisionEnter()
        {
            Vector2 netSlideDirection = Vector2.Zero;

            foreach (var col in Globals.entities)
            {
                if (this.Rectangle.Intersects(col.Rectangle) && col.IsCollidable && col.Tag != "Player")
                {
                    // Calculate the intersection between the colliders
                    float intersectionWidth  = Math.Min(this.Rectangle.X + this.Rectangle.Width, col.Rectangle.X + col.Rectangle.Width) - Math.Max(this.Rectangle.X, col.Rectangle.X);
                    float intersectionHeight = Math.Min(this.Rectangle.Y + this.Rectangle.Height, col.Rectangle.Y + col.Rectangle.Height) - Math.Max(this.Rectangle.Y, col.Rectangle.Y);

                    // Determine the direction of the slide based on the intersection
                    Vector2 slideDirection = Vector2.Zero;
                    if (intersectionWidth < intersectionHeight)
                    {
                        // Slide horizontally
                        if (this.Rectangle.X < col.Rectangle.X)
                        {
                            slideDirection.X = -1; // Slide to the left
                        }
                        else
                        {
                            slideDirection.X = 1; // Slide to the right
                        }
                    }
                    else
                    {
                        // Slide vertically
                        if (this.Rectangle.Y < col.Rectangle.Y)
                        {
                            slideDirection.Y = -1; // Slide upwards
                        }
                        else
                        {
                            slideDirection.Y = 1; // Slide downwards
                        }
                    }

                    // Accumulate the slide direction
                    netSlideDirection += slideDirection;
                }
            }

            // Normalize the net slide direction to ensure consistent speed
            if (netSlideDirection != Vector2.Zero)
            {
                netSlideDirection.Normalize();
            }

            // Adjust the position using the net slide direction and speed
            Position += netSlideDirection * Globals.Time * _speed;
        }

        public void OnCollisionEnter(List<Rail> rails)
        {
            foreach (var rail in rails)
            {
                if (Rectangle.Intersects(rail.Rectangle))
                {
                    switch (rail.GetRailType())
                    {
                        case 0:
                            _direction = new Vector2(0, -1); // YUKARI
                            Position += _direction * Globals.Time * (_speed * .5f);
                            break;
                        case 1:
                            _direction = new Vector2(1, 0); // SAG
                            Position += _direction * Globals.Time * (_speed * .5f);
                            break;
                        case 2:
                            _direction = new Vector2(-1, 0); // SOL
                            Position += _direction * Globals.Time * (_speed * .5f);
                            break;
                        case 3:
                            _direction = new Vector2(0, 1); // ASAGI
                            Position += _direction * Globals.Time * (_speed * .5f);
                            break;
                        case 4:
                            _direction = new Vector2(0.5f, -1); // SAG ALT YUKARI
                            _direction.Normalize();
                            Position += _direction * Globals.Time * (_speed * .5f);
                            break;
                        case 5:
                            _direction = new Vector2(-1, 0.5f); // SAG ALT ASAGI
                            _direction.Normalize();
                            Position += _direction * Globals.Time * (_speed * .5f);
                            break;
                        case 6:
                            _direction = new Vector2(-1, -0.5f); // SAG UST YUKARI
                            _direction.Normalize();
                            Position += _direction * Globals.Time * (_speed * .5f);
                            break;
                        case 7:
                            _direction = new Vector2(0.5f, 1); // SAG UST ASAGI
                            _direction.Normalize();
                            Position += _direction * Globals.Time * (_speed * .5f);
                            break;
                        case 8:
                            _direction = new Vector2(1, 0.5f); // SOL ALT ASAGI
                            _direction.Normalize();
                            Position += _direction * Globals.Time * (_speed * .5f);
                            break;
                        case 9:
                            _direction = new Vector2(-0.5f, -1); // SOL ALT YUKARI
                            _direction.Normalize();
                            Position += _direction * Globals.Time * (_speed * .5f);
                            break;
                        case 10:
                            _direction = new Vector2(-0.5f, 1); // SOL UST ASAGI
                            _direction.Normalize();
                            Position += _direction * Globals.Time * (_speed * .5f);
                            break;
                        case 11:
                            _direction = new Vector2(1, -0.5f); // SOL UST YUKARI
                            _direction.Normalize();
                            Position += _direction * Globals.Time * (_speed * .5f);
                            break;
                    }

                    break;
                }
                else
                {
                    _direction = new Vector2(0, 0);

                    //TODO: ADD SLOWING DOWN CODE..
                }
            }
        }

        public void OnCollisionEnter(ref List<Tile> drawingTiles)
        {
            //== TODO: NORMAL COLLISION EKLE ==//
            Vector2 netSlideDirection = Vector2.Zero;

            foreach (var col in drawingTiles)
            {
                if (this.Rectangle.Intersects(col.rectangle))
                {
                    // Calculate the intersection between the colliders
                    float intersectionWidth = Math.Min(this.Rectangle.X + this.Rectangle.Width, col.rectangle.X + col.rectangle.Width) - Math.Max(this.Rectangle.X, col.rectangle.X);
                    float intersectionHeight = Math.Min(this.Rectangle.Y + this.Rectangle.Height, col.rectangle.Y + col.rectangle.Height) - Math.Max(this.Rectangle.Y, col.rectangle.Y);

                    // Determine the direction of the slide based on the intersection
                    Vector2 slideDirection = Vector2.Zero;
                    if (intersectionWidth < intersectionHeight)
                    {
                        // Slide horizontally
                        if (this.Rectangle.X < col.rectangle.X)
                        {
                            slideDirection.X = -1; // Slide to the left
                        }
                        else
                        {
                            slideDirection.X = 1; // Slide to the right
                        }
                    }
                    else
                    {
                        // Slide vertically
                        if (this.Rectangle.Y < col.rectangle.Y)
                        {
                            slideDirection.Y = -1; // Slide upwards
                        }
                        else
                        {
                            slideDirection.Y = 1; // Slide downwards
                        }
                    }

                    // Accumulate the slide direction
                    netSlideDirection += slideDirection;
                }
            }

            // Normalize the net slide direction to ensure consistent speed
            if (netSlideDirection != Vector2.Zero)
            {
                netSlideDirection.Normalize();
            }

            // Adjust the position using the net slide direction and speed
            Position += netSlideDirection * Globals.Time * _speed;
        }

        private void CalculateRectangle()
        {
            // 2'ye hep bölücez playerTexture.Width ama bir de 8 frame old. için ona da bölünce 16'ya bölmüş oluyoruz toplam
            // Bir de kalınlığı (playerTexture.Width / 8)'di ben (playerTexture.Width / 12) yaptım o yüzden ekstra _position.X'e +15 ekledim.
            //_rectangle = new((int)_position.X - ((int)_origin.X * (int)_scale.X) + (int)(playerTexture.Width / 16 * _scale.X) + 25,
            //                 (int)_position.Y - ((int)_origin.Y * (int)_scale.Y) + (int)(playerTexture.Height / 10 * _scale.Y),
            //            (playerTexture.Width / 16) * (int)_scale.X, (playerTexture.Height / 16) * (int)_scale.Y);

            Rectangle = new((int)Position.X - ((int)Origin.X * (int)Scale.X) + (int)(playerTexture.Width / 16 * Scale.X) + 25,
                             (int)Position.Y - ((int)Origin.Y * (int)Scale.Y) + (int)(playerTexture.Height / 10 * Scale.Y),
                        (playerTexture.Width / 16) * (int)Scale.X, (playerTexture.Height / 16) * (int)Scale.Y);

            sortRectangle = new((int)Position.X - ((int)Origin.X * (int)Scale.X) + (int)(playerTexture.Width / 16 * Scale.X) + 25,
                             (int)Position.Y - ((int)Origin.Y * (int)Scale.Y) + (int)(playerTexture.Height / 10 * Scale.Y),
                        (playerTexture.Width / 16) * (int)Scale.X, (playerTexture.Height / 16) * (int)Scale.Y);
            sortRectangle.Inflate(150, 150);
        }
    }
}
