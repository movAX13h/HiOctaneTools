using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LevelEditor
{
    class Config
    {
        public static string DATA_FOLDER = "../data/hioctane/";
        public static string LOG_FILENAME = DATA_FOLDER + "logs.txt";

        public static int DEFAULT_LEVEL_NUMBER = 1;

        public static string SHADER_FOLDER = DATA_FOLDER + "shaders/";
        public static string SHADER_VERT_EXTENSION = ".vs";
        public static string SHADER_FRAG_EXTENSION = ".fs";

        public static string BRUSHES_SUBFOLDER = "gui/brushes/";
        public static string FONT_DEFAULT = DATA_FOLDER + "gui/fonts/andalemo.ttf";

        public static string AUTHOR_URL = "http://blog.thrill-project.com/";

        public static Color UI_COLOR_BLUE = Color.FromArgb(0, 122, 204);
        public static Color UI_COLOR_ORANGE = Color.FromArgb(202, 81, 0);
    }
}
