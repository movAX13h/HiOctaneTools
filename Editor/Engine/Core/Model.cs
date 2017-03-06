using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LevelEditor.Engine.Core
{
    public class Model : RenderNode
    {
        //protected int vaoHandle;

        protected bool ready = false;
        public bool Ready { get { return ready; } }

        protected Matrix4 shadowViewProjection;
        public Vector3 Size { get; protected set; }

        public Model()
        {
        }

        protected virtual void createVBOs()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void createVAOs()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void render()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void renderShadow()
        {

        }

        public override void Render()
        {
            base.Render();

            switch(EngineBase.Renderer.Pass)
            {
                case RenderPass.Normal: render(); break;
                case RenderPass.Shadow: renderShadow(); break;
                default: break;
            }
        }

    }
}
