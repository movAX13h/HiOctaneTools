using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

using LevelEditor.Games;
using LevelEditor.Utils;
using LevelEditor.Engine;
using LevelEditor.Games.HiOctane;

namespace LevelEditor
{
    class Window : GameWindow
    {
        #region Program entry point
        [STAThread]
        static void Main(string[] args)
        {
            int level = Config.DEFAULT_LEVEL_NUMBER;

            if (args.Length > 0)
            {
                if (int.TryParse(args[0], out level))
                {
                    level = Math.Min(9, level);
                }
            }

            using (Window window = new Window(level))
            {
                window.Run(60.0);
            }
        }
        #endregion

        #region statics
        private static Window instance;

        //public static Vector2 Center;

        public static bool IsVSync { get; private set; }
        public static bool IsFullscreen { get; private set; }
        public static bool IsCursorVisible { get { return instance.CursorVisible; } }

        public static int RenderFPS { get; private set; }
        public static double UpdateDuration { get; private set; }
        public static double RenderDuration { get; private set; }

        public static bool MouseLeftDown { get; private set; }
        public static bool MouseRightDown { get; private set; }
        public static bool MouseMiddleDown { get; private set; }

        public static float MouseWheelDelta { get; private set; }
        public static bool MouseMoved { get; private set; }

        public static bool MouseWrap = false;

        public static Vector2 MousePos { get; private set; }
        public static Vector2 MousePosLast { get; private set; }
        public static Vector2 MouseDelta { get; private set; }

        public static void ToggleFullscreen()
        {
            instance.toggleFullscreen();
        }

        public static void Minimize()
        {
            instance.minimize();
        }

        public static void HideCursor()
        {
            if (instance.CursorVisible == false) return;
            instance.CursorVisible = false;
            instance.CenterCursor();
            MouseDelta = Vector2.Zero;
        }

        public static void ShowCursor()
        {
            instance.CursorVisible = true;
        }

        public static void Terminate()
        {
            instance.Exit();
        }

        public static string GetTitle() { return instance.Title; }

        public static void SetTitleAppendix(string title)
        {
            instance.Title = Application.ProductName + " - " + title;
        }

        public static void Drag(Vector2 delta)
        {
            if (IsFullscreen) return;

            instance.X += (int)delta.X;
            instance.Y -= (int)delta.Y;
            MousePos -= delta;
        }

        public static void ResizeDrag()
        {
            Size s = new Size((int)MousePos.X + 8, instance.Height - (int)MousePos.Y + 8);
            if (s.Width < 650) s.Width = 650;
            if (s.Height < 400) s.Height = 400;
            instance.Size = s;
        }

        public static void SetVSync(bool state)
        {
            instance.VSync = state ? VSyncMode.On : VSyncMode.Off;
            IsVSync = state;
        }

        //public static void Width

        #endregion

        private IGame editor;

        private int startLevel;
        public Point center;

        private int renderFrameCount = 0;
        private double time = 0;
        private double prevRenderTime = 0;

        public Window(int level) : base(1200, 660, new GraphicsMode(32, 24, 8, 4)) // 32bpp color, 24bpp z-depth, 8bpp stencil and 4x antialiasing
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            WindowBorder = WindowBorder.Hidden;

            instance = this;
            startLevel = level;

            Title = Application.ProductName + " " + Application.ProductVersion.Replace(".0", "");
            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            IsVSync = false;
            IsFullscreen = false;

            RenderFPS = 0;
            UpdateDuration = 0;
            RenderDuration = 0;

            MouseLeftDown = false;
            MouseRightDown = false;
            MouseMiddleDown = false;
            MouseWheelDelta = 0;
            MouseMoved = false;
            MousePos = Vector2.Zero;
            MousePosLast = Vector2.Zero;
            MouseDelta = Vector2.Zero;
        }

        #region Window/Canvas Event Handlers
        protected override void OnLoad(EventArgs e)
        {
            VSync = VSyncMode.Off;
            IsVSync = false;

            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;

            editor = new Games.HiOctane.Editor(startLevel);
            if (!editor.Ready)
            {
                MessageBox.Show("Check logfile!");
                Exit();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            CursorVisible = true;
            base.OnClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            recalculateCenter();
            if (editor.Ready) editor.OnResize(ClientRectangle, Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            UpdateDuration = e.Time;
            editor.Update((float)e.Time);
            MouseWheelDelta = 0;
            MouseMoved = false;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            RenderDuration = e.Time;
            time += e.Time;

            // fps
            renderFrameCount++;
            if (Math.Floor(time) != Math.Floor(prevRenderTime))
            {
                RenderFPS = renderFrameCount;
                renderFrameCount = 0;
            }
            prevRenderTime = time;


            editor.Render();
            SwapBuffers();
        }

        protected void recalculateCenter()
        {
            //Center = new Vector2((float)Math.Floor(0.5f * Width), (float)Math.Floor(0.5f * Height));
            //center = new Point((int)Center.X, (int)Center.Y);
            center = new Point((int)Math.Floor(0.5f * Width), (int)Math.Floor(0.5f * Height));
        }

        protected void minimize()
        {
            WindowState = WindowState.Minimized;
        }

        protected void toggleFullscreen()
        {
            if (WindowState == WindowState.Fullscreen)
            {
                //WindowBorder = WindowBorder.Resizable;
                IsFullscreen = false;
                WindowState = WindowState.Normal;
            }
            else
            {
                //WindowBorder = WindowBorder.Hidden;
                IsFullscreen = true;
                WindowState = WindowState.Fullscreen;
            }
        }
        #endregion

        #region Keyboard/Mouse events
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    MouseLeftDown = true;
                    break;
                case MouseButton.Right:
                    MouseRightDown = true;
                    break;
                case MouseButton.Middle:
                    MouseMiddleDown = true;
                    break;
            }

            editor.MouseDown(e.Button);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    MouseLeftDown = false;
                    //CursorVisible = true;
                    break;
                case MouseButton.Right:
                    MouseRightDown = false;
                    break;
                case MouseButton.Middle:
                    MouseMiddleDown = false;
                    break;
            }

            editor.MouseUp(e.Button);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            MouseMoved = true;
            MousePosLast = MousePos;
            MousePos = new Vector2(e.X, Height - e.Y);
            MouseDelta = new Vector2(e.XDelta, -e.YDelta); //new Vector2(MousePos.X - MousePosLast.X, MousePos.Y - MousePosLast.Y);

            if (MouseDelta.Length > Height * 0.5f)
            {
                MouseDelta = Vector2.Zero;
                return;
            }

            editor.MouseMove(MousePos, MouseDelta);

            if (MouseWrap)
            {
                Vector2 mp = MousePos;
                if (mp.X > Width) mp.X -= Width;
                if (mp.X < 1) mp.X += Width - 1;

                if (GUI.IsVisible)
                {
                    if (mp.Y > Height - 32) mp.Y -= Height - 52;
                    if (mp.Y < 20) mp.Y += Height - 52;
                }
                else
                {
                    if (mp.Y > Height - 1) mp.Y -= Height - 1;
                    if (mp.Y < 1) mp.Y += Height - 1;
                }

                if (mp != MousePos)
                {
                    Point p = PointToScreen(new Point((int)mp.X, Height - (int)mp.Y));
                    OpenTK.Input.Mouse.SetPosition(p.X, p.Y);
                    MousePos = mp;
                }
            }
            MouseDelta = Vector2.Zero;
        }

        public void CenterCursor()
        {
            MousePos = new Vector2(center.X, center.Y);
            Point screenCenter = PointToScreen(center);
            OpenTK.Input.Mouse.SetPosition(screenCenter.X, screenCenter.Y);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            MouseWheelDelta = e.DeltaPrecise;
            editor.MouseWheel(e.DeltaPrecise);
            base.OnMouseWheel(e);
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs key)
        {
            switch (key.Key)
            {
                case Key.Escape: Exit(); break;
                default: editor.KeyDown(key.Key); break;
            }
        }

        private void OnKeyUp(object sender, KeyboardKeyEventArgs key)
        {
            switch (key.Key)
            {
                case Key.BackSpace: toggleFullscreen(); break;
                case Key.Insert: SetVSync(!IsVSync); break;
                default: editor.KeyUp(key.Key); break;
            }

        }
        #endregion



    }
}
