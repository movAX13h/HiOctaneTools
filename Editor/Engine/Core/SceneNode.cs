using System;
using System.Collections.Generic;
//using OpenTK;

namespace LevelEditor.Engine.Core
{
    public class SceneNode
    {
        public string Name = "";
        public List<SceneNode> Nodes { get; private set; }
        public SceneNode Parent { get; private set; }

        public SceneNode()
        {
            Nodes = new List<SceneNode>();
        }

        public void AddNode(SceneNode node)
        {
            if (node.Parent != this)
            {
                Nodes.Add(node);
                node.Parent = this;
            }
        }

        public void RemoveNode(SceneNode node)
        {
            if (node.Parent == this)
            {
                Nodes.Remove(node);
                node.Parent = null;
            }
        }

        public virtual void Update(float time)
        {
            foreach (SceneNode node in Nodes) node.Update(time);
        }

        public string GraphString(string indent = "", bool last = true)
        {
            string s = indent;

            if (last)
            {
                s += "\\--";
                indent += "  ";
            }
            else
            {
                s += "|--";
                indent += "|  ";
            }

            s += (string.IsNullOrEmpty(Name) ? "[unnamed scene node] " : Name) + Environment.NewLine;

            for (int i = 0, len = Nodes.Count; i < len; i++)
            {
                s += Nodes[i].GraphString(indent, i == len - 1);
            }

            return s;
        }

    }
}
