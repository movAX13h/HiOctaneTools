using System;
using System.IO;
using System.Diagnostics;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Resources;
using System.Drawing;

namespace LevelEditor.Engine.Core
{
    public class Shader
    {
        protected ShaderResource shaderResource;

        public Shader()
        {

        }

        public bool SetUniform(string name, float f)
        {
            int location = shaderResource.UniformLocation(name);
            if (location < 0) return false;
            GL.Uniform1(location, 1, ref f);
            return true;
        }

        public bool SetUniform(string name, Matrix3 matrix)
        {
            int location = shaderResource.UniformLocation(name);
            if (location < 0) return false;
            GL.UniformMatrix3(location, false, ref matrix);
            return true;
        }
        public bool SetUniform(string name, Matrix4 matrix)
        {
            int location = shaderResource.UniformLocation(name);
            if (location < 0) return false;
            GL.UniformMatrix4(location, false, ref matrix);
            return true;
        }

        public bool SetUniform(string name, int i)
        {
            int location = shaderResource.UniformLocation(name);
            if (location < 0) return false;
            GL.Uniform1(location, 1, ref i);
            return true;
        }

        public bool SetUniform(string name, ref Vector3 v)
        {
            int location = shaderResource.UniformLocation(name);
            if (location < 0) return false;
            GL.Uniform3(location, ref v);
            return true;
        }

        public bool SetUniform(string name, Vector3 v)
        {
            int location = shaderResource.UniformLocation(name);
            if (location < 0) return false;
            GL.Uniform3(location, ref v);
            return true;
        }

        public bool SetUniform(string name, Vector2 v)
        {
            int location = shaderResource.UniformLocation(name);
            if (location < 0) return false;
            GL.Uniform2(location, ref v);
            return true;
        }

        public bool SetUniform(string name, ref Vector2 v)
        {
            int location = shaderResource.UniformLocation(name);
            if (location < 0) return false;
            GL.Uniform2(location, ref v);
            return true;
        }

        public bool SetUniform(string name, Vector4 v)
        {
            int location = shaderResource.UniformLocation(name);
            if (location < 0) return false;
            GL.Uniform4(location, ref v);
            return true;
        }

        public bool SetUniform(string name, ref Vector4 v)
        {
            int location = shaderResource.UniformLocation(name);
            if (location < 0) return false;
            GL.Uniform4(location, ref v);
            return true;
        }

        public bool SetUniform(string name, Color color)
        {
            int location = shaderResource.UniformLocation(name);
            if (location < 0) return false;

            Vector3 v = new Vector3(color.R / 256f, color.G / 255f, color.B / 255f);
            GL.Uniform3(location, ref v);
            return true;
        }

        public bool SetUniform(string name, bool v)
        {
            int location = shaderResource.UniformLocation(name);
            if (location < 0) return false;
            GL.Uniform1(location, v ? 1.0f : 0.0f);
            return true;
        }

        public virtual void Bind()
        {
            GL.UseProgram(shaderResource.ProgramHandle);
        }

        public virtual void Unbind()
        {
            GL.UseProgram(0);
        }

        public int ProgramHandle
        {
            get { return shaderResource.ProgramHandle; }
        }

    }
}
