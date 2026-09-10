using System;
using System.Drawing;
using OpenTK;
using LevelEditor.Engine.GUI.Controls;

namespace LevelEditor.Games.HiOctane.Controls
{
    public class CameraToolbar : Panel
    {
        private readonly Label caption, detail;
        private readonly Button[] modes = new Button[3];
        private CameraController controller;

        public CameraToolbar(Action<CameraMode> changeMode, Action fit, Action help) : base(new Vector2(340, 92))
        {
            MouseEnabled = true;
            BackgroundColor = Color.FromArgb(37, 37, 38);
            BorderSize = 1;
            caption = new Label(new Vector2(324, 18), 9);
            caption.Pos = new Vector2(8, 70);
            AddChild(caption);
            string[] names = { "Fly F2", "Pan F3", "Top F4" };
            for (int i = 0; i < 3; i++)
            {
                CameraMode mode = (CameraMode)i;
                modes[i] = MakeButton(names[i], 6 + i * 112, 40, 104, delegate { changeMode(mode); });
            }
            MakeButton("Fit F", 6, 8, 80, fit);
            MakeButton("Help F1", 92, 8, 96, help);
            detail = new Label(new Vector2(138, 18), 9);
            detail.Pos = new Vector2(196, 12);
            AddChild(detail);
        }

        private Button MakeButton(string text, float x, float y, float width, Action action)
        {
            var button = new Button(new Vector2(width, 26), text, null, delegate { action(); });
            button.Pos = new Vector2(x, y);
            AddChild(button);
            return button;
        }

        public void SetCamera(CameraController camera) { controller = camera; Refresh(); }

        private void Refresh()
        {
            if (controller == null) return;
            caption.Text = controller.Mode == CameraMode.Pan ? "CAMERA | PAN: FIXED HEIGHT" :
                controller.Mode == CameraMode.TopDown ? "CAMERA | TOP-DOWN MAP" : "CAMERA | FREE FLIGHT";
            detail.Text = controller.Mode == CameraMode.TopDown ? "Span " + controller.Camera.OrthographicHeight.ToString("0.0") :
                "Height " + controller.Camera.Position.Y.ToString("0.0");
            for (int i = 0; i < modes.Length; i++)
                modes[i].BackgroundColor1 = (int)controller.Mode == i ? Config.UI_COLOR_BLUE : Color.FromArgb(45, 45, 48);
        }

        public override void Update(float time, float dTime) { Refresh(); base.Update(time, dTime); }
    }
}
