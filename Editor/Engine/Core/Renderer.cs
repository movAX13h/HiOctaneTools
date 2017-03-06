using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Utils;
using LevelEditor.Engine.Lights;
using LevelEditor.Engine.FrameBuffers;
using LevelEditor.Engine.Materials;
using LevelEditor.Engine.Models;
using LevelEditor.Engine.PostProcess;
using LevelEditor.Engine.GUI.Controls;

namespace LevelEditor.Engine.Core
{
    public enum RenderPass { Shadow, Normal, Post }

    class Renderer
    {
        private int width;
        private int height;

        private Vector2 size;

        private Matrix4 view;
        private Matrix4 projection;
        private Vector2 mouse;
        private Vector3 nearPoint;
        private Vector3 farPoint;
        private RenderPass pass;

        public RenderPass Pass { get { return pass; } }
        public Matrix4 View { get { return view; } }
        public Matrix4 Projection { get { return projection; } }
        public Vector2 Mouse { get { return mouse; } set { mouse = value; } }
        public Vector3 NearPoint { get { return nearPoint; } set { nearPoint = value; } }
        public Vector3 FarPoint { get { return farPoint; } set { farPoint = value; } }

        private CameraNode camera = null;
        public CameraNode CurrentCamera { get { return camera; } }

        public Matrix4 ShadowProjection { get { return shadowProjection; } }
        private Matrix4 shadowProjection;

        public Matrix4 ShadowMapBias { get; private set; }

        public DefaultFrameBuffer DefaultFrameBuffer { get; private set; }
        public ShadowFrameBuffer ShadowFrameBuffer { get; private set; }

        private PostProcessEffect postProcessEffect;

        private Color clearColor;

        public bool Ready { get; private set; }

        public Renderer()
        {
            Ready = false;

            Log.WriteLine("using GL " + GL.GetString(StringName.Version));

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.LineSmooth);
            //GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);

            ShadowMapBias = new Matrix4(0.5f, 0, 0, 0, 0, 0.5f, 0, 0, 0, 0, 0.5f, 0, 0.5f, 0.5f, 0.5f, 1.0f);

            postProcessEffect = new PostProcessDisplay(3.7f, 2.2f, new Vector3(2.5f, 2.4f, 2.2f));
            //postProcessEffect = new PostProcessBlur(1.0f / DefaultFrameBuffer.Size.X, 1.0f / DefaultFrameBuffer.Size.Y);
            //postProcessEffect = new PostProcessEdgeDetect(0.2f, new Vector4(0.0f, 0.0f, 0.0f, 0.12f));

            if (!postProcessEffect.Ready)
            {
                Log.WriteLine(Log.LOG_ERROR, "failed to load post processing effect");
                return;
            }

            ClearColor(Color.CornflowerBlue);
            Ready = true;
        }

        private bool createFrameBuffers(int w, int h)
        {
            if (DefaultFrameBuffer != null) DefaultFrameBuffer.Delete();

            DefaultFrameBuffer = new DefaultFrameBuffer(w, h);
            DefaultFrameBuffer.Create();
            if (!DefaultFrameBuffer.Ready)
            {
                Log.WriteLine(Log.LOG_ERROR, "failed to create default frame buffer");
                return false;
            }

            if (ShadowFrameBuffer == null)
            {
                ShadowFrameBuffer = new ShadowFrameBuffer(2048, 2048);
                ShadowFrameBuffer.Create();

                if (!ShadowFrameBuffer.Ready)
                {
                    Log.WriteLine(Log.LOG_ERROR, "failed to create shadow frame buffer");
                    return false;
                }
            }
            return true;
        }

        public void ClearColor(Color color)
        {
            clearColor = color;
            GL.ClearColor(color);
        }

        public void Render(RootNode scene, Panel overlay)
        {
            // render scene shadow to framebuffer texture (from light source to scene origin)
            if (scene.ShadowMaterial != null && scene.ShadowMaterial.Ready && ShadowFrameBuffer.Ready)
            {
                pass = RenderPass.Shadow;
                ShadowFrameBuffer.Bind();
                scene.ShadowMaterial.Bind();

                    DirectionalLight light = (DirectionalLight)scene.GetLight(0);
                    view = Matrix4.LookAt(-light.Direction * 10.0f, Vector3.Zero, Vector3.UnitY);
                    shadowProjection = Matrix4.CreateOrthographicOffCenter(-10, 10, -10, 10, -10, 20);

                    renderNode(scene);

                scene.ShadowMaterial.Unbind();
                ShadowFrameBuffer.Unbind();
            }

            // render scene to framebuffer
            if (DefaultFrameBuffer.Ready)
            {
                //GL.Disable(EnableCap.CullFace);

                pass = RenderPass.Normal;
                DefaultFrameBuffer.Bind();

                if (camera == null) view = Matrix4.LookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
                else view = Matrix4.LookAt(camera.Position, camera.LookAt, camera.Up);

                renderNode(scene);

                if (overlay != null)
                {
                    GL.Disable(EnableCap.DepthTest);
                    overlay.Draw(size);
                    GL.Enable(EnableCap.DepthTest);
                }

                DefaultFrameBuffer.Unbind();
            }

            // post processing
            pass = RenderPass.Post;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.PushMatrix();
            GL.Ortho(0, width, 0, height, -1, 1); // Bottom-left corner pixel is 0/0

            postProcessEffect.SetSize(size);
            postProcessEffect.Render(scene.Time);

            GL.PopMatrix();
        }

        private void renderNode(SceneNode node)
        {
            if (node is RenderNode) ((RenderNode)node).Render();
            foreach (SceneNode subnode in node.Nodes) renderNode(subnode); // recursion
        }

        public void SetCamera(CameraNode cam)
        {
            camera = cam;
        }

        public void setViewport(int x, int y, int w, int h)
        {
            width = w;
            height = h;
            size = new Vector2(width, height);
            GL.Viewport(x, y, width, height);

            createFrameBuffers(width, height);
        }

        public void setPerspective(float fovy, float aspect, float near, float far)
        {
            projection = Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, near, far);
        }

        public void Resize(Rectangle clientRectangle, int w, int h)
        {
            setViewport(clientRectangle.X, clientRectangle.Y, clientRectangle.Width, clientRectangle.Height);
            setPerspective((float)Math.PI / 3, (float)w / (float)h, 1.0f, 10000.0f);
        }

        public Vector3 Unproject(Vector2 pos, float z)
        {
            return Unproject(new Vector3(pos.X, pos.Y, z));
        }

        public Vector3 Unproject(Vector3 pos)
        {
            Vector4 vec;

            vec.X = 2.0f * pos.X / (float)width - 1;
            vec.Y = 2.0f * pos.Y / (float)height - 1;
            vec.Z = pos.Z;
            vec.W = 1.0f;

            Matrix4 viewInv = Matrix4.Invert(view);
            Matrix4 projInv = Matrix4.Invert(projection);

            Vector4.Transform(ref vec, ref projInv, out vec);
            Vector4.Transform(ref vec, ref viewInv, out vec);

            if (vec.W != 0)
            {
                vec.X /= vec.W;
                vec.Y /= vec.W;
                vec.Z /= vec.W;
            }

            return vec.Xyz;
        }

    }
}
