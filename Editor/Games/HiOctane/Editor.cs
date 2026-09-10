using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

using OpenTK.Input;
using OpenTK;

using LevelEditor.Engine;
using LevelEditor.Engine.Core;
using LevelEditor.Utils;
using LevelEditor.Engine.GUI.Controls;

namespace LevelEditor.Games.HiOctane
{
    public class Editor : IGame
    {
        #region statics
        public static float Time { get; private set; }
        public static Profiler Profiler { get; private set; }
        #endregion

        private EngineBase engine;
        private Level level;
        private GUI gui;

        private int width;
        private int height;
        private bool cameraDragging;

        public override bool Ready { get { return ready; } }
        private bool ready;

        public Editor(int startLevelNumber)
        {
            ready = false;

            // setup engine
            engine = new EngineBase();

            if (engine.Ready)
            {
                engine.ClearColor(Color.Black);

                Profiler = new Engine.GUI.Controls.Profiler();
                Profiler.Visible = false;

                // setup gui
                gui = new GUI(this);
                if (!gui.Ready)
                {
                    Log.WriteLine(Log.LOG_ERROR, "failed to setup GUI");
                    return;
                }

                engine.SetOverlay(gui);

                // setup level
                LoadLevel(startLevelNumber);
            }
        }

        public bool LoadLevel(int levelNumber)
        {
            CameraMode cameraMode = level == null ? CameraMode.Pan : level.Camera.Mode;
            ready = false;
            ResetNavigation();
            // Detach reusable mode cursors before unloading the old scene's GPU resources.
            gui.DeactivateModes();
            if (level != null) level.Unload();

            level = new Level(levelNumber);
            if (engine.SetScene(level))
            {
                level.Camera.AspectRatio = width / (float)Math.Max(1, height);
                level.Camera.SetMode(cameraMode);
                if (level.Camera.Camera != null) engine.SetCamera(level.Camera.Camera);

                Log.WriteLine(Log.LOG_INFO, "level " + levelNumber + " setup done" + Environment.NewLine +
                    "GRAPH" + Environment.NewLine + level.GraphString());

                gui.SetLevel(level);

                Time = 0;
                ready = true;
                return true;
            }
            else
            {
                Log.WriteLine(Log.LOG_ERROR, "level " + levelNumber + "setup failed");
                return false;
            }
        }

        public override void Update(float dTime)
        {
            if (!ready) return;

            Time += dTime;
            Profiler.Begin("UPDATE", true, 10);
            gui.Update(Time, dTime);
            if (gui.HasModalDialog) level.Camera.ResetInput();
            level.Update(dTime);
            Profiler.End("UPDATE");
        }

        public override void Render()
        {
            if (!ready) return;
            Profiler.Begin("RENDER", true, 10);
            engine.Render();
            Profiler.End("RENDER");
        }

        public override void OnResize(Rectangle clientRectangle, int width, int height)
        {
            this.width = width;
            this.height = height;
            if (level != null) level.Camera.AspectRatio = width / (float)Math.Max(1, height);

            engine.OnResize(clientRectangle, width, height);
            gui.Resize(width / Window.UiScale, height / Window.UiScale);
        }

        #region Mouse

        public override void MouseDown(MouseButton button)
        {
            if (!ready) return;
            if (button == MouseButton.Left) gui.WindowMouseDown();
            if (button == MouseButton.Right && !gui.HasModalDialog && !gui.PointerOverUI && !Window.MouseLeftDown)
            {
                cameraDragging = true;
                Window.MouseWrap = true;
                gui.SuspendEditingUntilRelease();
            }
        }

        public override void MouseUp(MouseButton button)
        {
            if (button == MouseButton.Right) { cameraDragging = false; Window.MouseWrap = false; }
        }

        public override void MouseMove(Vector2 mousePos, Vector2 mouseDelta)
        {
            if (!ready) return;

            if (cameraDragging && Window.MouseRightDown && !gui.HasModalDialog)
                level.Camera.MouseMove(mouseDelta / Math.Max(1, height));
        }

        public override void MouseWheel(float delta)
        {
            if (ready && !gui.HasModalDialog && !gui.PointerOverUI) level.Camera.Zoom(delta);
        }
        #endregion

        #region Keyboard
        public override void KeyDown(Key key)
        {
            if (!ready) return;
            if (key == Key.F1) { gui.ToggleHelp(); return; }
            if (key == Key.Escape) { gui.Escape(); return; }
            if (gui.HasModalDialog) return;
            if (key == Key.F2) { SetCameraMode(CameraMode.Fly); return; }
            if (key == Key.F3) { SetCameraMode(CameraMode.Pan); return; }
            if (key == Key.F4) { SetCameraMode(CameraMode.TopDown); return; }
            if (key == Key.F) { FitMap(); return; }
            level.KeyDown(key);
            level.Camera.KeyDown(key);
            gui.KeyDown(key);
            if (key == Key.Tab) gui.Visible = !gui.Visible;
            if (key == Key.Home) Profiler.Visible = !Profiler.Visible;
        }

        public override void KeyUp(Key key)
        {
            if (!ready) return;
            level.KeyUp(key);
            level.Camera.KeyUp(key);
            if (gui.HasModalDialog) return;
            if (key == Key.BackSpace) Window.ToggleFullscreen();
            if (key == Key.Insert) Window.SetVSync(!Window.IsVSync);
        }

        public void SetCameraMode(CameraMode mode)
        {
            if (!ready || gui.HasModalDialog) return;
            ResetNavigation();
            Vector3 position = level.Camera.Camera.Position;
            Vector3 direction = (level.Camera.Camera.LookAt - position).Normalized();
            var hit = level.Collisions.RayCast(position, direction);
            level.Camera.SetMode(mode, hit.Hit ? (Vector3?)hit.Position : null);
            gui.SuspendEditingUntilRelease();
        }

        public void FitMap()
        {
            if (!ready || gui.HasModalDialog) return;
            ResetNavigation();
            level.Camera.FitMap();
            gui.SuspendEditingUntilRelease();
        }

        public void ResetNavigation()
        {
            cameraDragging = false;
            Window.MouseWrap = false;
            if (level != null && level.Camera != null) level.Camera.ResetInput();
        }

        public override void FocusLost()
        {
            ResetNavigation();
            if (gui != null) { gui.ResetInteraction(); gui.SuspendEditingUntilRelease(); }
        }
        #endregion
    }
}
