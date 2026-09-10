using LevelEditor.Engine.Cameras;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace LevelEditor.Games.HiOctane
{
    public enum CameraMode { Fly, Pan, TopDown }

    public class CameraController
    {
        public float MoveSpeed = 18f;
        public float MoveSpeedFast = 60f;
        public float MouseSpeed = 3f;
        public FpsCamera Camera { get; private set; }
        public CameraMode Mode { get; private set; }
        public float AspectRatio = 16f / 9f;

        private readonly HashSet<Key> keys = new HashSet<Key>();
        private Vector3 mapSize = new Vector3(256, 40, 160);
        private Vector3 perspectiveDirection, perspectivePosition, topDownAnchor;
        private bool animate;
        private Vector3 animationPosition, animationLookAt;

        public CameraController(Vector3 position, Vector3 lookAt)
        {
            Camera = new FpsCamera(position, lookAt);
            Mode = CameraMode.Pan;
        }

        public void SetMapSize(Vector3 size) { mapSize = size; }

        public void ResetInput()
        {
            keys.Clear();
            animate = false;
        }

        private Vector3 Direction { get { return (Camera.LookAt - Camera.Position).Normalized(); } }

        private Vector3 HorizontalForward
        {
            get
            {
                if (Mode == CameraMode.TopDown) return -Vector3.UnitZ;
                Vector3 forward = Direction;
                forward.Y = 0;
                return forward.LengthSquared > 0.0001f ? forward.Normalized() : -Vector3.UnitZ;
            }
        }

        public void SetMode(CameraMode mode, Vector3? focus = null)
        {
            if (Mode == mode) return;
            ResetInput();
            if (mode == CameraMode.TopDown)
            {
                perspectiveDirection = Direction;
                perspectivePosition = Camera.Position;
                Vector3 center = focus ?? Camera.Position + HorizontalForward * 25;
                topDownAnchor = new Vector3(center.X, 0, center.Z);
                float distance = Math.Max(8, (Camera.Position - center).Length);
                Camera.OrthographicHeight = Math.Max(8, Math.Min(512, distance * 1.1547f));
                Camera.Up = -Vector3.UnitZ;
                Camera.Relocate(new Vector3(center.X, Math.Max(mapSize.Y + 50, Camera.Position.Y), center.Z),
                    new Vector3(center.X, 0, center.Z));
            }
            else if (Mode == CameraMode.TopDown)
            {
                // Preserve the area reached while panning the map, and restore its 3D heading.
                Vector3 position = perspectivePosition + new Vector3(Camera.Position.X - topDownAnchor.X, 0, Camera.Position.Z - topDownAnchor.Z);
                Camera.Up = Vector3.UnitY;
                Camera.OrthographicHeight = 0;
                Camera.Relocate(position, position + perspectiveDirection);
            }
            Mode = mode;
        }

        public void FitMap()
        {
            ResetInput();
            Vector3 center = new Vector3(mapSize.X * 0.5f, 0, mapSize.Z * 0.5f);
            float span = Math.Max(mapSize.Z, mapSize.X / Math.Max(0.1f, AspectRatio)) * 1.15f;
            if (Mode == CameraMode.TopDown)
            {
                Camera.OrthographicHeight = span;
                Camera.Relocate(new Vector3(center.X, mapSize.Y + 100, center.Z), center);
            }
            else
            {
                float distance = Math.Max(span, mapSize.Y + 30);
                Vector3 position = center + new Vector3(0, distance, distance * 0.65f);
                Camera.Relocate(position, center);
            }
        }

        internal void AnimateTo(Vector3 target, float distance)
        {
            ResetInput();
            if (Mode == CameraMode.TopDown)
            {
                animationPosition = new Vector3(target.X, Camera.Position.Y, target.Z);
                animationLookAt = new Vector3(target.X, 0, target.Z);
            }
            else if (Mode == CameraMode.Pan)
            {
                animationPosition = target - HorizontalForward * distance;
                animationPosition.Y = Camera.Position.Y;
                animationLookAt = animationPosition + Direction;
            }
            else
            {
                animationPosition = target - Direction * distance;
                animationLookAt = target;
            }
            animate = true;
        }

        public void Update(float dTime)
        {
            if (dTime <= 0) return;
            if (animate)
            {
                float fraction = 1 - (float)Math.Exp(-6 * dTime);
                Camera.Relocate(Vector3.Lerp(Camera.Position, animationPosition, fraction),
                    Vector3.Lerp(Camera.LookAt, animationLookAt, fraction));
                if ((Camera.Position - animationPosition).Length < 0.05f) animate = false;
                return;
            }
            float forwardAmount = (Down(Key.W, Key.Up) ? 1 : 0) - (Down(Key.S, Key.Down) ? 1 : 0);
            float rightAmount = (Down(Key.D, Key.Right) ? 1 : 0) - (Down(Key.A, Key.Left) ? 1 : 0);
            float verticalAmount = (keys.Contains(Key.E) ? 1 : 0) - (keys.Contains(Key.Q) ? 1 : 0);
            Vector3 forward = Mode == CameraMode.Fly ? Direction : HorizontalForward;
            Vector3 right = Vector3.Cross(HorizontalForward, Vector3.UnitY);
            Vector3 movement = forward * forwardAmount + right * rightAmount;
            if (Mode != CameraMode.TopDown) movement += Vector3.UnitY * verticalAmount;
            if (movement.Length > 1) movement.Normalize();
            float speed = Down(Key.ShiftLeft, Key.ShiftRight) ? MoveSpeedFast : MoveSpeed;
            if (Mode == CameraMode.TopDown) speed *= Math.Max(0.25f, Camera.OrthographicHeight / 80f);
            Translate(movement * Math.Min(dTime, 0.1f) * speed);
            if (Mode == CameraMode.TopDown && verticalAmount != 0) Zoom(verticalAmount * dTime * 5);
        }

        private bool Down(Key first, Key second) { return keys.Contains(first) || keys.Contains(second); }

        public void KeyDown(Key key)
        {
            keys.Add(key);
            if (key == Key.W || key == Key.A || key == Key.S || key == Key.D || key == Key.Q || key == Key.E ||
                key == Key.Up || key == Key.Down || key == Key.Left || key == Key.Right) animate = false;
        }
        public void KeyUp(Key key) { keys.Remove(key); }

        private void Translate(Vector3 delta)
        {
            // Only explicit height controls alter Y in Pan mode.
            if (delta.LengthSquared == 0) return;
            Vector3 position = Camera.Position + delta;
            if (Mode != CameraMode.TopDown) position.Y = Math.Max(0.5f, position.Y);
            delta = position - Camera.Position;
            Camera.Relocate(position, Camera.LookAt + delta);
        }

        // Mouse deltas are normalized by viewport height, independent of DPI/aspect ratio.
        public void MouseMove(Vector2 delta)
        {
            animate = false;
            if (Mode == CameraMode.Fly || (Mode == CameraMode.Pan && Down(Key.AltLeft, Key.AltRight)))
            {
                Camera.Rotate(-delta.X * MouseSpeed, -delta.Y * MouseSpeed);
                return;
            }
            float span = Mode == CameraMode.TopDown ? Camera.OrthographicHeight : Math.Max(8, Camera.Position.Y) * 1.1547f;
            Vector3 forward = HorizontalForward;
            Vector3 right = Vector3.Cross(forward, Vector3.UnitY);
            Translate((-right * delta.X - forward * delta.Y) * span);
        }

        public void Zoom(float delta)
        {
            animate = false;
            if (Mode == CameraMode.TopDown)
                Camera.OrthographicHeight = Math.Max(4, Math.Min(1024, Camera.OrthographicHeight * (float)Math.Exp(-delta * 0.15f)));
            else if (Mode == CameraMode.Pan)
                Translate(Vector3.UnitY * (Camera.Position.Y * ((float)Math.Exp(-delta * 0.12f) - 1)));
            else Translate(Direction * delta * Math.Max(1, Camera.Position.Y * 0.08f));
        }
    }
}
