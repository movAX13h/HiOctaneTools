using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using LevelEditor.Engine.GUI.Controls;

namespace LevelEditor.Games.HiOctane.Controls
{
    public class HelpDialog : Panel
    {
        private readonly FloatPanel dialog;
        private readonly Label introduction;
        private readonly List<Label> keys = new List<Label>();
        private readonly List<Label> descriptions = new List<Label>();
        private readonly Button[] tabs = new Button[3];
        private readonly Button closeButton;
        private readonly Action close;
        private readonly string[][] content = {
            new[] {
                "F2 / F3 / F4", "Fly / constant-height Pan / Top-down map",
                "WASD / Arrows", "Move. Pan keeps the camera at the same height.",
                "Right drag", "Look in Fly; move the map in Pan and Top-down.",
                "Alt + right drag", "Look around in Pan without changing height.",
                "Q / E", "Lower / raise. In Top-down: zoom out / in.",
                "Mouse wheel", "Fly: dolly. Pan: height. Top-down: zoom.",
                "Shift", "Hold for faster keyboard movement.",
                "F", "Fit the whole map in the current camera view."
            },
            new[] {
                "1 / 2 / 3", "Inspect / Textures / Terrain editing modes",
                "4 / 5 / 6 / 7", "Buildings / Walls / Waypoints / Actions",
                "Textures", "Select a tile, then left-drag on terrain to paint.",
                "Rotate / Flip", "Change the selected texture's orientation.",
                "Pick / Middle", "Sample a terrain tile. Undo restores one stroke.",
                "Terrain", "Choose a brush and lower, raise, or flatten.",
                "LEVEL > Save", "Save changes to a level file.",
                "Availability", "Column textures and unfinished modes are limited."
            },
            new[] {
                "F1", "Open or close this help dialog.",
                "Escape", "Close help or menus. Use PROGRAM > Exit to quit.",
                "Tab", "Show or hide the editor interface.",
                "Backspace", "Toggle fullscreen.",
                "Insert", "Toggle VSync.",
                "Home", "Show or hide the performance profiler.",
                "Window controls", "Use the top-right buttons to minimize or close.",
                "Windows scaling", "Layout follows each monitor's display scale."
            }
        };

        public HelpDialog(Action close) : base(new Vector2(800, 600))
        {
            this.close = close;
            Visible = false;
            MouseEnabled = true;
            Alpha = 0.75f;
            BackgroundColor = Color.Black;
            dialog = new FloatPanel(new Vector2(620, 420), "HELP & CONTROLS", false, false);
            AddChild(dialog);
            introduction = new Label(new Vector2(596, 20), 9);
            introduction.Pos.X = 12;
            dialog.AddChild(introduction);
            string[] names = { "Camera", "Editing", "Shortcuts" };
            for (int i = 0; i < tabs.Length; i++)
            {
                int page = i;
                tabs[i] = new Button(new Vector2(192, 26), names[i], null, delegate { ShowPage(page); });
                tabs[i].Pos.X = 12 + 202 * i;
                dialog.AddChild(tabs[i]);
            }
            for (int i = 0; i < 8; i++)
            {
                var key = new Label(new Vector2(140, 20), 9);
                key.TextColor = Color.FromArgb(80, 185, 255);
                key.Pos.X = 12;
                dialog.AddChild(key);
                keys.Add(key);
                var description = new Label(new Vector2(456, 20), 9);
                description.Pos.X = 154;
                dialog.AddChild(description);
                descriptions.Add(description);
            }
            closeButton = new Button(new Vector2(150, 28), "Close (Esc)", null, delegate { this.close(); });
            closeButton.Pos.Y = 12;
            dialog.AddChild(closeButton);
            ShowPage(0);
            Resize(800, 600);
        }

        public void Open()
        {
            ResetInteraction();
            ShowPage(0);
            Visible = true;
        }

        private void ShowPage(int page)
        {
            introduction.Text = page == 0 ? "Camera controls work in every editor mode." :
                page == 1 ? "Navigation stays the same when you switch editing tools." : "Application and window shortcuts";
            for (int i = 0; i < 8; i++)
            {
                keys[i].Text = content[page][2 * i];
                descriptions[i].Text = content[page][2 * i + 1];
            }
            for (int i = 0; i < tabs.Length; i++)
                tabs[i].BackgroundColor1 = i == page ? Config.UI_COLOR_BLUE : Color.FromArgb(45, 45, 48);
        }

        public override void Resize(float width, float height)
        {
            base.Resize(width, height);
            dialog.Resize(620, Math.Min(420, Math.Max(324, height - 64)));
            dialog.Pos = new Vector2((float)Math.Floor((width - dialog.Size.X) / 2), (float)Math.Floor((height - dialog.Size.Y) / 2));
            introduction.Pos.Y = dialog.Size.Y - 48;
            foreach (Button tab in tabs) tab.Pos.Y = dialog.Size.Y - 82;
            float rowHeight = (dialog.Size.Y - 146) / 8;
            for (int i = 0; i < 8; i++) keys[i].Pos.Y = descriptions[i].Pos.Y = dialog.Size.Y - 111 - rowHeight * i;
            closeButton.Pos.X = (dialog.Size.X - closeButton.Size.X) / 2;
        }
    }
}
