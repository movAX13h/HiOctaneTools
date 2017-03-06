using LevelEditor.Engine.Core;
using LevelEditor.Engine.GUI.Controls;
using LevelEditor.Games.HiOctane.Resources;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace LevelEditor.Games.HiOctane.Controls
{
    public class EntityListItem : ListItem
    {
        public SceneNode Node;

        public EntityListItem(SceneNode node, Vector2 size, string text, Action<ListItem> cbSelected = null) : base(size, text, cbSelected)
        {
            Node = node;
        }
    }
}
