using LevelEditor.Engine.Cameras;
using LevelEditor.Utils;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace LevelEditor.Games.HiOctane
{
    public class CameraController
    {
        public float MoveSpeed = 3f;
        public float MoveSpeedFast = 20f;
        public float MouseSpeed = 3f;

        public FpsCamera Camera { get { return camera; } }
        private FpsCamera camera;

        private Vector3 movement = Vector3.Zero;

        private bool moveForward = false;
        private bool moveBackward = false;
        private bool moveLeft = false;
        private bool moveRight = false;

        private bool fast = false;

        private bool animate = false;
        private Vector3 animationTarget;
        private float animationTargetDistance;

        public CameraController(Vector3 position, Vector3 lookAt)
        {
            camera = new FpsCamera(position, lookAt);
        }

        /*
        public void SetPosition(Vector3 pos)
        {
            Vector3 d = camera.LookAt - camera.Position;
            camera.Position = pos;
            camera.LookAt = pos + d;
        }
        */
        internal void AnimateTo(Vector3 pos, float distance)
        {
            animationTarget = pos;
            animationTargetDistance = distance;
            animate = true;
        }

        public void Update(float dTime)
        {
            if (animate)
            {
                Vector3 pos = camera.Position;
                Vector3 dir = (animationTarget - pos).Normalized();
                pos += (animationTarget - animationTargetDistance * dir - pos) * 2f * dTime;

                Vector3 lookAt = camera.LookAt;
                Vector3 d = animationTarget - lookAt;
                lookAt += d * 2f * dTime;

                camera.Relocate(pos, lookAt);

                if (d.Length < 0.1f) animate = false;
                return;
            }


            if (moveForward) movement.Z = 1.0f;
            if (moveBackward) movement.Z = -1.0f;
            if (moveLeft) movement.X = 1.0f;
            if (moveRight) movement.X = -1.0f;

            //if (movement.Length > 1.0f) movement.Normalize();

            float speed = (fast ? MoveSpeedFast : MoveSpeed);
            camera.Move(dTime * movement.Z * speed);
            camera.Strafe(dTime * movement.X * speed);

            movement *= 0.8f * (1.0f - dTime);
        }

        public void KeyDown(Key key)
        {
            if (key == Key.W) moveForward = true;
            if (key == Key.S) moveBackward = true;
            if (key == Key.A) moveLeft = true;
            if (key == Key.D) moveRight = true;
            if (key == Key.ShiftLeft) fast = true;
        }

        public void KeyUp(Key key)
        {
            if (key == Key.W) moveForward = false;
            if (key == Key.S) moveBackward = false;
            if (key == Key.A) moveLeft = false;
            if (key == Key.D) moveRight = false;
            if (key == Key.ShiftLeft) fast = false;
        }

        public void MouseMove(Vector2 delta)
        {
            delta *= MouseSpeed;
            camera.Rotate(-delta.X, -delta.Y);
        }

    }
}
