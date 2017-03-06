using System;
using System.Collections.Generic;

using LevelEditor.Engine.Resources.Loaders;

namespace LevelEditor.Engine.Resources
{
    class ResourceManager
    {
        private Dictionary<string, Resource> resources;

        public ResourceManager()
        {
            resources = new Dictionary<string, Resource>();
        }

        public ShaderResource GetShader(string name)
        {
            return GetShader(name, null);
        }

        public ShaderResource GetShader(string name, List<string> defines)
        {
            return GetShader(name, name, defines);
        }

        public ShaderResource GetShader(string vertName, string fragName, List<string> defines)
        {
            string hash = "";
            if (defines != null && defines.Count > 0) hash = "_" + string.Join(",", defines).GetHashCode().ToString();

            string key = "shd_" + vertName + "_" + fragName + hash;
            if (resources.ContainsKey(key)) return (ShaderResource)resources[key];

            ShaderResource r = new ShaderResource(vertName, fragName, defines);
            if (r.Ready) resources.Add(key, r);
            return r;
        }

        public TextureResource GetTexture(string name)
        {
            string key = "tex_" + name.Replace(".", "_").Replace("/", "_");
            if (resources.ContainsKey(key)) return (TextureResource)resources[key];
            TextureResource r = new TextureResource(name);
            if (r.Ready) resources.Add(key, r);
            return r;
        }

        public HeightmapResource GetHeightmap(string name)
        {
            string key = "hmap_" + name;
            if (resources.ContainsKey(key)) return (HeightmapResource)resources[key];
            HeightmapResource r = new HeightmapResource(name);
            if (r != null) resources.Add(key, r);
            return r;
        }

        /*
        public Map GetMap(string name)
        {
            string key = "map_" + name;
            if (resources.ContainsKey(key)) return (Map)resources[key];
            Map r = MapLoader.Load(name);
            if (r != null) resources.Add(key, r);
            return r;
        }
        */
        /*
        public string GetText(string name)
        {
            return TextLoader.Load(name); // text is not cached
        }
        */
    }
}
