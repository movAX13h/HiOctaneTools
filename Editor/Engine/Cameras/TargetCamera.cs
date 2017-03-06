using OpenTK;
using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.Cameras
{
    class TargetCamera : CameraNode
    {
        public TargetCamera(Vector3 position, Vector3 lookAt) : base(position, lookAt)
        {
            // TODO: follow target RenderNode
        }
    }
}
