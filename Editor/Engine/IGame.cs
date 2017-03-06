using System.Drawing;
using System.Collections.Generic;

using OpenTK.Input;
using OpenTK;

namespace LevelEditor.Engine
{
    public abstract class IGame
    {
        abstract public bool Ready { get; }

        abstract public void Update(float time);
        abstract public void Render();

        abstract public void OnResize(Rectangle clientRectangle, int width, int height);

        abstract public void MouseDown(MouseButton button);
        abstract public void MouseUp(MouseButton button);
        abstract public void MouseMove(Vector2 mousePos, Vector2 mouseDelta);
        abstract public void MouseWheel(float delta);

        abstract public void KeyDown(Key key);
        abstract public void KeyUp(Key key);
    }
}
