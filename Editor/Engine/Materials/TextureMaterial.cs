using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Core;
using LevelEditor.Engine.Resources;
using System.Collections.Generic;

namespace LevelEditor.Engine.Materials
{
    class TextureMaterial : Material
    {
        private TextureResource textureResource;
        public int TextureHandle { get { return textureResource.Id; } }

        private TextureResource normalsResource;
        public int NormalsHandle { get { return normalsResource.Id; } }

        public bool UseNormalMap { get; private set; }
        public bool UseTexture { get; private set; }

        public TextureMaterial(string textureFilename, string normalsFilename, bool useVertexNormals, bool receiveShadows)
        {
            ready = true;
            UseTexture = false;
            UseNormalMap = false;

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

            if (normalsFilename != "")
            {
                normalsResource = EngineBase.Manager.GetTexture(normalsFilename);
                if (normalsResource.Ready) UseNormalMap = true;
                else
                {
                    ready = false;
                    return;
                }
            }

            // defines that need to be enabled in the shader
            List<string> defines = new List<string>();
            if (UseTexture) defines.Add("USE_COLOR_MAP");
            if (UseNormalMap) defines.Add("USE_NORMAL_MAP");
            if (useVertexNormals) defines.Add("USE_VERTEX_NORMALS");
            if (receiveShadows) defines.Add("USE_SHADOW_MAP");

            shaderResource = EngineBase.Manager.GetShader("texture", defines);
            if (!shaderResource.Ready) ready = false;
        }

        /*
        public override void Bind()
        {
            base.Bind();
        }

        public override void UnBind()
        {
            base.UnBind();
        }
        */
    }
}
