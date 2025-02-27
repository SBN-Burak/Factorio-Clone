using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sandbox.Engine;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox.Oyun.Game_Entities.Mines
{
    public class CollactableStone : Entity
    {

        private Vector2 _direction = Vector2.Zero; // Movement Direction
        private readonly float _speed = 125;

        public CollactableStone(Vector2 position)
        {
            TextureSource = "Textures/stone";
            _texture2D = Globals.Content.Load<Texture2D>(TextureSource);

            Tag = "Collectable Stone";
            Details = "Collectable Stone";
            Position = position;
            Scale = Vector2.One;
            Color = Color.White;

            IsCollidable = false; // Player ile collide etmesin diye (Pistonda)

            CalculateRectangle();
        }

        //TODO: Scale'i terraindeki halinden daha küçük olacak ve player ile yaklaşınca ses çıkarıp
        //          envatere 1 ekleyip yok olacak.

        //TODO: OnCollision <- Rail için aynı player gibi yap. Piston üzerinde hareket etmesi için
        //          Bu arada pistonlarda yan yana iki tane maden olmayacak tekli gidecekler (Mindustry gibi)

        public override void UpdateLogic()
        {
            MovementLogic();

            base.UpdateLogic();
        }

        public override void OnCollisionEnter()
        {
            // Filter rails that have new directions
            var railsWithNewDirection = Globals.rails.Where(rail => rail.hasNewDirection);

            foreach (var rail in railsWithNewDirection)
            {
                if (IsFullyInside(rail.Rectangle))
                {
                    switch (rail.GetRailType())
                    {
                        case 0:
                            _direction = new Vector2(0, -1);
                            //Position += _direction * Globals.Time * _speed; // Yukarı
                            break;
                        case 1:
                            _direction = new Vector2(1, 0);
                            //Position += _direction * Globals.Time * _speed;  // Sag
                            break;
                        case 2:
                            _direction = new Vector2(-1, 0);
                            //Position += _direction * Globals.Time * _speed; // Sol
                            break;
                        case 3:
                            _direction = new Vector2(0, 1);
                            //Position += _direction * Globals.Time * _speed; // Asagi
                            break;
                        case 4:
                            _direction = new Vector2(.35f, -1);
                            //Position += _direction * Globals.Time * _speed; // SAG ALT YUKARI
                            break;
                        case 5:
                            _direction = new Vector2(-1, 0.35f);
                            //Position += _direction * Globals.Time * _speed; // SAG ALT ASAGI
                            break;
                        case 6:
                            _direction = new Vector2(-1f, -0.35f);
                            //Position += _direction * Globals.Time * _speed; // SAG UST YUKARI
                            break;
                        case 7:
                            _direction = new Vector2(0.35f, 1);
                            //Position += _direction * Globals.Time * _speed; // SAG UST ASAGI
                            break;
                        case 8:
                            _direction = new Vector2(1, 0.35f);
                            //Position += _direction * Globals.Time * _speed; // SOL ALT ASAGI
                            break;
                        case 9:
                            _direction = new Vector2(-0.35f, -1);
                            //Position += _direction * Globals.Time * _speed; // SOL ALT YUKARI
                            break;
                        case 10:
                            _direction = new Vector2(-0.35f, 1);
                            //Position += _direction * Globals.Time * _speed; // SOL UST ASAGI
                            break;
                        case 11:
                            _direction = new Vector2(1, -0.35f);
                            //Position += _direction * Globals.Time * _speed; // SOL UST YUKARI
                            break;
                    }

                    break;
                }

                //rail.renk = Color.Red;
            }

            base.OnCollisionEnter();
        }

        private bool IsFullyInside(Rectangle railRect)
        {
            // Check if the movable object's bounding box (Rectangle) is fully inside the rail's Rectangle
            return railRect.Contains(Rectangle);
        }

        private void MovementLogic()
        {
            Position += _direction * Globals.Time * _speed;

            CalculateRectangle();
        }

        private void CalculateRectangle()
        {
            Rectangle = new Rectangle((int)Position.X - ((int)Origin.X * (int)Scale.X),
                                      (int)Position.Y - ((int)Origin.Y * (int)Scale.Y),
                                      (int)(_texture2D.Width * Scale.X),
                                      (int)(_texture2D.Height * Scale.Y));
        }

    }
}
