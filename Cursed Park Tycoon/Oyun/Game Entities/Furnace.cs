using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using Sandbox.Engine;
using Sandbox.Engine.Particle;
using Sandbox.Oyun.Inventory_System;
using System;
using System.Collections.Generic;

namespace Sandbox.Oyun.Game_Entities
{
    public class Furnace : Entity
    {
        //== PARTICLES ==//
        private readonly StaticEmitter _staticEmitter;
        private ParticleEmitterData _particleEmitterData;
        private readonly ParticleEmitter _particleEmitter;
        private ParticleData _particleData;
        private Vector2 _smokegravity = new(0, 0.003f);
        private Vector2 _particleOffset = new (35, -2);
        private readonly int _totalParticles = 25;
        //===============//

        //== Animation Manager ==//
        private readonly AnimationManager _animationManager = new();
        private bool _updateAnimation = false;
        //=======================//

        //== Furnace Baking ==//
        private readonly float bakingTime = 1f;
        private float remainingTime;
        //====================//

        //== GUI Interface Data envanter sağ panelde açılıyor ==//
        public EntityInterface entityInterface;

        public Rectangle sortRectangle;

        public Furnace(string source, Vector2 position, Vector2 scale, Color color) : base(source, position, scale, color)
        {
            Tag = "Furnace";
            Details = "Furnace";

            hasAnimation = true;
            hasUIInterface = true;
            AddAnimation(0, 5, 0, 1, .25f);
            
            remainingTime = bakingTime;

            _staticEmitter = new(position + _particleOffset);

            _particleData = new()
            {
                colorStart = Color.LawnGreen,     //new Color(5, 5, 5, 1f)
                colorEnd = Color.LawnGreen, //new Color(255, 255, 255, 1f)
                sizeStart = 10,
                sizeEnd = 200
            };

            _particleEmitterData = new()
            {
                interval = 0.3f, // 0.2 saniye aralıkla spawn ediyor.
                emitCount = 2, // Her saniye bu kadar spawn ediyor.
                angleVariance = 15, // angle noktasını orijin kabul ediyor. Ne kadar arttırırsan o kadar geniş fırlatır.
                particleData = _particleData,
                angle = 45,
                lifespanMin = 3,
                lifespanMax = 8,
                speedMin = 50,
                speedMax = 100,
            };

            _particleEmitter = new(_staticEmitter, _particleEmitterData, _smokegravity, this._totalParticles);

            ParticleManager.AddParticleEmitter(_particleEmitter);

            //== Entity Interface Setup ==//

            entityInterface = new("Furnace", "Textures/furnace");

            sortRectangle.Inflate(0, 200);
            SortIntersectingEntities();
        }

        public override void UpdateLogic()
        {
            CheckFuelItemSlotHovered();
            //CheckFuelItemSlotPressed();

            if (InputManager.KeyPressed(Keys.P))
            {
                _particleEmitter.StartEmitter();
                _updateAnimation = true;
            }
            if (InputManager.KeyPressed(Keys.O))
            {
                _particleEmitter.StopEmitter();
                _updateAnimation = false;
            }

            if(_updateAnimation)
            {
                StartFurnace();
               _animationManager.UpdateAnimationWithOffset(0, 1);
            }
            else
            {
                StopFurnace();
               _animationManager.SetAnimationIndex(0, 0);
            }

            base.UpdateLogic();
        }

        public void CheckFuelItemSlotHovered()
        {
            // Mouse Hover// Mouse Hover
            entityInterface.fuelSlot.slotButton.OnMouseEnter = element =>
            {
                if (!Globals.ToolTipInterface.isItemGrabbed)
                {
                    Globals.ToolTipInterface.mainPanel.Size = new Vector2(60, 60);
                    Globals.ToolTipInterface.mainPanel.IsHidden = false;

                    Globals.ToolTipInterface.mainPanel.AddChild(
                            new Image(Anchor.TopLeft, Vector2.One,
                                new TextureRegion(Globals.Content.Load<Texture2D>("Textures/coal")), true)
                            {
                                ImageScale = new Vector2(1.25f),
                                PositionOffset = new Vector2(0, 0)
                            });
                }
            };
            entityInterface.fuelSlot.slotButton.OnMouseExit = element =>
            {
                if (!Globals.ToolTipInterface.isItemGrabbed)
                {
                    Globals.ToolTipInterface.mainPanel.IsHidden = true;
                    Globals.ToolTipInterface.mainPanel.RemoveChildren();
                }
            };
        }

        public override void ShowUIInterface()
        {
            InventoryManager.inventoryGUI.ShowInventoryGUI();

            InventoryManager.inventoryGUI.rightPanel.RemoveChildren();
            InventoryManager.inventoryGUI.rightPanel.AddChild(this.entityInterface.mainPanel);

            base.ShowUIInterface();
        }

        public override void CleanUp()
        {
            ParticleManager.RemoveParticleEmitter(_particleEmitter);

            base.CleanUp();
        }

        public void StartFurnace()
        {
            remainingTime -= Globals.Time;

            if (remainingTime <= 0f)
            {
                InventoryManager.RemoveItems(new CraftingRecipe("stone", 1));

                remainingTime = bakingTime;
            }

        }
        public void StopFurnace()
        {
            _particleEmitter.StopEmitter();
        }

        ////////////////////////

        public void AddAnimation(Object animKey, int framesX, int framesY, int row, float frameTime)
        {
            hasAnimation = true;

            if (framesY == 0)
            {
                // Snap position to 64x64 grid
                int snappedX = (int)(Math.Floor(Position.X / 64.0) * 64);
                int snappedY = (int)(Math.Floor(Position.Y / 64.0) * 64);
                snappedY += 64;

                int snappedWidth =  (int)(Math.Floor((_texture2D.Width / framesX * Scale.X) / 64.0) * 64);
                int snappedHeigth = (int)(Math.Floor((_texture2D.Height * Scale.Y) / 64.0) * 64);

                // Snap origin to 64x64 grid
                int snappedOriginX = (int)(Math.Floor(Origin.X / 64.0f) * 64);
                int snappedOriginY = (int)(Math.Floor(Origin.Y / 64.0f) * 64);

                Origin = new Vector2(snappedOriginX, snappedOriginY);

                Rectangle = new Rectangle(snappedX,
                                          snappedY,
                                          snappedWidth*2, //*2
                                          snappedHeigth/2);

                sortRectangle = Rectangle;
            }
            else
            {
                // Snap position to 64x64 grid
                int snappedX = (int)(Math.Floor(Position.X / 64.0) * 64);
                int snappedY = (int)(Math.Floor(Position.Y / 64.0) * 64);

                int snappedWidth = (int)(Math.Floor((_texture2D.Width / framesX * Scale.X) / 64.0) * 64);
                int snappedHeigth = (int)(Math.Floor((_texture2D.Height / framesY * Scale.Y) / 64.0) * 64);

                // Snap origin to 64x64 grid
                int snappedOriginX = (int)(Math.Floor(Origin.X / 64.0f) * 64);
                int snappedOriginY = (int)(Math.Floor(Origin.Y / 64.0f) * 64);

                Origin = new Vector2(snappedOriginX, snappedOriginY);

                Rectangle = new Rectangle(snappedX,
                                          snappedY,
                                          snappedWidth*2, //*2
                                          snappedHeigth);

                sortRectangle = Rectangle;
            }

            _animationManager.AddAnimation(animKey, new(_texture2D, framesX, framesY, row, frameTime));
        }

        public override void DrawEntityAnimation()
        {
            _animationManager.Draw(Position, Scale, Color);
        }
    }
}
