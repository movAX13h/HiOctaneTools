using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Core;
using LevelEditor.Engine.Resources;
using System.Collections.Generic;

namespace LevelEditor.Engine.Materials
{
    class BillboardMaterial : Material
    {
        private TextureResource textureResource;
        public int TextureHandle { get { return textureResource.Id; } }

        public bool UseTexture { get; private set; }
        public bool UseShadowMap { get; private set; }

        public BillboardMaterial(string textureFilename = "")
        {
            ready = true;
            UseTexture = false;
            UseShadowMap = false;

            if (textureFilename != "")
            {
                textureResource = EngineBase.Manager.GetTexture(textureFilename);
                if (textureResource.Ready) UseTexture = true;
                else
                {
                    ready = false;
                    return;
                }
            }

            // defines that need to be enabled in the shader
            List<string> defines = new List<string>();
            if (UseTexture) defines.Add("USE_COLOR_MAP");
            if (UseShadowMap) defines.Add("USE_SHADOW_MAP");

            shaderResource = EngineBase.Manager.GetShader("billboard", defines);
            if (!shaderResource.Ready) ready = false;
        }

        public override void Bind()
        {
            //GL.Disable(EnableCap.DepthTest);
            base.Bind();
        }

        public override void Unbind()
        {
            base.Unbind();
            //GL.Enable(EnableCap.DepthTest);
        }

    }
}
