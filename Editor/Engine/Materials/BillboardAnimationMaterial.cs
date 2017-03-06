using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Core;
using LevelEditor.Engine.Resources;
using System.Collections.Generic;

namespace LevelEditor.Engine.Materials
{
    class BillboardAnimationMaterial : Material
    {
        public TextureResource TextureResource { get; private set; }
        public int TextureHandle { get { return this.TextureResource.Id; } }

        public bool UseTexture { get; private set; }
        public bool UseShadowMap { get; private set; }


        public BillboardAnimationMaterial(string textureFilename = "")
        {
            ready = true;
            UseTexture = false;
            UseShadowMap = false;

            if (textureFilename != "")
            {
                this.TextureResource = EngineBase.Manager.GetTexture(textureFilename);
                if (this.TextureResource.Ready) UseTexture = true;
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

            shaderResource = EngineBase.Manager.GetShader("billboardanim", defines);
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
