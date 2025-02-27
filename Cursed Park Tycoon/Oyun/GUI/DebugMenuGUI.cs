using MLEM.Ui.Elements;
using MLEM.Ui;
using Microsoft.Xna.Framework;
using Sandbox.Engine;
using System.Collections.Generic;

namespace Sandbox.Oyun.GUI
{
    public class DebugMenuGUI
    {
        bool isFullscreen = false;

        readonly Panel mainPanel;
        Paragraph fps_text;
        Paragraph world_pos_text;

        public DebugMenuGUI(int fps_value, Vector2 mouse_world_pos, List<Rail> rails) 
        {
            mainPanel = new Panel(Anchor.TopCenter, new Vector2(650, 100), Vector2.Zero, setHeightBasedOnChildren: true)
            {
                DrawColor = new Color(135, 135, 135)
            };

            fps_text = new Paragraph(Anchor.CenterLeft, 1, "FPS: " + fps_value)
            {
                PositionOffset = new Vector2(20, 0),
                TextScale = .70f
            };
            world_pos_text = new Paragraph(Anchor.CenterRight, 1,
                "(World Grid) " + "X: " + (int)mouse_world_pos.X
                    + " Y: " + (int)mouse_world_pos.Y)
            {
                PositionOffset = new Vector2(-150, 0),
                TextScale = .70f
            };
            mainPanel.AddChild(new Checkbox(Anchor.CenterRight, new Vector2(0.5F, 0.5F), "Fullscreen", false)
            {
                OnPressed = element =>
                {
                    isFullscreen = !isFullscreen;
                    WindowSettings.IsFullScreen(isFullscreen);
                },
                PositionOffset = new Vector2(-100, 0),
            });
            mainPanel.AddChild(new Button(Anchor.CenterRight, new Vector2(0.1F, 60), "Reset", "Clear Scene")
            {
                //OnPressed = element => this.UiSystem.Remove("InfoBox"),
                OnPressed = element =>
                {
                    DeleteAll(rails);
                },
                PositionOffset = new Vector2(20, 0)
            });


            mainPanel.AddChild(fps_text);
            mainPanel.AddChild(world_pos_text);;

        }

        private static void DeleteAll(List<Rail> rails)
        {
            foreach(var entity in Globals.entities)
            {
                entity.CleanUp();
            }
            Globals.entities.Clear();
            rails.Clear();
        }

        public Element GetElementGUI()
        {
            return mainPanel;
        }

        public void UpdateDebugMenuText(int fps_value, Vector2 mouse_world_pos)
        {
            fps_text.Text = "FPS: " + fps_value;
            world_pos_text.Text = "(World Grid) " + "X: " + (int)mouse_world_pos.X
                    + " Y: " + (int)mouse_world_pos.Y;
        }
    }
}
