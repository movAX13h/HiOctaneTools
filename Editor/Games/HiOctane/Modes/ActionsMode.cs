using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;

using LevelEditor.Engine.GUI.Controls;
using LevelEditor.Games.HiOctane.Controls;
using LevelEditor.Games.HiOctane.Resources;
using LevelEditor.Utils;

namespace LevelEditor.Games.HiOctane.Modes
{
    public class ActionsMode : EditMode
    {
        private FloatPanel listsPanel;
        private ListBox groupsList;
        private ListBox entitiesList;
        private DropDown newEntityDropDown;

        private string[] groupNames = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        private List<EntityItem.EntityType> ignoredTypes = new List<EntityItem.EntityType> {
            EntityItem.EntityType.Unknown,
            EntityItem.EntityType.WallSegment,
            EntityItem.EntityType.WaypointAmmo,
            EntityItem.EntityType.WaypointFast,
            EntityItem.EntityType.WaypointFuel,
            EntityItem.EntityType.WaypointShield,
            EntityItem.EntityType.WaypointShortcut,
            EntityItem.EntityType.WaypointSlow,
            EntityItem.EntityType.WaypointSpecial1,
            EntityItem.EntityType.WaypointSpecial2,
            EntityItem.EntityType.WaypointSpecial3
        };

        public ActionsMode() : base("Actions")
        {
            showWalls = false;
            showWaypoints = false;
            showBuildings = true;
            showEntities = true;
            showTerrain = true;

            fadeTerrain = true;
            fadeBuildings = true;
        }

        public override void CreateControls(GUI gui)
        {
            listsPanel = new FloatPanel(new Vector2(300, 200), "ENTITIES", false, false);
            listsPanel.Pos.X = 0;
            listsPanel.Pos.Y = 22;
            listsPanel.Visible = false;
            gui.AddChild(listsPanel);

            groupsList = new ListBox(new Vector2(120, listsPanel.Size.Y - 24), 1, groupItemSelected);
            groupsList.Pos.X = 2;
            groupsList.Pos.Y = 2;
            listsPanel.AddChild(groupsList);

            entitiesList = new ListBox(new Vector2(174, listsPanel.Size.Y - 24), 1, entityItemSelected);
            entitiesList.Pos.X = 124;
            entitiesList.Pos.Y = 2;
            listsPanel.AddChild(entitiesList);

            newEntityDropDown = new DropDown(addEntityDropDownItemSelected);
            newEntityDropDown.Pos.X = listsPanel.Size.X - 20;
            listsPanel.AddChild(newEntityDropDown);

            newEntityDropDown.AddItem("New Checkpoint", 1);
            newEntityDropDown.AddItem("New Trigger", 1);
            newEntityDropDown.AddItem("New Recovery Truck", 1);
            newEntityDropDown.AddItem("New Cone", 1);
            newEntityDropDown.AddItem("New Explosion", 1);
            newEntityDropDown.AddItem("New Steam", 1);
            newEntityDropDown.AddItem("New Fuel Item", 1);
            newEntityDropDown.AddItem("New Shield Item", 1);
            newEntityDropDown.AddItem("New Ammo Item", 1);
            newEntityDropDown.AddItem("New Minigun Item", 1);
            newEntityDropDown.AddItem("New Missile Item", 1);
            newEntityDropDown.AddItem("New Booster Item", 1);

            layout();
        }

        private void addEntityDropDownItemSelected(ListItem item)
        {

        }

        public override void Resize(GUI gui)
        {
            listsPanel.Resize(listsPanel.Size.X, gui.Size.Y - 134);

            groupsList.Resize(groupsList.Size.X, listsPanel.Size.Y - 24);
            entitiesList.Resize(entitiesList.Size.X, listsPanel.Size.Y - 24);

            layout();
        }

        private void layout()
        {
            newEntityDropDown.Pos.Y = listsPanel.Size.Y - 18;
        }

        protected override void enable()
        {
            groupsList.Clear();
            groupsList.Lock();
            foreach (var groupEntry in level.GroupedEntities)
            {
                addGroup(groupEntry.Key, groupEntry.Value);
            }
            groupsList.Unlock();

            entitiesList.Clear();

            listsPanel.Visible = true;
        }


        public void addGroup(int id, List<EntityItem> entities)
        {
            groupsList.AddItem("GROUP " + groupNames[groupsList.Items.Count], id);
        }

        private void groupItemSelected(ListItem item)
        {
            int id = (int)item.Tag;
            entitiesList.Clear();
            entitiesList.Lock();
            foreach(EntityItem entity in level.GroupedEntities[id])
            {
                if (ignoredTypes.Contains(entity.GameType)) continue;
                addEntity(entity);
            }
            entitiesList.Unlock();
        }

        private void addEntity(EntityItem entity)
        {
            entitiesList.AddItem(entity.GameType.ToString().CamelCaseToHuman(), entity);
        }

        private void entityItemSelected(ListItem item)
        {

        }

        protected override void disable()
        {
            listsPanel.Visible = false;
            //list.Visible = false;
        }
    }
}
