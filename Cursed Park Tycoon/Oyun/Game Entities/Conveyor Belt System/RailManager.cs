using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sandbox.Engine;
using System.Collections.Generic;
using static Sandbox.Oyun.Rail;

namespace Sandbox.Oyun
{
    public class RailManager
    {
        private bool activateRailPlacement = false;

        private int rail_state = 1; // 1->YUKRARI, 2->SAG, 3->SOL, 4->ASAGI, ...
        private bool lock_rail_direction = false;
        private float X_AXIS_LOCK;
        private float Y_AXIS_LOCK;

        private readonly Rail blueprintRail;

        public RailManager()
        {
            blueprintRail = new("Textures/new_pistonlar", Vector2.Zero, new Vector2(2f, 2f),
                new Color(Color.White, 0.5f), RailType.YUKARI);
        }

        public void Activate()
        {
            activateRailPlacement = true;
        }

        public void Deactivate()
        {
            activateRailPlacement = false;
        }

        // Mouse ile transparent rayı göstermek için. Nereye koyacağımız ve nasıl gözüktüğünü görmek için.
        public void UpdateBlueprintRailPos(Vector2 position)
        {
            if(activateRailPlacement)
            {
                if(Globals.rails.Count > 0)
                {
                    // UP
                    if(IsThereLastRail(new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y - 64)))
                    {
                        Rail gettingRailForInfo = GetRail(new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y - 64));

                        {
                            if (gettingRailForInfo.railType == (int)RailType.ASAGI || gettingRailForInfo.railType == (int)RailType.SAG_UST_ASAGI ||
                                gettingRailForInfo.railType == (int)RailType.SOL_UST_ASAGI)
                            {
                                if (InputManager.KeyPressed(Keys.R))
                            {
                                rail_state = (rail_state % 4) + 1;

                                if (rail_state > 4)
                                    rail_state = 1;

                                switch (rail_state)
                                {
                                    // 3 asagi, 8 sag, 5 sol, 0 yukarı bunlar frame indexleri
                                    case 1:
                                        blueprintRail.railType = 3;
                                        break;
                                    case 2:
                                        blueprintRail.railType = 8;
                                        break;
                                    case 3:
                                        blueprintRail.railType = 5;
                                        break;
                                    case 4:
                                        blueprintRail.railType = 0;
                                        break;
                                }
                            }
                                if (Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements &&
                                    !IsThereEntity(blueprintRail.Position))
                                {
                                    {
                                    //if (rail_state == 1)
                                    //{
                                    //    if (!lock_rail_direction)
                                    //    {
                                    //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                    //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                    //        X_AXIS_LOCK = blueprintRail.Position.X;
                                    //        rails.Add(rail);
                                    //
                                    //        lock_rail_direction = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        if (!IsThereRail(new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y)))
                                    //        {
                                    //            Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y),
                                    //                new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                    //
                                    //            rails.Add(lockedRail);
                                    //        }
                                    //    }
                                    //}
                                    //else if (rail_state == 2)
                                    //{
                                    //    if (!lock_rail_direction)
                                    //    {
                                    //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                    //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SOL_ALT_ASAGI);
                                    //        X_AXIS_LOCK = blueprintRail.Position.X;
                                    //        rails.Add(rail);
                                    //
                                    //        lock_rail_direction = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        if (!IsThereRail(new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y)))
                                    //        {
                                    //            Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y),
                                    //                new Vector2(2f, 2f), Color.White, RailType.SOL_ALT_ASAGI);
                                    //
                                    //            rails.Add(lockedRail);
                                    //        }
                                    //    }
                                    //}
                                    //else if (rail_state == 3)
                                    //{
                                    //    if (!lock_rail_direction)
                                    //    {
                                    //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                    //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SAG_ALT_ASAGI);
                                    //        X_AXIS_LOCK = blueprintRail.Position.X;
                                    //        rails.Add(rail);
                                    //
                                    //        lock_rail_direction = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        if (!IsThereRail(new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y)))
                                    //        {
                                    //            Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y),
                                    //                new Vector2(2f, 2f), Color.White, RailType.SAG_ALT_ASAGI);
                                    //
                                    //            rails.Add(lockedRail);
                                    //        }
                                    //    }
                                    //}
                                    //else if (rail_state == 4)
                                    //{
                                    //    if (!lock_rail_direction)
                                    //    {
                                    //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                    //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                    //        X_AXIS_LOCK = blueprintRail.Position.X;
                                    //        rails.Add(rail);
                                    //
                                    //        lock_rail_direction = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        if (!IsThereRail(new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y)))
                                    //        {
                                    //            Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y),
                                    //                new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                    //
                                    //            rails.Add(lockedRail);
                                    //        }
                                    //    }
                                    //}
                                }

                                    switch (rail_state)
                                {
                                    // 3 asagi, 8 sag, 5 sol, 0 yukarı bunlar frame indexleri
                                    case 1:
                                        Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                            new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail);
                                        blueprintRail.railType = (int)RailType.ASAGI;

                                        if (rail.railType != gettingRailForInfo.railType)
                                        {
                                            rail.hasNewDirection = true;
                                        }
                                        break;
                                    case 2:
                                        Rail rail2 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                            new Vector2(2f, 2f), Color.White, RailType.SOL_ALT_ASAGI);
                                        //X_AXIS_LOCK = blueprintRail.Position.X;
                                        Globals.rails.Add(rail2);
                                        blueprintRail.railType = (int)RailType.SAG;

                                        if (rail2.railType != gettingRailForInfo.railType)
                                        {
                                            rail2.hasNewDirection = true;
                                        }
                                        break;
                                    case 3:
                                        Rail rail3 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                            new Vector2(2f, 2f), Color.White, RailType.SAG_ALT_ASAGI);
                                        //X_AXIS_LOCK = blueprintRail.Position.X;
                                        Globals.rails.Add(rail3);
                                        blueprintRail.railType = (int)RailType.SOL;

                                        if (rail3.railType != gettingRailForInfo.railType)
                                        {
                                            rail3.hasNewDirection = true;
                                        }
                                        break;
                                    case 4:
                                        Rail rail4 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                            new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                        //X_AXIS_LOCK = blueprintRail.Position.X;
                                        Globals.rails.Add(rail4);
                                        blueprintRail.railType = (int)RailType.YUKARI;
                                            if (rail4.railType != gettingRailForInfo.railType)
                                            {
                                                rail4.hasNewDirection = true;
                                            }
                                            break;
                                }
                                }
                            }
                            else if (gettingRailForInfo.railType == (int)RailType.YUKARI)
                        {
                            //  Ters yönde old. için 4 deffault rail'leri döndür.
                            if (InputManager.KeyPressed(Keys.R))
                            {
                                rail_state = (rail_state % 4) + 1;

                                if (rail_state > 4) // 1-4 arası tutuyorum. rail frame'leri
                                    rail_state = 1;

                                blueprintRail.railType = rail_state - 1; // -1 for fitting rail rendering index
                            }
                            if (Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements &&
                                !IsThereEntity(blueprintRail.Position))
                            {
                                switch (rail_state)
                                {
                                    case 1:
                                        Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                            new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                        //X_AXIS_LOCK = blueprintRail.Position.X;
                                        Globals.rails.Add(rail);
                                        blueprintRail.railType = (int)RailType.YUKARI;
                                            if (rail.railType != gettingRailForInfo.railType)
                                            {
                                                rail.hasNewDirection = true;
                                            }

                                            break;
                                    case 2:
                                        Rail rail2 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                            new Vector2(2f, 2f), Color.White, RailType.SAG);
                                        //X_AXIS_LOCK = blueprintRail.Position.X;
                                        Globals.rails.Add(rail2);
                                        blueprintRail.railType = (int)RailType.SAG;
                                            if (rail2.railType != gettingRailForInfo.railType)
                                            {
                                                rail2.hasNewDirection = true;
                                            }
                                            break;
                                    case 3:
                                        Rail rail3 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                            new Vector2(2f, 2f), Color.White, RailType.SOL);
                                        //X_AXIS_LOCK = blueprintRail.Position.X;
                                        Globals.rails.Add(rail3);
                                        blueprintRail.railType = (int)RailType.SOL;
                                            if (rail3.railType != gettingRailForInfo.railType)
                                            {
                                                rail3.hasNewDirection = true;
                                            }
                                            break;
                                    case 4:
                                        Rail rail4 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                            new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                        //X_AXIS_LOCK = blueprintRail.Position.X;
                                        Globals.rails.Add(rail4);
                                        blueprintRail.railType = (int)RailType.ASAGI;
                                            if (rail4.railType != gettingRailForInfo.railType)
                                            {
                                                rail4.hasNewDirection = true;
                                            }
                                            break;
                                }
                            }
                        }
                        }
                    }
                    // DOWN
                    else if(IsThereLastRail(new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y + 64)))
                    {
                        Rail bottomRail = GetRail(new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y + 64));

                        {
                            if (bottomRail.railType == (int)RailType.YUKARI || bottomRail.railType == (int)RailType.SAG_ALT_YUKARI ||
                                bottomRail.railType == (int)RailType.SOL_ALT_YUKARI)
                            {
                                if (InputManager.KeyPressed(Keys.R))
                                {
                                    rail_state = (rail_state % 4) + 1;

                                    if (rail_state > 4)
                                        rail_state = 1;

                                    switch (rail_state)
                                    {
                                        // 0 yukarı, 11 sag, 6 sol, 3 asagı bunlar frame indexleri
                                        case 1:
                                            blueprintRail.railType = 0;
                                            break;
                                        case 2:
                                            blueprintRail.railType = 11;
                                            break;
                                        case 3:
                                            blueprintRail.railType = 6;
                                            break;
                                        case 4:
                                            blueprintRail.railType = 3;
                                            break;
                                    }
                                }
                                if (Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements &&
                                    !IsThereEntity(blueprintRail.Position))
                                {
                                    {
                                        //if (rail_state == 1)
                                        //{
                                        //    if (!lock_rail_direction)
                                        //    {
                                        //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                        //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                        //        X_AXIS_LOCK = blueprintRail.Position.X;
                                        //        rails.Add(rail);
                                        //
                                        //        lock_rail_direction = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!IsThereRail(new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y)))
                                        //        {
                                        //            Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y),
                                        //                new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                        //
                                        //            rails.Add(lockedRail);
                                        //        }
                                        //    }
                                        //}
                                        //else if (rail_state == 2)
                                        //{
                                        //    if (!lock_rail_direction)
                                        //    {
                                        //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                        //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SOL_UST_YUKARI);
                                        //        X_AXIS_LOCK = blueprintRail.Position.X;
                                        //        rails.Add(rail);
                                        //
                                        //        lock_rail_direction = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!IsThereRail(new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y)))
                                        //        {
                                        //            Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y),
                                        //                new Vector2(2f, 2f), Color.White, RailType.SOL_UST_YUKARI);
                                        //
                                        //            rails.Add(lockedRail);
                                        //        }
                                        //    }
                                        //}
                                        //else if (rail_state == 3)
                                        //{
                                        //    if (!lock_rail_direction)
                                        //    {
                                        //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                        //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SAG_UST_YUKARI);
                                        //        X_AXIS_LOCK = blueprintRail.Position.X;
                                        //        rails.Add(rail);
                                        //
                                        //        lock_rail_direction = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!IsThereRail(new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y)))
                                        //        {
                                        //            Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y),
                                        //                new Vector2(2f, 2f), Color.White, RailType.SAG_UST_YUKARI);
                                        //
                                        //            rails.Add(lockedRail);
                                        //        }
                                        //    }
                                        //}
                                        //else if (rail_state == 4)
                                        //{
                                        //    if (!lock_rail_direction)
                                        //    {
                                        //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                        //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                        //        X_AXIS_LOCK = blueprintRail.Position.X;
                                        //        rails.Add(rail);
                                        //
                                        //        lock_rail_direction = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!IsThereRail(new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y)))
                                        //        {
                                        //            Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y),
                                        //                new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                        //
                                        //            rails.Add(lockedRail);
                                        //        }
                                        //    }
                                        //}
                                    }


                                    switch (rail_state)
                                    {
                                        // 0 yukarı, 11 sag, 6 sol, 3 asagı bunlar frame indexleri
                                        case 1:
                                            Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail);
                                            blueprintRail.railType = (int)RailType.YUKARI;
                                            if (rail.railType != bottomRail.railType)
                                            {
                                                rail.hasNewDirection = true;
                                            }
                                            break;
                                        case 2:
                                            Rail rail2 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SOL_UST_YUKARI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail2);
                                            blueprintRail.railType = (int)RailType.SAG;
                                            if (rail2.railType != bottomRail.railType)
                                            {
                                                rail2.hasNewDirection = true;
                                            }
                                            break;
                                        case 3:
                                            Rail rail3 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SAG_UST_YUKARI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail3);
                                            blueprintRail.railType = (int)RailType.SOL;
                                            if (rail3.railType != bottomRail.railType)
                                            {
                                                rail3.hasNewDirection = true;
                                            }
                                            break;
                                        case 4:
                                            Rail rail4 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail4);
                                            blueprintRail.railType = (int)RailType.ASAGI;
                                            if (rail4.railType != bottomRail.railType)
                                            {
                                                rail4.hasNewDirection = true;
                                            }
                                            break;
                                    }
                                }
                            }
                            else if (bottomRail.railType == (int)RailType.ASAGI)
                            {
                                //  Ters yönde old. için 4 deffault rail'leri döndür.
                                if (InputManager.KeyPressed(Keys.R))
                                {
                                    rail_state = (rail_state % 4) + 1;

                                    if (rail_state > 4) // 1-4 arası tutuyorum. rail frame'leri
                                        rail_state = 1;

                                    blueprintRail.railType = rail_state - 1; // -1 for fitting rail rendering index
                                }
                                if (Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements &&
                                    !IsThereEntity(blueprintRail.Position))
                                {
                                    switch (rail_state)
                                    {
                                        case 1:
                                            Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail);
                                            if (rail.railType != bottomRail.railType)
                                            {
                                                rail.hasNewDirection = true;
                                            }
                                            break;
                                        case 2:
                                            Rail rail2 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SAG);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail2);
                                            if (rail2.railType != bottomRail.railType)
                                            {
                                                rail2.hasNewDirection = true;
                                            }
                                            break;
                                        case 3:
                                            Rail rail3 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SOL);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail3);
                                            if (rail3.railType != bottomRail.railType)
                                            {
                                                rail3.hasNewDirection = true;
                                            }
                                            break;
                                        case 4:
                                            Rail rail4 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail4);
                                            if (rail4.railType != bottomRail.railType)
                                            {
                                                rail4.hasNewDirection = true;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    // RIGHT
                    else if (IsThereLastRail(new Vector2(blueprintRail.Position.X + 64, blueprintRail.Position.Y)))
                    {
                        Rail rightRail = GetRail(new Vector2(blueprintRail.Position.X + 64, blueprintRail.Position.Y));

                        {
                            if (rightRail.railType == (int)RailType.SOL || rightRail.railType == (int)RailType.SAG_ALT_ASAGI ||
                                    rightRail.railType == (int)RailType.SAG_UST_YUKARI)
                            {
                                if (InputManager.KeyPressed(Keys.R))
                                {
                                    rail_state = (rail_state % 4) + 1;

                                    if (rail_state > 4)
                                        rail_state = 1;

                                    switch (rail_state)
                                    {
                                        // 2 sol, 9 yukarı, 10 alt, 1 sag bunlar frame indexleri
                                        case 1:
                                            blueprintRail.railType = 2;
                                            break;
                                        case 2:
                                            blueprintRail.railType = 9;
                                            break;
                                        case 3:
                                            blueprintRail.railType = 10;
                                            break;
                                        case 4:
                                            blueprintRail.railType = 1;
                                            break;
                                    }
                                }
                                if (Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements &&
                                    !IsThereEntity(blueprintRail.Position))
                                {
                                    {
                                        //if (rail_state == 1)
                                        //{
                                        //    if (!lock_rail_direction)
                                        //    {
                                        //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                        //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SOL);
                                        //        Y_AXIS_LOCK = blueprintRail.Position.Y;
                                        //        rails.Add(rail);
                                        //
                                        //        lock_rail_direction = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!IsThereRail(new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK)))
                                        //        {
                                        //            Rail lockedRail = new("new_pistonlar", new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK),
                                        //                new Vector2(2f, 2f), Color.White, RailType.SOL);
                                        //
                                        //            rails.Add(lockedRail);
                                        //
                                        //            blueprintRail.railType = (int)RailType.SOL;
                                        //        }
                                        //    }
                                        //}
                                        //else if (rail_state == 2)
                                        //{
                                        //    if (!lock_rail_direction)
                                        //    {
                                        //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                        //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SOL_ALT_YUKARI);
                                        //        Y_AXIS_LOCK = blueprintRail.Position.Y;
                                        //        rails.Add(rail);
                                        //
                                        //        lock_rail_direction = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!IsThereRail(new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK)))
                                        //        {
                                        //            Rail lockedRail = new("new_pistonlar", new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK),
                                        //                new Vector2(2f, 2f), Color.White, RailType.SOL_ALT_YUKARI);
                                        //
                                        //            rails.Add(lockedRail);
                                        //
                                        //            blueprintRail.railType = (int)RailType.YUKARI;
                                        //        }
                                        //    }
                                        //}
                                        //else if (rail_state == 3)
                                        //{
                                        //    if (!lock_rail_direction)
                                        //    {
                                        //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                        //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SOL_UST_ASAGI);
                                        //        Y_AXIS_LOCK = blueprintRail.Position.Y;
                                        //        rails.Add(rail);
                                        //
                                        //        lock_rail_direction = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!IsThereRail(new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK)))
                                        //        {
                                        //            Rail lockedRail = new("new_pistonlar", new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK),
                                        //                new Vector2(2f, 2f), Color.White, RailType.SOL_UST_ASAGI);
                                        //
                                        //            rails.Add(lockedRail);
                                        //
                                        //            blueprintRail.railType = (int)RailType.ASAGI;
                                        //        }
                                        //    }
                                        //}
                                        //else if (rail_state == 4)
                                        //{
                                        //    if (!lock_rail_direction)
                                        //    {
                                        //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                        //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SAG);
                                        //        Y_AXIS_LOCK = blueprintRail.Position.Y;
                                        //        rails.Add(rail);
                                        //
                                        //        lock_rail_direction = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!IsThereRail(new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK)))
                                        //        {
                                        //            Rail lockedRail = new("new_pistonlar", new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK),
                                        //                new Vector2(2f, 2f), Color.White, RailType.SAG);
                                        //
                                        //            rails.Add(lockedRail);
                                        //
                                        //            blueprintRail.railType = (int)RailType.SAG;
                                        //        }
                                        //    }
                                        //}
                                    }

                                    switch (rail_state)
                                    {
                                        // 2 sol, 9 yukarı, 10 alt, 1 sag bunlar frame indexleri
                                        case 1:
                                            Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SOL);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail);
                                            blueprintRail.railType = (int)RailType.SOL;
                                            if (rail.railType != rightRail.railType)
                                            {
                                                rail.hasNewDirection = true;
                                            }
                                            break;
                                        case 2:
                                            Rail rail2 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SOL_ALT_YUKARI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail2);
                                            blueprintRail.railType = (int)RailType.YUKARI;
                                            if (rail2.railType != rightRail.railType)
                                            {
                                                rail2.hasNewDirection = true;
                                            }
                                            break;
                                        case 3:
                                            Rail rail3 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SOL_UST_ASAGI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail3);
                                            blueprintRail.railType = (int)RailType.ASAGI;
                                            if (rail3.railType != rightRail.railType)
                                            {
                                                rail3.hasNewDirection = true;
                                            }
                                            break;
                                        case 4:
                                            Rail rail4 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SAG);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail4);
                                            blueprintRail.railType = (int)RailType.SAG;
                                            if (rail4.railType != rightRail.railType)
                                            {
                                                rail4.hasNewDirection = true;
                                            }
                                            break;
                                    }
                                }
                            }
                            else if (rightRail.railType == (int)RailType.SAG)
                            {
                                //  Ters yönde old. için 4 deffault rail'leri döndür.
                                if (InputManager.KeyPressed(Keys.R))
                                {
                                    rail_state = (rail_state % 4) + 1;

                                    if (rail_state > 4) // 1-4 arası tutuyorum. rail frame'leri
                                        rail_state = 1;

                                    blueprintRail.railType = rail_state - 1; // -1 for fitting rail rendering index
                                }
                                if (Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements &&
                                    !IsThereEntity(blueprintRail.Position))
                                {
                                    switch (rail_state)
                                    {
                                        case 1:
                                            Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail);
                                            blueprintRail.railType = (int)RailType.YUKARI;
                                            if (rail.railType != rightRail.railType)
                                            {
                                                rail.hasNewDirection = true;
                                            }
                                            break;
                                        case 2:
                                            Rail rail2 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SAG);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail2);
                                            blueprintRail.railType = (int)RailType.SAG;
                                            if (rail2.railType != rightRail.railType)
                                            {
                                                rail2.hasNewDirection = true;
                                            }
                                            break;
                                        case 3:
                                            Rail rail3 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SOL);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail3);
                                            blueprintRail.railType = (int)RailType.SOL;
                                            if (rail3.railType != rightRail.railType)
                                            {
                                                rail3.hasNewDirection = true;
                                            }
                                            break;
                                        case 4:
                                            Rail rail4 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail4);
                                            blueprintRail.railType = (int)RailType.ASAGI;
                                            if (rail4.railType != rightRail.railType)
                                            {
                                                rail4.hasNewDirection = true;
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements &&
                                        !IsThereEntity(blueprintRail.Position)) 
                                {
                                    if (blueprintRail.railType == (int)RailType.ASAGI)
                                    {
                                        Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                    new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                        //X_AXIS_LOCK = blueprintRail.Position.X;
                                        Globals.rails.Add(rail);
                                        if (rail.railType != rightRail.railType)
                                        {
                                            rail.hasNewDirection = true;
                                        }
                                    }
                                    else
                                    {
                                        Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                    new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                        //X_AXIS_LOCK = blueprintRail.Position.X;
                                        Globals.rails.Add(rail);
                                        if (rail.railType != rightRail.railType)
                                        {
                                            rail.hasNewDirection = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // LEFT
                    else if (IsThereLastRail(new Vector2(blueprintRail.Position.X - 64, blueprintRail.Position.Y)))
                    {
                        Rail gettingRailForInfo = GetRail(new Vector2(blueprintRail.Position.X - 64, blueprintRail.Position.Y));

                        {
                            if (gettingRailForInfo.railType == (int)RailType.SAG || gettingRailForInfo.railType == (int)RailType.SOL_UST_YUKARI ||
                            gettingRailForInfo.railType == (int)RailType.SOL_ALT_ASAGI)
                            {
                                if (InputManager.KeyPressed(Keys.R))
                                {
                                    rail_state = (rail_state % 4) + 1;

                                    if (rail_state > 4)
                                        rail_state = 1;

                                    switch (rail_state)
                                    {
                                        // 1 sag, 7 asagi, 4 yukari , 2 sol bunlar frame indexleri
                                        case 1:
                                            blueprintRail.railType = 1;
                                            break;
                                        case 2:
                                            blueprintRail.railType = 7;
                                            break;
                                        case 3:
                                            blueprintRail.railType = 4;
                                            break;
                                        case 4:
                                            blueprintRail.railType = 2;
                                            break;
                                    }
                                }
                                if (Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements &&
                                        !IsThereEntity(blueprintRail.Position))
                                {
                                    {
                                        //if (rail_state == 1)
                                        //{
                                        //    if (!lock_rail_direction)
                                        //    {
                                        //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                        //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SAG);
                                        //        Y_AXIS_LOCK = blueprintRail.Position.Y;
                                        //        rails.Add(rail);
                                        //
                                        //        lock_rail_direction = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!IsThereRail(new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK)))
                                        //        {
                                        //            Rail lockedRail = new("new_pistonlar", new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK),
                                        //                new Vector2(2f, 2f), Color.White, RailType.SAG);
                                        //
                                        //            rails.Add(lockedRail);
                                        //
                                        //            blueprintRail.railType = (int)RailType.SAG;
                                        //        }
                                        //    }
                                        //}
                                        //else if (rail_state == 2)
                                        //{
                                        //    if (!lock_rail_direction)
                                        //    {
                                        //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                        //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SAG_UST_ASAGI);
                                        //        Y_AXIS_LOCK = blueprintRail.Position.Y;
                                        //        rails.Add(rail);
                                        //
                                        //        lock_rail_direction = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!IsThereRail(new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK)))
                                        //        {
                                        //            Rail lockedRail = new("new_pistonlar", new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK),
                                        //                new Vector2(2f, 2f), Color.White, RailType.SAG_UST_ASAGI);
                                        //
                                        //            rails.Add(lockedRail);
                                        //
                                        //            blueprintRail.railType = (int)RailType.ASAGI;
                                        //        }
                                        //    }
                                        //}
                                        //else if (rail_state == 3)
                                        //{
                                        //    if (!lock_rail_direction)
                                        //    {
                                        //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                        //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SAG_ALT_YUKARI);
                                        //        Y_AXIS_LOCK = blueprintRail.Position.Y;
                                        //        rails.Add(rail);
                                        //
                                        //        lock_rail_direction = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!IsThereRail(new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK)))
                                        //        {
                                        //            Rail lockedRail = new("new_pistonlar", new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK),
                                        //                new Vector2(2f, 2f), Color.White, RailType.SAG_ALT_YUKARI);
                                        //
                                        //            rails.Add(lockedRail);
                                        //
                                        //            blueprintRail.railType = (int)RailType.YUKARI;
                                        //        }
                                        //    }
                                        //}
                                        //else if (rail_state == 4)
                                        //{
                                        //    if (!lock_rail_direction)
                                        //    {
                                        //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                        //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SOL);
                                        //        Y_AXIS_LOCK = blueprintRail.Position.Y;
                                        //        rails.Add(rail);
                                        //
                                        //        lock_rail_direction = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (!IsThereRail(new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK)))
                                        //        {
                                        //            Rail lockedRail = new("new_pistonlar", new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK),
                                        //                new Vector2(2f, 2f), Color.White, RailType.SOL);
                                        //
                                        //            rails.Add(lockedRail);
                                        //
                                        //            blueprintRail.railType = (int)RailType.SOL;
                                        //        }
                                        //    }
                                        //}
                                    }

                                    switch (rail_state)
                                    {
                                        // 1 sag, 7 asagi, 4 yukari , 2 sol bunlar frame indexleri
                                        case 1:
                                            Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SAG);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail);
                                            blueprintRail.railType = (int)RailType.SAG;
                                            if (rail.railType != gettingRailForInfo.railType)
                                            {
                                                rail.hasNewDirection = true;
                                            }
                                            break;
                                        case 2:
                                            Rail rail2 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SAG_UST_ASAGI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail2);
                                            blueprintRail.railType = (int)RailType.ASAGI;
                                            if (rail2.railType != gettingRailForInfo.railType)
                                            {
                                                rail2.hasNewDirection = true;
                                            }
                                            break;
                                        case 3:
                                            Rail rail3 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SAG_ALT_YUKARI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail3);
                                            blueprintRail.railType = (int)RailType.YUKARI;
                                            if (rail3.railType != gettingRailForInfo.railType)
                                            {
                                                rail3.hasNewDirection = true;
                                            }
                                            break;
                                        case 4:
                                            Rail rail4 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SOL);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail4);
                                            blueprintRail.railType = (int)RailType.SOL;
                                            if (rail4.railType != gettingRailForInfo.railType)
                                            {
                                                rail4.hasNewDirection = true;
                                            }
                                            break;
                                    }
                                }
                            }
                            else if (gettingRailForInfo.railType == (int)RailType.SOL)
                            {
                                //  Ters yönde old. için 4 deffault rail'leri döndür.
                                if (InputManager.KeyPressed(Keys.R))
                                {
                                    rail_state = (rail_state % 4) + 1;

                                    if (rail_state > 4) // 1-4 arası tutuyorum. rail frame'leri
                                        rail_state = 1;

                                    blueprintRail.railType = rail_state - 1; // -1 for fitting rail rendering index
                                }
                                if (Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements &&
                                        !IsThereEntity(blueprintRail.Position))
                                {
                                    switch (rail_state)
                                    {
                                        case 1:
                                            Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail);
                                            blueprintRail.railType = (int)RailType.YUKARI;
                                            if (rail.railType != gettingRailForInfo.railType)
                                            {
                                                rail.hasNewDirection = true;
                                            }
                                            break;
                                        case 2:
                                            Rail rail2 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SAG);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail2);
                                            blueprintRail.railType = (int)RailType.SAG;
                                            if (rail2.railType != gettingRailForInfo.railType)
                                            {
                                                rail2.hasNewDirection = true;
                                            }
                                            break;
                                        case 3:
                                            Rail rail3 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.SOL);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail3);
                                            blueprintRail.railType = (int)RailType.SOL;
                                            if (rail3.railType != gettingRailForInfo.railType)
                                            {
                                                rail3.hasNewDirection = true;
                                            }
                                            break;
                                        case 4:
                                            Rail rail4 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                            //X_AXIS_LOCK = blueprintRail.Position.X;
                                            Globals.rails.Add(rail4);
                                            blueprintRail.railType = (int)RailType.ASAGI;
                                            if (rail4.railType != gettingRailForInfo.railType)
                                            {
                                                rail4.hasNewDirection = true;
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements &&
                                        !IsThereEntity(blueprintRail.Position))
                                {
                                    if (blueprintRail.railType == (int)RailType.ASAGI)
                                    {
                                        Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                    new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                        //X_AXIS_LOCK = blueprintRail.Position.X;
                                        Globals.rails.Add(rail);
                                        if (rail.railType != gettingRailForInfo.railType)
                                        {
                                            rail.hasNewDirection = true;
                                        }
                                    }
                                    else
                                    {
                                        Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                                    new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                        //X_AXIS_LOCK = blueprintRail.Position.X;
                                        Globals.rails.Add(rail);
                                        if (rail.railType != gettingRailForInfo.railType)
                                        {
                                            rail.hasNewDirection = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // IF THE LAST BELT IS NOT NEIGHBOUR: DEFAULT ROTATIONS
                    else
                    {
                        //  Ters yönde old. için 4 deffault rail'leri döndür.
                        if (InputManager.KeyPressed(Keys.R))
                        {
                            rail_state = (rail_state % 4) + 1;

                            if (rail_state > 4) // 1-4 arası tutuyorum. rail frame'leri
                                rail_state = 1;

                            blueprintRail.railType = rail_state - 1; // -1 for fitting rail rendering index
                        }
                        if (Globals.Mouse.LeftButton == ButtonState.Released)
                        {
                            lock_rail_direction = false;
                            X_AXIS_LOCK = 0;
                            Y_AXIS_LOCK = 0;
                        }
                        if (Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements &&
                                !IsThereEntity(blueprintRail.Position))
                        {
                            {
                                //if (rail_state == 1)
                                //{
                                //    if (!lock_rail_direction)
                                //    {
                                //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                //        X_AXIS_LOCK = blueprintRail.Position.Y;
                                //        rails.Add(rail);
                                //
                                //        lock_rail_direction = true;
                                //    }
                                //    else
                                //    {
                                //        if (!IsThereRail(new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y)))
                                //        {
                                //            Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y),
                                //                new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                //
                                //            rails.Add(lockedRail);
                                //        }
                                //    }
                                //}
                                //else if (rail_state == 2)
                                //{
                                //    if (!lock_rail_direction)
                                //    {
                                //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SAG);
                                //        Y_AXIS_LOCK = blueprintRail.Position.Y;
                                //        rails.Add(rail);
                                //
                                //        lock_rail_direction = true;
                                //    }
                                //    else
                                //    {
                                //        if (!IsThereRail(new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK)))
                                //        {
                                //            Rail lockedRail = new("new_pistonlar", new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK),
                                //                new Vector2(2f, 2f), Color.White, RailType.SAG);
                                //
                                //            rails.Add(lockedRail);
                                //        }
                                //    }
                                //}
                                //else if (rail_state == 3)
                                //{
                                //    if (!lock_rail_direction)
                                //    {
                                //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SOL);
                                //        Y_AXIS_LOCK = blueprintRail.Position.Y;
                                //        rails.Add(rail);
                                //
                                //        lock_rail_direction = true;
                                //    }
                                //    else
                                //    {
                                //        if (!IsThereRail(new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK)))
                                //        {
                                //            Rail lockedRail = new("new_pistonlar", new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK),
                                //                new Vector2(2f, 2f), Color.White, RailType.SOL);
                                //
                                //            rails.Add(lockedRail);
                                //        }
                                //    }
                                //}
                                //else if (rail_state == 4)
                                //{
                                //    if (!lock_rail_direction)
                                //    {
                                //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                                //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                //        X_AXIS_LOCK = blueprintRail.Position.Y;
                                //        rails.Add(rail);
                                //
                                //        lock_rail_direction = true;
                                //    }
                                //    else
                                //    {
                                //        if (!IsThereRail(new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y)))
                                //        {
                                //            Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y),
                                //                new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                //
                                //            rails.Add(lockedRail);
                                //        }
                                //    }
                                //}
                            }

                            switch (rail_state)
                            {
                                case 1:
                                    blueprintRail.railType = (int)RailType.YUKARI;
                                    Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                        new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                    //X_AXIS_LOCK = blueprintRail.Position.X;

                                    rail.hasNewDirection = true;
                                    Globals.rails.Add(rail);

                                    break;
                                case 2:
                                    blueprintRail.railType = (int)RailType.SAG;
                                    Rail rail2 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                        new Vector2(2f, 2f), Color.White, RailType.SAG);
                                    //X_AXIS_LOCK = blueprintRail.Position.X;

                                    rail2.hasNewDirection = true;
                                    Globals.rails.Add(rail2);
                                    break;
                                case 3:
                                    blueprintRail.railType = (int)RailType.SOL;
                                    Rail rail3 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                        new Vector2(2f, 2f), Color.White, RailType.SOL);
                                    //X_AXIS_LOCK = blueprintRail.Position.X;

                                    rail3.hasNewDirection = true;
                                    Globals.rails.Add(rail3);
                                    break;
                                case 4:
                                    blueprintRail.railType = (int)RailType.ASAGI;
                                    Rail rail4 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                        new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                    //X_AXIS_LOCK = blueprintRail.Position.X;

                                    rail4.hasNewDirection = true;
                                    Globals.rails.Add(rail4);
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    //  Ters yönde old. için 4 deffault rail'leri döndür.
                    if (InputManager.KeyPressed(Keys.R))
                    {
                        rail_state = (rail_state % 4) + 1;

                        if (rail_state > 4) // 1-4 arası tutuyorum. rail frame'leri
                            rail_state = 1;

                        blueprintRail.railType = rail_state - 1; // -1 for fitting rail rendering index
                    }
                    if (Globals.Mouse.LeftButton == ButtonState.Released)
                    {
                        lock_rail_direction = false;
                        X_AXIS_LOCK = 0;
                        Y_AXIS_LOCK = 0;
                    }
                    if (Globals.Mouse.LeftButton == ButtonState.Pressed && !Globals.IsMouseEnteredGUIElements &&
                            !IsThereEntity(blueprintRail.Position))
                    {
                        {
                            //if (rail_state == 1)
                            //{
                            //    if (!lock_rail_direction)
                            //    {
                            //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                            //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                            //        X_AXIS_LOCK = blueprintRail.Position.Y;
                            //        rails.Add(rail);
                            //
                            //        lock_rail_direction = true;
                            //    }
                            //    else
                            //    {
                            //        if (!IsThereRail(new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y)))
                            //        {
                            //            Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y),
                            //                new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                            //
                            //            rails.Add(lockedRail);
                            //        }
                            //    }
                            //}
                            //else if (rail_state == 2)
                            //{
                            //    if (!lock_rail_direction)
                            //    {
                            //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                            //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SAG);
                            //        Y_AXIS_LOCK = blueprintRail.Position.Y;
                            //        rails.Add(rail);
                            //
                            //        lock_rail_direction = true;
                            //    }
                            //    else
                            //    {
                            //        if (!IsThereRail(new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK)))
                            //        {
                            //            Rail lockedRail = new("new_pistonlar", new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK),
                            //                new Vector2(2f, 2f), Color.White, RailType.SAG);
                            //
                            //            rails.Add(lockedRail);
                            //        }
                            //    }
                            //}
                            //else if (rail_state == 3)
                            //{
                            //    if (!lock_rail_direction)
                            //    {
                            //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                            //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.SOL);
                            //        Y_AXIS_LOCK = blueprintRail.Position.Y;
                            //        rails.Add(rail);
                            //
                            //        lock_rail_direction = true;
                            //    }
                            //    else
                            //    {
                            //        if (!IsThereRail(new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK)))
                            //        {
                            //            Rail lockedRail = new("new_pistonlar", new Vector2(blueprintRail.Position.X, Y_AXIS_LOCK),
                            //                new Vector2(2f, 2f), Color.White, RailType.SOL);
                            //
                            //            rails.Add(lockedRail);
                            //        }
                            //    }
                            //}
                            //else if (rail_state == 4)
                            //{
                            //    if (!lock_rail_direction)
                            //    {
                            //        Rail rail = new("new_pistonlar", new Vector2(blueprintRail.Position.X,
                            //            blueprintRail.Position.Y), new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                            //        X_AXIS_LOCK = blueprintRail.Position.Y;
                            //        rails.Add(rail);
                            //
                            //        lock_rail_direction = true;
                            //    }
                            //    else
                            //    {
                            //        if (!IsThereRail(new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y)))
                            //        {
                            //            Rail lockedRail = new("new_pistonlar", new Vector2(X_AXIS_LOCK, blueprintRail.Position.Y),
                            //                new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                            //
                            //            rails.Add(lockedRail);
                            //        }
                            //    }
                            //}
                        }

                        switch (rail_state)
                        {
                            case 1:
                                blueprintRail.railType = (int)RailType.YUKARI;
                                Rail rail = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                    new Vector2(2f, 2f), Color.White, RailType.YUKARI);
                                //X_AXIS_LOCK = blueprintRail.Position.X;

                                rail.hasNewDirection = true;
                                Globals.rails.Add(rail);
                                break;
                            case 2:
                                blueprintRail.railType = (int)RailType.SAG;
                                Rail rail2 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                    new Vector2(2f, 2f), Color.White, RailType.SAG);
                                //X_AXIS_LOCK = blueprintRail.Position.X;

                                rail2.hasNewDirection = true;
                                Globals.rails.Add(rail2);
                                break;
                            case 3:
                                blueprintRail.railType = (int)RailType.SOL;
                                Rail rail3 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                    new Vector2(2f, 2f), Color.White, RailType.SOL);
                                //X_AXIS_LOCK = blueprintRail.Position.X;

                                rail3.hasNewDirection = true;
                                Globals.rails.Add(rail3);
                                break;
                            case 4:
                                blueprintRail.railType = (int)RailType.ASAGI;
                                Rail rail4 = new("Textures/new_pistonlar", new Vector2(blueprintRail.Position.X, blueprintRail.Position.Y),
                                    new Vector2(2f, 2f), Color.White, RailType.ASAGI);
                                //X_AXIS_LOCK = blueprintRail.Position.X;

                                rail4.hasNewDirection = true;
                                Globals.rails.Add(rail4);
                                break;
                        }
                    }
                }

                // Check if the position has changed
                if (blueprintRail.Position != position)
                    rail_state = 1;

                blueprintRail.Position = position;
            }
        }

        public void DrawBlueprintRail()
        {
            if(activateRailPlacement)
            {
                blueprintRail.DrawRail();
            }
        }

        private bool IsThereEntity(Vector2 mousePos)
        {
            foreach (var rail in Globals.rails)
            {
                if (rail.Rectangle.Contains(mousePos))
                {
                    return true;
                }
            }
            foreach (var entity in Globals.entities)
            {
                if (entity.Rectangle.Contains(mousePos))
                {
                    // There is entity except if it is a mine. So we can deploy any entity on top of mines
                    if (entity.Tag != "Stone" && entity.Tag != "Iron" && entity.Tag != "Coal")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Checks if the last placed rail's position is the same as the parameter's position
        public bool IsThereLastRail(Vector2 position)
        {
            if(Globals.rails[Globals.rails.Count - 1].Position == position)
            {
                return true;
            }

            return false;
        }

        public Rail GetRail(Vector2 position)
        {
            foreach (var rail in Globals.rails)
            {
                if (rail.Position == position)
                {
                    return rail;
                }
            }
            return null;
        }
    }
}
