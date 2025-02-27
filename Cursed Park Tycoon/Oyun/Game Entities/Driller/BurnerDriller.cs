using Microsoft.Xna.Framework;
using Sandbox.Engine;
using Sandbox.Engine.Particle;
using Sandbox.Oyun.Game_Entities.Mines;
using System;
using System.Collections.Generic;

namespace Sandbox.Oyun.Game_Entities.Driller
{
    public class BurnerDriller : Entity
    {
        private string drillingMine;

        private readonly float miningTime = 2f;
        private float remainingTime;

        //== PARTICLES ==//
        private readonly StaticEmitter _staticEmitter;
        private ParticleEmitterData _particleEmitterData;
        private readonly ParticleEmitter _particleEmitter;
        private ParticleData _particleData;
        private Vector2 _smokegravity = new(0, 0f);
        private Vector2 _particleOffset = new(15, 0);
        private readonly int _totalParticles = 20;
        //===============//

        //== Animation Manager ==//
        private readonly AnimationManager _animationManager = new();
        private bool _updateAnimation = false;

        //== GUI Interface Data (We send these datas to the InventoryManager's furnace template) ==//

        public EntityInterface entityInterface;

        private int bakingProgressBarValue = 0; // Baking progress
        private int fuelProgressBarValue = 0; // Fuel progress

        private int addCount = 1;

        //=========================================================================================//

        public BurnerDriller(string source, Vector2 position, Vector2 scale, Color color) : base(source, position, scale, color)
        {
            Tag = "Burner Driller";
            Details = "Burner Driller";

            hasAnimation = true;
            hasUIInterface = true;
            AddAnimation(0, 5, 0, 1, .3f);

            //SetCustomOrigin(new Vector2(-this.Origin.X / 10 - 20, this.Origin.Y / 2 - 5));
            //ScaleRect(new Vector2(2f, .8f));

            remainingTime = miningTime;

            //== PARTICLES ==//
            _staticEmitter = new(position + _particleOffset);
            _particleData = new()
            {
                colorStart = new Color(100, 100, 100, 1f),
                colorEnd = new Color(15, 15, 15, 1f),
                sizeStart = 10,
                sizeEnd = 100
            };
            _particleEmitterData = new()
            {
                interval = 0.2f, // ... saniye aralıkla spawn ediyor.
                emitCount = 2, // Her saniye bu kadar spawn ediyor.
                angleVariance = 40, // angle noktasını orijin kabul ediyor. Ne kadar arttırırsan o kadar geniş fırlatır.
                particleData = _particleData,
                angle = 40,
                lifespanMin = 2,
                lifespanMax = 3,
                speedMin = 50,
                speedMax = 90,
            };
            _particleEmitter = new(_staticEmitter, _particleEmitterData, _smokegravity, this._totalParticles);
            ParticleManager.AddParticleEmitter(_particleEmitter);
            //===============//

            //== Entity Interface Setup ==//

            entityInterface = new("Burner Driller", "Textures/burnerDriller");
        }

        public override void OnCollisionEnter()
        {
            foreach (var col in Globals.entities) 
            {
                if (this.Rectangle.Intersects(col.Rectangle) && col.Tag == "Stone" && !col.IsCollidable)
                {
                    drillingMine = col.Tag;
                    UpdateLogic();
                    break;
                }
                else if (this.Rectangle.Intersects(col.Rectangle) && col.Tag == "Iron" && !col.IsCollidable)
                {
                    drillingMine = col.Tag;
                    UpdateLogic();
                    break;
                }
                else if (this.Rectangle.Intersects(col.Rectangle) && col.Tag == "Coal" && !col.IsCollidable)
                {
                    drillingMine = col.Tag;
                    UpdateLogic();
                    break;
                }
            }

            // KOMŞU RAİL VAR MI KONTROL ET VARSA ONUNKİNİ TRUE YAP BOOLU
            foreach (var rail in Globals.rails)
            {
                if(rail.Position == this.Position + new Vector2(0, 128))
                {
                    rail.hasNewDirection = true;
                    rail.neighbourDrillerCount++;
                    break;
                }

                //break;
            }

            base.OnCollisionEnter();
        }

        public override void UpdateLogic()
        {
            Drill();

            if (_updateAnimation)
                _animationManager.UpdateAnimationWithOffset(0, 1);
            else
                _animationManager.SetAnimationIndex(0, 0);

            base.UpdateLogic();
        }

        public override void ShowUIInterface()
        {
            InventoryManager.inventoryGUI.ShowInventoryGUI();

            InventoryManager.inventoryGUI.rightPanel.RemoveChildren();

            this.entityInterface.craftingPanel.IsHidden = true;
            InventoryManager.inventoryGUI.rightPanel.AddChild(this.entityInterface.mainPanel);

            base.ShowUIInterface();
        }

        public override void CleanUp()
        {
            ParticleManager.RemoveParticleEmitter(_particleEmitter);

            base.CleanUp();
        }

        private void Drill()
        {
            remainingTime -= Globals.Time;

            if(remainingTime <= 0f)
            {
                switch (drillingMine) //== TODO: Animasyon aşağı doğru inince burası kontrol edilsin. ==//
                {
                    case "Stone":
                        StartSmoke();
                        _updateAnimation = true;

                        CollactableStone stone = new(this.Position + new Vector2(0, 140));
                        Globals.entities.Add(stone);
                        //InventoryManager.AddItem("stone", 1);

                        remainingTime = miningTime;
                        break;
                    case "Iron":
                        StartSmoke();
                        _updateAnimation = true;

                        CollactableIron iron = new(this.Position + new Vector2(0, 140));
                        Globals.entities.Add(iron);
                        //InventoryManager.AddItem("iron", 1);

                        remainingTime = miningTime;
                        break;
                    case "Coal":
                        StartSmoke();
                        _updateAnimation = true;

                        CollactableCoal coal = new(this.Position + new Vector2(0, 140));
                        Globals.entities.Add(coal);
                        //InventoryManager.AddItem("coal", 1);

                        remainingTime = miningTime;
                        break;
                    default:
                        StopSmoke();
                        break;
                }
            }
        }

        public void StartSmoke()
        {
            _particleEmitter.StartEmitter();
        }

        public void StopSmoke()
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

                int snappedWidth = (int)(Math.Floor((_texture2D.Width / framesX * Scale.X) / 64.0) * 64);
                int snappedHeigth = (int)(Math.Floor((_texture2D.Height * Scale.Y) / 64.0) * 64);

                Origin = new((_texture2D.Width / 2) / framesX, (_texture2D.Height / 2));

                Rectangle = new Rectangle(snappedX,
                                          snappedY,
                                          snappedWidth,
                                          snappedHeigth);
            }
            else
            {
                // Snap position to 64x64 grid
                int snappedX = (int)(Math.Floor(Position.X / 64.0) * 64);
                int snappedY = (int)(Math.Floor(Position.Y / 64.0) * 64);
                snappedY += 64;

                int snappedWidth = (int)(Math.Floor((_texture2D.Width / framesX * Scale.X) / 64.0) * 64);
                int snappedHeigth = (int)(Math.Floor((_texture2D.Height / framesY * Scale.Y) / 64.0) * 64);

                Origin = new((_texture2D.Width / 2) / framesX, (_texture2D.Height / 2) / framesY);

                Rectangle = new Rectangle(
                                      snappedX,
                                      snappedY,
                                      snappedWidth,
                                      snappedHeigth);
            }

            _animationManager.AddAnimation(animKey, new(_texture2D, framesX, framesY, row, frameTime));
        }
        public override void DrawEntityAnimation()
        {
            _animationManager.Draw(Position, Scale, Color);
        }
    }
}
