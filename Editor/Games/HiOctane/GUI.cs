using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using LevelEditor.Engine.GUI;
using LevelEditor.Engine.GUI.Controls;
using System.Drawing;
using LevelEditor.Games.HiOctane.Resources;
using LevelEditor.Engine.Core;
using LevelEditor.Games.HiOctane.Controls;
using LevelEditor.Games.HiOctane.Modes;
using LevelEditor.Engine;
using System.IO;

namespace LevelEditor.Games.HiOctane
{
    public class GUI : Panel
    {
        #region statics
        public static bool MouseUsed = false;
        private static GUI instance;

        public static bool IsVisible
        {
            get
            {
                return instance.Visible;
            }
        }
        #endregion

        private Menu menu;
        private ListBox list;
        private Label titleLabel;
        private InfoPanel infoPanel;
        private Panel statusPanel;
        private Label fpsLabel;

        private ImageButton winMinimizeButton;
        private ImageButton winFullscreenButton;
        private ImageButton winRestoreButton;
        private ImageButton winCloseButton;
        private ResizeGrip winSizeGripButton;

        private EditMode currentMode = null;
        private List<EditMode> editModes = new List<EditMode>();

        private Editor editor;
        private Level level;

        private bool allowMouse = false;

        public GUI(Editor editor) : base(new Vector2(800, 600))
        {
            this.editor = editor;
            instance = this;

            Alpha = 0f;
            BackgroundColor = Color.Black;

            editModes.Add(new ViewMode());
            editModes.Add(new TexturesMode());
            editModes.Add(new TerrainMode());
            editModes.Add(new BuildingsMode());
            editModes.Add(new WallsMode());
            editModes.Add(new WaypointsMode());
            editModes.Add(new ActionsMode());

            #region main menu
            menu = new Menu(new Vector2(400, 32), "gui/images/bullfrog.png");

            MenuItem item = menu.AddItem("PROGRAM");
            item.AddItem("[BSP] Fullscreen", toggleFullscreen);
            item.AddItem("[INS] VSync ON/OFF", toggleVSync);
            item.AddItem("[TAB] GUI ON/OFF", toggleGUI);
            item.AddItem("[HOM] PROFILER ON/OFF", toggleProfiler);
            item.AddSeparator();
            item.AddItem("[ESC] Exit", exitApplication);

            item = menu.AddItem("LEVEL");
            item.AddItem("New…", dummyHandler);
            item.AddItem("Load…", loadClicked);
            item.AddItem("Save…", saveClicked);
            //item.AddItem("Save As…", dummyHandler);
            item.AddSeparator();
            item.AddItem("Amazon Delta Turnpike", loadLevelClicked, 1);
            item.AddItem("Trans-Asia Interstate", loadLevelClicked, 2);
            item.AddItem("Shanghai Dragon", loadLevelClicked, 3);
            item.AddItem("New Chernobyl Central", loadLevelClicked, 4);
            item.AddItem("Slam Canyon", loadLevelClicked, 5);
            item.AddItem("Thrak City", loadLevelClicked, 6);
            item.AddItem("Ancient Mine Town", loadLevelClicked, 7);
            item.AddItem("Arctic Land", loadLevelClicked, 8);
            item.AddItem("Death Match Arena", loadLevelClicked, 9);

            item = menu.AddItem("MODE");

            int i = 1;
            foreach(EditMode mode in editModes)
            {
                item.AddItem("[" + i + "] " + mode.Name, modeMenuItemSelected, mode);
                i++;
            }

            item = menu.AddItem("ABOUT");
            item.AddItem("Help", dummyHandler);
            item.AddItem("Author", authorLink);
            #endregion

            // list
            list = new ListBox(new Vector2(300, 600), 1);
            list.Visible = false;

            // info panel
            infoPanel = new InfoPanel();

            // status panel
            statusPanel = new Panel(new Vector2(300, 22));
            statusPanel.MouseEnabled = true; // avoid click-through
            statusPanel.BackgroundColor = Config.UI_COLOR_BLUE;
            statusPanel.BorderColor = Color.FromArgb(63, 63, 70);
            statusPanel.BorderSize = 1;

            fpsLabel = new Label(new Vector2(550, 17), 9);
            fpsLabel.Pos.X = 20;
            fpsLabel.Pos.Y = 1;
            fpsLabel.BackgroundColor = Config.UI_COLOR_BLUE;
            fpsLabel.TextColor = Color.White;
            statusPanel.AddChild(fpsLabel);

            // label
            titleLabel = new Label(new Vector2(250, 20), 10);
            titleLabel.TextColor = Color.Gray;
            titleLabel.BackgroundColor = menu.BackgroundColor;
            titleLabel.Text = Window.GetTitle(); // Config.APP_NAME + " v" + Config.APP_VERSION;

            #region window buttons
            Vector2 iconSize = new Vector2(20, 20);
            winMinimizeButton = new ImageButton(iconSize, "gui/images/winMinimize.png", null, winMinimizeButtonClicked);
            winFullscreenButton = new ImageButton(iconSize, "gui/images/winFullscreen.png", null, winFullscreenButtonClicked);
            winRestoreButton = new ImageButton(iconSize, "gui/images/winRestore.png", null, winRestoreButtonClicked);
            winCloseButton = new ImageButton(iconSize, "gui/images/winClose.png", null, winCloseButtonClicked);
            winSizeGripButton = new ResizeGrip(iconSize, "gui/images/winResizeGrip.png", winResizeGripDragged);
            #endregion

            // add all controls
            AddChild(list);
            foreach (EditMode mode in editModes)
            {
                mode.CreateControls(this);
            }
            AddChild(infoPanel);
            AddChild(menu);
            AddChild(titleLabel);
            AddChild(statusPanel);

            AddChild(winMinimizeButton);
            AddChild(winFullscreenButton);
            AddChild(winRestoreButton);
            AddChild(winCloseButton);

            AddChild(winSizeGripButton);

            Editor.Profiler.Pos.Y = 50;
            AddChild(Editor.Profiler);

            layout();
        }

        private const string LevelFilesFilter = "Level files (*.dat)|*.dat|All files (*.*)|*.*";

        private void loadClicked(MenuItem obj)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = LevelFilesFilter;
            openFileDialog1.Title = "Select a Hi-Octane Level File";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        private void saveClicked(MenuItem obj)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog1.Filter = LevelFilesFilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            bool wasFullscreen = Window.IsFullscreen;
            if (Window.IsFullscreen) Window.ToggleFullscreen();

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                level.Data.Save(saveFileDialog1.FileName);
            }

            if (wasFullscreen) Window.ToggleFullscreen();
        }

        private void layout()
        {
            list.Pos.Y = statusPanel.Size.Y;

            titleLabel.Pos.X = Size.X - titleLabel.Size.X - 90;
            titleLabel.Pos.Y = Size.Y - titleLabel.Size.Y - 9;

            menu.Pos.Y = Size.Y - menu.Size.Y;

            float y = Size.Y - 27;

            winCloseButton.Pos.X = Size.X - 30;
            winCloseButton.Pos.Y = y;

            winFullscreenButton.Pos.X = winCloseButton.Pos.X - 32;
            winFullscreenButton.Pos.Y = y;

            winRestoreButton.Pos.X = winFullscreenButton.Pos.X;
            winRestoreButton.Pos.Y = y;

            winMinimizeButton.Pos.X = winRestoreButton.Pos.X - 32;
            winMinimizeButton.Pos.Y = y;

            winSizeGripButton.Pos.X = Size.X - 20;

            infoPanel.Pos.Y = menu.Pos.Y - infoPanel.Size.Y;

            fpsLabel.Pos.X = (float)Math.Floor(0.5f * (Size.X - fpsLabel.Size.X));

            Editor.Profiler.Pos.X = Size.X - Editor.Profiler.Size.X - 10;
        }

        public override void Resize(float w, float h)
        {
            base.Resize(w, h);

            list.Resize(300, h - menu.Size.Y - infoPanel.Size.Y - statusPanel.Size.Y);
            menu.Resize(w, menu.Size.Y);
            statusPanel.Resize(w, statusPanel.Size.Y);

            foreach (EditMode mode in editModes)
            {
                mode.Resize(this);
            }

            winFullscreenButton.Visible = !Window.IsFullscreen;
            winRestoreButton.Visible = Window.IsFullscreen;
            winSizeGripButton.Visible = winFullscreenButton.Visible;

            Editor.Profiler.Resize(Editor.Profiler.Size.X, h - 100);

            layout();
        }

        public override void Draw(Vector2 resolution)
        {
            base.Draw(resolution);
            currentMode.Draw();
        }

        public void SetLevel(Level level)
        {
            this.level = level;
            infoPanel.LevelName.Text = level.Name;
            Window.SetTitleAppendix(level.Name);
            foreach (EditMode mode in editModes) mode.SetLevel(level);
            enableMode(editModes[0]);
        }

        public override void Update(float time, float dTime)
        {
            string space = "         ";
            fpsLabel.Text = "VSYNC " + (Window.IsVSync ? "ON" : "OFF").PadRight(3, ' ') + space +
                            "FPS " + Window.RenderFPS.ToString().PadRight(4, ' ') + space +
                            "RT " + (Window.RenderDuration * 1000).ToString("0.0ms").PadRight(10, ' ') + space +
                            "UT " + (Window.UpdateDuration * 1000).ToString("0.0ms").PadRight(10, ' ');

            if (!Window.MouseLeftDown) allowMouse = true; // allow mouse over


            if (allowMouse && Window.IsCursorVisible) ProcessMouse(); // this recursively iterates all controls with MouseEnabled/MouseChildren enabled
            if (!MouseConsumed) currentMode.Update(time, dTime);
            MouseUsed = MouseConsumed || currentMode.MouseUsed;


            base.Update(time, dTime);
        }

        public void WindowMouseDown()
        {
            ProcessMouse();
            allowMouse = MouseConsumed;
            if (!MouseConsumed) menu.Close();
        }


        #region mode
        private void enableMode(EditMode mode)
        {
            if (currentMode != null) currentMode.Disable();
            currentMode = mode;
            currentMode.Enable();
            infoPanel.Headline.Text = mode.Name.ToUpper();
        }
        #endregion

        #region menu item handlers
        private void dummyHandler(MenuItem item)
        {

        }

        private void loadLevelClicked(MenuItem item)
        {
            /*if (System.Windows.Forms.MessageBox.Show("Loading will discard all changes made." + Environment.NewLine +
                "Do you want to continue?", "Unsaved changes",
                System.Windows.Forms.MessageBoxButtons.YesNo,
                System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {*/

            if (!editor.LoadLevel((int)item.Tag)) System.Windows.Forms.MessageBox.Show("Failed to load level! Please restart the application!", "Sorry...", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }

        private void modeMenuItemSelected(MenuItem item)
        {
            EditMode mode = item.Tag as EditMode;
            enableMode(mode);
        }

        private void winCloseButtonClicked(ImageButton button)
        {
            Window.Terminate();
        }

        private void winRestoreButtonClicked(ImageButton obj)
        {
            Window.ToggleFullscreen();
        }

        private void winFullscreenButtonClicked(ImageButton obj)
        {
            Window.ToggleFullscreen();
        }

        private void winMinimizeButtonClicked(ImageButton obj)
        {
            Window.Minimize();
        }

        private void winResizeGripDragged(Vector2 delta)
        {
            Window.ResizeDrag();
        }

        private void exitApplication(MenuItem item)
        {
            Window.Terminate();
        }

        private void toggleFullscreen(MenuItem item)
        {
            Window.ToggleFullscreen();
        }

        private void toggleVSync(MenuItem item)
        {
            Window.SetVSync(!Window.IsVSync);
        }

        private void toggleGUI(MenuItem item)
        {
            Visible = false;
        }

        private void toggleProfiler(MenuItem obj)
        {
            Editor.Profiler.Visible = !Editor.Profiler.Visible;
        }

        private void authorLink(MenuItem item)
        {
            System.Diagnostics.Process.Start(Config.AUTHOR_URL);
        }
        #endregion


        internal void KeyDown(OpenTK.Input.Key key)
        {
            switch(key)
            {
                case OpenTK.Input.Key.Number1: enableMode(editModes[0]); break;
                case OpenTK.Input.Key.Number2: enableMode(editModes[1]); break;
                case OpenTK.Input.Key.Number3: enableMode(editModes[2]); break;
                case OpenTK.Input.Key.Number4: enableMode(editModes[3]); break;
                case OpenTK.Input.Key.Number5: enableMode(editModes[4]); break;
                case OpenTK.Input.Key.Number6: enableMode(editModes[5]); break;
                case OpenTK.Input.Key.Number7: enableMode(editModes[6]); break;
            }
        }
    }
}
