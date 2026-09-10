using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LevelEditor
{
    // OpenTK 1.x does not expose WM_DPICHANGED. Keep the native window and the
    // custom OpenGL UI in sync without changing the scene's pixel coordinates.
    internal sealed class WindowDpi : NativeWindow
    {
        private readonly Action<float, Rectangle> changed;

        public WindowDpi(IntPtr handle, Action<float, Rectangle> changed)
        {
            this.changed = changed;
            AssignHandle(handle);
        }

        public static float GetScale(IntPtr handle)
        {
            try
            {
                uint dpi = GetDpiForWindow(handle);
                if (dpi != 0) return dpi / 96f;
            }
            catch (EntryPointNotFoundException)
            {
                // Older Windows versions use the system-aware manifest fallback.
            }

            using (Graphics graphics = Graphics.FromHwnd(handle))
                return graphics.DpiX / 96f;
        }

        protected override void WndProc(ref Message message)
        {
            const int WmDpiChanged = 0x02E0;
            if (message.Msg == WmDpiChanged)
            {
                NativeRectangle rect = (NativeRectangle)Marshal.PtrToStructure(
                    message.LParam, typeof(NativeRectangle));
                float scale = (message.WParam.ToInt64() & 0xffff) / 96f;
                changed(scale, Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom));
                message.Result = IntPtr.Zero;
                return;
            }
            base.WndProc(ref message);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NativeRectangle
        {
            public int Left, Top, Right, Bottom;
        }

        [DllImport("user32.dll")]
        private static extern uint GetDpiForWindow(IntPtr window);
    }
}
