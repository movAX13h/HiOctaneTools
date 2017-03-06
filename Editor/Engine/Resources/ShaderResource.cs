using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Utils;
using LevelEditor.Engine;

using LevelEditor.Engine.Resources.Loaders;
using System.Text.RegularExpressions;

namespace LevelEditor.Engine.Resources
{
    public class ShaderResource : Resource
    {
        protected string vertexShaderSource;
        protected int vertexShaderHandle;

        protected string fragmentShaderSource;
        protected int fragmentShaderHandle;

        protected int programHandle;

        private Dictionary<string, int> uniformLocations;

        public ShaderResource(string name):this(name, null)
        {

        }

        public ShaderResource(string name, List<string> defines) : this(name, name, defines)
        {
        }

        public ShaderResource(string vertName, string fragName, List<string> defines) : base(vertName + " + " + fragName)
        {
            string msg = "loading shader '" + vertName + "' and '" + fragName + "'";
            if (defines != null && defines.Count > 0) msg += " with these defines enabled: " + string.Join(", ", defines);
            Log.WriteLine(msg);

            // vertex shader file
            string filename = Config.SHADER_FOLDER + vertName + Config.SHADER_VERT_EXTENSION;
            vertexShaderSource = TextLoader.Load(filename);

            // fragment shader file
            filename = Config.SHADER_FOLDER + fragName + Config.SHADER_FRAG_EXTENSION;
            fragmentShaderSource = TextLoader.Load(filename);

            uniformLocations = new Dictionary<string, int>();

            if (defines != null && defines.Count > 0) updateShaderDefines(defines);
            ready = create();
        }

        private void updateShaderDefines(List<string> defines)
        {
            vertexShaderSource = enableDefines(vertexShaderSource, defines);
            fragmentShaderSource = enableDefines(fragmentShaderSource, defines);
            //Log.WriteLine("\n\n" + vertexShaderSource);
            //Log.WriteLine("\n\n" + fragmentShaderSource);
        }

        private string enableDefines(string source, List<string> defines)
        {
            Regex r = new Regex(@"#define (?<name>[A-Z_]*) (?<status>[0,1])", RegexOptions.Multiline);

            foreach (Match m in r.Matches(source))
            {
                if (m.Success)
                {
                    string line = m.Value.Trim();
                    string name = m.Groups["name"].Value.Trim();
                    if (defines.Contains(name)) source = source.Replace(line, "#define " + name + " 1");
                }
            }

            return source;
        }

        private bool create()
        {
            if (vertexShaderSource.Length == 0 || fragmentShaderSource.Length == 0) return false;

            // vertex shader
            vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShaderSource);
            GL.CompileShader(vertexShaderHandle);
            string info = GL.GetShaderInfoLog(vertexShaderHandle);
            if (info == "") Log.WriteLine("compiled vertex shader '" + Name + "'");
            else
            {
                if (info.Contains("error"))
                {
                    Log.WriteLine(Log.LOG_ERROR, "failed to compile vertex shader '" + Name + "':\n" + info);
                    return false;
                }
                else Log.WriteLine(Log.LOG_INFO, "compiled vertex shader '" + Name + "':\n" + info);
            }

            // fragment shader
            fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle, fragmentShaderSource);
            GL.CompileShader(fragmentShaderHandle);
            info = GL.GetShaderInfoLog(fragmentShaderHandle);
            if (info == "") Log.WriteLine("compiled fragment shader '" + Name + "'");
            else
            {
                if (info.Contains("error"))
                {
                    Log.WriteLine(Log.LOG_ERROR, "failed to compile fragment shader '" + Name + "':\n" + info);
                    return false;
                }

                if (info.Contains("warning"))
                {
                    Log.WriteLine(Log.LOG_WARNING, "warning at fragment shader compile'" + Name + "':\n" + info);
                }
                else Log.WriteLine(Log.LOG_INFO, "compiled fragment shader '" + Name + "':\n" + info);
            }

            // program
            programHandle = GL.CreateProgram();
            GL.AttachShader(programHandle, vertexShaderHandle);
            GL.AttachShader(programHandle, fragmentShaderHandle);
            GL.LinkProgram(programHandle);

            info = GL.GetProgramInfoLog(programHandle);
            if (info == "") Log.WriteLine(Log.LOG_INFO, "uploaded shader program '" + Name + "'");
            else
            {
                if (info.Contains("error"))
                {
                    Log.WriteLine(Log.LOG_ERROR, "failed to upload shader program '" + Name + "':\n" + info);
                    return false;
                }
                else Log.WriteLine(Log.LOG_INFO, "uploaded shader program '" + Name + "':\n" + info);
            }

            return true;
        }

        public int UniformLocation(string name)
        {
            if (uniformLocations.ContainsKey(name)) return uniformLocations[name];
            int location = GL.GetUniformLocation(programHandle, name);
            uniformLocations.Add(name, location);
            return location;
        }

        public int ProgramHandle
        {
            get { return programHandle; }
        }

    }
}
