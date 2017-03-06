using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenTK;
using LevelEditor.Engine.GUI.Controls;
using System.IO;

namespace LevelEditor.Games.HiOctane.Controls
{
        public class BrushSelector : Panel
        {
            private List<ImageButton> brushes;
            public ImageButton SelectedBrush { get; private set; }

            private Panel brushesList;

            public BrushSelector() : base(new Vector2(40, 40))
            {
                Name = "Brush Selector";
                MouseEnabled = false; // avoid click-through

                BorderSize = 1;

                brushesList = new Panel(new Vector2(484, 204));
                brushesList.MouseEnabled = true;
                brushesList.Pos.X = Size.X + 2;
                brushesList.Pos.Y = Size.Y - brushesList.Size.Y + 4;

                brushes = new List<ImageButton>();
                string directory = Path.Combine(Config.DATA_FOLDER, Config.BRUSHES_SUBFOLDER);

                string[] files = Directory.GetFiles(directory, "*.jpg");

                float x = 4;
                float y = brushesList.Size.Y - 40;

                foreach(string filename in files)
                {
                    string basename = Path.GetFileName(filename);
                    ImageButton btn = new ImageButton(new Vector2(36, 36), Path.Combine(Config.BRUSHES_SUBFOLDER, basename),
                        null, brushClicked);
                    btn.Pos.X = x;
                    btn.Pos.Y = y;
                    brushes.Add(btn);
                    brushesList.AddChild(btn);

                    x += btn.Size.X + 4;
                    if (x + btn.Size.X + 4 > brushesList.Size.X)
                    {
                        x = 4;
                        y -= btn.Size.Y + 4;
                    }
                }

                selectBrush(0);
            }

            private Vector2 brushPos;

            private void selectBrush(int id)
            {
                if (id < 0 && id > brushes.Count - 1) return;
                selectBrush(brushes[id]);
            }

            private void selectBrush(ImageButton brush)
            {
                if (SelectedBrush != null)
                {
                    SelectedBrush.Pos.X = brushPos.X;
                    SelectedBrush.Pos.Y = brushPos.Y;
                    brushesList.AddChild(SelectedBrush);
                }

                brushPos.X = brush.Pos.X;
                brushPos.Y = brush.Pos.Y;
                SelectedBrush = brush;
                SelectedBrush.Pos.X = 2;
                SelectedBrush.Pos.Y = 2;

                AddChild(SelectedBrush);
            }

            private void brushClicked(ImageButton brush)
            {
                if (brush == SelectedBrush)
                {
                    if (brushesList.Parent == null) AddChild(brushesList);
                    else RemoveChild(brushesList);
                }
                else
                {
                    selectBrush(brush);
                    RemoveChild(brushesList);
                }
            }



        }

}
