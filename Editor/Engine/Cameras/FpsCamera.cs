using System;

using OpenTK;
using OpenTK.Graphics;

using LevelEditor.Engine.Core;
using LevelEditor.Utils;
using System.Diagnostics;

namespace LevelEditor.Engine.Cameras
{
    public class FpsCamera : CameraNode
    {
        private float yaw;
        private float pitch;

        public FpsCamera(Vector3 pos, Vector3 lookAt) : base(pos, lookAt)
        {
            Relocate(pos, lookAt);
        }

        public void Relocate(Vector3 pos, Vector3 lookAt)
        {
            Vector3 delta = (lookAt - pos).Normalized();
            position = pos;
            LookAt = lookAt;

            yaw = (float)(Math.Atan2(delta.X, delta.Z));
            pitch = (float)Math.PI - (float)Math.Atan2(delta.Y, Math.Sqrt(delta.X * delta.X + delta.Z * delta.Z));
        }

        public void Move(float distance)
	    {
            Vector3 direction = LookAt - position;
            Vector3 delta = Vector3.Multiply(direction, distance);

            position = Vector3.Add(position, delta);
            LookAt = Vector3.Add(LookAt, delta);
        }

        public void Strafe(float distance)
	    {
            Vector3 direction = LookAt - position;
		    Vector3 normal = Vector3.Cross(Vector3.UnitY, direction);
            Vector3 delta = Vector3.Multiply(normal, distance);

            position = Vector3.Add(position, delta);
            LookAt = Vector3.Add(LookAt, delta);
        }

        public void Rotate(float dx, float dy)
        {
            yaw += dx;
            pitch += dy;
            if (pitch < 1.8f) pitch = 1.8f;
            if (pitch > 4.5f) pitch = 4.5f;

            Matrix4 rotationMatrix = Matrix4.Mult(Matrix4.CreateRotationX(pitch), Matrix4.CreateRotationY(yaw));
            LookAt = Vector3.Add(position, -Vector3.TransformVector(Vector3.UnitZ, rotationMatrix));
        }

        public override string ToString()
        {
            return "Pos: " + position.ToString() + ", Pitch: " + pitch.ToString() + ", Yaw: " + yaw.ToString();
        }
    }
}
