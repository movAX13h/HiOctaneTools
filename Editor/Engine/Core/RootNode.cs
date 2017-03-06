using System;
using System.Text;
using System.Collections.Generic;
using OpenTK.Input;
using OpenTK;
using LevelEditor.Engine.Resources;

namespace LevelEditor.Engine.Core
{
    public abstract class RootNode : SceneNode
    {
        public float Time { get; protected set; }

        private bool ready;
        public bool Ready { get { return ready; } }

        private List<LightNode> lights;
        public Material ShadowMaterial { get; protected set; }

        public RootNode()
        {
            lights = new List<LightNode>();
            ready = false;
            Time = 0;
        }

        public void Setup()
        {
            if (ready) return;
            ready = setup();
        }

        public void AddLight(LightNode light)
        {
            lights.Add(light);
            AddNode(light);
        }

        public LightNode GetLight(int id)
        {
            if (id < lights.Count) return lights[id];
            return null;
        }

        protected abstract bool setup();

        public virtual void KeyDown(Key key) { }
        public virtual void KeyUp(Key key) { }

        public virtual void MouseMove(Vector2 pos, Vector2 delta) { }

    }
}
