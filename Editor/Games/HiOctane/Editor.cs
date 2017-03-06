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
            ready = false;
            if (level != null) level.Unload();

            level = new Level(levelNumber);
            if (engine.SetScene(level))
            {
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

            engine.OnResize(clientRectangle, width, height);
            gui.Resize(width, height);
        }

        #region Mouse

        public override void MouseDown(MouseButton button)
        {
            if (button == MouseButton.Left) gui.WindowMouseDown();
        }

        public override void MouseUp(MouseButton button)
        {
            if (button == MouseButton.Right) Window.MouseWrap = false; //Window.ShowCursor();
        }

        public override void MouseMove(Vector2 mousePos, Vector2 mouseDelta)
        {
            if (!ready) return;

            if (!GUI.MouseUsed)
            {
                Vector2 delta = new Vector2(mouseDelta.X / (float)width, mouseDelta.Y / (float)height);

                if (Window.MouseRightDown)
                {
                    Window.MouseWrap = true;
                    level.Camera.MouseMove(delta);
                }

            }
        }

        public override void MouseWheel(float delta)
        {

        }
        #endregion

        #region Keyboard
        public override void KeyDown(Key key)
        {
            if (!ready) return;
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
        }
        #endregion
    }
}
