using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Misc;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using Sandbox.Engine;

namespace Sandbox.Oyun.GUI
{
    public class MainMenuGUI
    {
        readonly Panel mainPanel;

        readonly Panel secondPanel; // Adding Button inside of this...

        readonly Panel creditsPanel;

        Panel loadSavePanel;
        SquishingGroup saveButtonGroup;

        Button newGameButton;
        Button loadGameButton;
        Button settingsButton;
        Button creditsButton;
        Button exitButton;

        SoundEffect soundEffect;

        public bool isHidden = true;

        public bool newGameButtonPressed = false;

        //== SAVE PANEL ELEMENTS ==//

        Button save1Button;
        Button save1ButtonEditText;
        Text save1ButtonText;
        bool isSave1Full = false;

        Button save2Button;
        Button save2ButtonEditText;
        Text save2ButtonText;
        bool isSave2Full = false;

        Button save3Button;
        Button save3ButtonEditText;
        Text save3ButtonText;
        bool isSave3Full = false;


        //========================//

        public MainMenuGUI(Game game1)
        {
            soundEffect = Globals.Content.Load<SoundEffect>("Audios/button_hover");

            mainPanel = new Panel(Anchor.Center, new Vector2(250, 400), Vector2.Zero, setHeightBasedOnChildren: false)
            {
                DrawColor = new Color(135, 135, 135)
            };
            secondPanel = new Panel(Anchor.Center, new Vector2(242, 395), Vector2.Zero, setHeightBasedOnChildren: false)
            {
                DrawColor = new Color(135, 135, 135)
            };
            creditsPanel = new Panel(Anchor.Center, new Vector2(242, 395), Vector2.Zero, setHeightBasedOnChildren: false)
            {
                IsHidden = true,
                DrawColor = new Color(135, 135, 135)
            };
            loadSavePanel = new Panel(Anchor.Center, new Vector2(242, 395), Vector2.Zero, setHeightBasedOnChildren: false)
            {
                IsHidden = true,
                DrawColor = new Color(135, 135, 135)
            };

            newGameButton = new Button(Anchor.AutoCenter, new Vector2(150, 50), "New Game")
            {
                OnPressed = element =>
                {
                    soundEffect.Play(0.1f, .4f, 0);
                    newGameButtonPressed = true;
                    //Globals.SceneManager.SwitchScene(Scenes.Gameplay, game1);
                },
                //MouseEnterAnimation = new UiAnimation(0.15, (a, e, p) => e.ScaleTransform(1 + Easings.OutSine(p) * 0.05F)),
                //MouseExitAnimation = new UiAnimation(0.15, (a, e, p) => e.ScaleTransform(1 + Easings.OutSine.ReverseOutput()(p) * 0.05F))
                //{
                //    Finished = (a, e) => e.Transform = Matrix.Identity
                //},
                PositionOffset = new Vector2(0, 20),
                OnMouseEnter = element => soundEffect.Play(0.1f, .6f, 0),
                NormalColor = Color.DarkGray,
            };

            loadGameButton = new Button(Anchor.AutoCenter, new Vector2(150, 50), "Load Save")
            {
                OnPressed = element =>
                {
                    soundEffect.Play(0.1f, .4f, 0);
                    loadSavePanel.IsHidden = false;
                },
                PositionOffset = new Vector2(0, 20),
                OnMouseEnter = element => soundEffect.Play(0.1f, .6f, 0),
                NormalColor = Color.DarkGray
            };

            settingsButton = new Button(Anchor.AutoCenter, new Vector2(150, 50), "Settings")
            {
                OnPressed = element =>
                {
                    soundEffect.Play(0.1f, .4f, 0);
                    WindowSettings.IsFullScreen(true);
                },
                PositionOffset = new Vector2(0, 20),
                OnMouseEnter = element => soundEffect.Play(0.1f, .6f, 0),
                NormalColor = Color.DarkGray
            };

            creditsButton = new Button(Anchor.AutoCenter, new Vector2(150, 50), "Credits")
            {
                OnPressed = element =>
                {
                    soundEffect.Play(0.1f, .4f, 0);
                    creditsPanel.IsHidden = false;
                },
                PositionOffset = new Vector2(0, 20),
                OnMouseEnter = element => soundEffect.Play(0.1f, .6f, 0),
                NormalColor = Color.DarkGray
            };

            exitButton = new Button(Anchor.AutoCenter, new Vector2(150, 50), "Exit")
            {
                //OnPressed = element => this.UiSystem.Remove("InfoBox"),
                OnPressed = element =>
                {
                    soundEffect.Play(0.1f, .4f, 0);
                    game1.Exit();
                },
                PositionOffset = new Vector2(0, 20),
                OnMouseEnter = element => soundEffect.Play(0.1f, .6f, 0),
                NormalColor = Color.DarkGray
            };

            secondPanel.AddChild(newGameButton);
            secondPanel.AddChild(loadGameButton);
            secondPanel.AddChild(settingsButton);
            secondPanel.AddChild(creditsButton);
            secondPanel.AddChild(exitButton);

            //== CREDITS PANEL ELEMENTS ==//
            {
                creditsPanel.AddChild(new Paragraph(Anchor.TopCenter, 1, "Developed by SBN", true)
                {
                    TextScale = .85f,
                    TextColor = Color.Orange,
                    PositionOffset = new Vector2(0, 40)
                });
                creditsPanel.AddChild(new Button(Anchor.BottomLeft, new Vector2(150, 50), "Back")
                {
                    //OnPressed = element => this.UiSystem.Remove("InfoBox"),
                    OnPressed = element =>
                    {
                        soundEffect.Play(0.1f, .4f, 0);
                        creditsPanel.IsHidden = true;
                    },
                    OnMouseEnter = element => soundEffect.Play(0.1f, .6f, 0),
                    NormalColor = Color.DarkGray
                });
                creditsPanel.AddChild(new Image(Anchor.Center, new Vector2(.75f, .75f),
                    new TextureRegion(Globals.Content.Load<Texture2D>("Textures/benim")), scaleToImage: false)
                {
                    //ImageScale = new Vector2(5f, 5f)
                });
            }

            //== LOAD SAVE PANEL ELEMENTS ==//
            {

                save1Button = new Button(Anchor.AutoInlineCenterIgnoreOverflow, new Vector2(235, 50), "SAVE 1")
                {
                    //OnPressed = element => this.UiSystem.Remove("InfoBox"),
                    OnPressed = element =>
                    {
                        soundEffect.Play(0.1f, .4f, 0);

                        //==TODO: Check if this button is true(boolean) then load the save ==//
                        if(!isSave1Full && Globals.entities.Count == 0)
                        {
                            Globals.entities = SaveManager.Load<Entity>("Saves/save1.json");

                            isSave1Full = true;
                        }
                    },
                    OnMouseEnter = element => soundEffect.Play(0.1f, .6f, 0),
                    NormalColor = Color.DarkGray
                };
                //save1ButtonText = new Text();
                loadSavePanel.AddChild(save1Button);

                save2Button = new Button(Anchor.AutoInlineCenterIgnoreOverflow, new Vector2(235, 50), "SAVE 2")
                {
                    //OnPressed = element => this.UiSystem.Remove("InfoBox"),
                    OnPressed = element =>
                    {
                        soundEffect.Play(0.1f, .4f, 0);
                    },
                    OnMouseEnter = element => soundEffect.Play(0.1f, .6f, 0),
                    NormalColor = Color.DarkGray,
                    PositionOffset = new Vector2(-235, 65)
                };
                loadSavePanel.AddChild(save2Button);

                save3Button = new Button(Anchor.AutoInlineCenterIgnoreOverflow, new Vector2(235, 50), "SAVE 3")
                {
                    //OnPressed = element => this.UiSystem.Remove("InfoBox"),
                    OnPressed = element =>
                    {
                        soundEffect.Play(0.1f, .4f, 0);
                    },
                    OnMouseEnter = element => soundEffect.Play(0.1f, .6f, 0),
                    NormalColor = Color.DarkGray,
                    PositionOffset = new Vector2(-235, 65)
                };
                loadSavePanel.AddChild(save3Button);

                loadSavePanel.AddChild(new Button(Anchor.BottomLeft, new Vector2(150, 50), "Back")
                {
                    //OnPressed = element => this.UiSystem.Remove("InfoBox"),
                    OnPressed = element =>
                    {
                        soundEffect.Play(0.1f, .4f, 0);
                        loadSavePanel.IsHidden = true;
                    },
                    OnMouseEnter = element => soundEffect.Play(0.1f, .6f, 0),
                    NormalColor = Color.DarkGray
                });
            }

            mainPanel.AddChild(secondPanel);
            mainPanel.AddChild(creditsPanel);

            mainPanel.AddChild(loadSavePanel);
        }

        public void InputChecks()
        {
            if (InputManager.KeyPressed(Keys.Escape))
            {
                if (!creditsPanel.IsHidden)
                {
                    exitButton.IsHidden = false;
                    creditsPanel.IsHidden = true;
                }
                soundEffect.Play(0.1f, .4f, 0);
            }
        }

        public void ShowMainMenuGUI()
        {
            mainPanel.IsHidden = isHidden;
        }

        public Element GetElementGUI()
        {
            return mainPanel;
        }
    }
}
