using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;

using LevelEditor.Engine.Core;
using LevelEditor.Utils;
using LevelEditor;
using LevelEditor.Engine.Resources;
using LevelEditor.Engine.GUI.Controls;

namespace LevelEditor.Engine
{
    class EngineBase
    {
        private static Renderer renderer;
        private static RootNode currentScene;
        private static ResourceManager resourceManager;

        public static Renderer Renderer { get { return renderer; } }
        public static RootNode Scene { get { return currentScene; } }
        public static ResourceManager Manager { get { return resourceManager; } }

        public bool Ready { get { return ready; } }
        private bool ready;

        private Panel overlay = null;

        public EngineBase()
        {
            if (Directory.Exists(Config.DATA_FOLDER))
            {
                Log.Filename = Config.LOG_FILENAME;

                resourceManager = new ResourceManager();
                renderer = new Renderer();
                ready = renderer.Ready;
            }
            else ready = false;
        }

        public void ClearColor(Color color)
        {
            renderer.ClearColor(color);
        }

        public bool SetScene(RootNode scene)
        {
            scene.Setup();

            if (scene.Ready)
            {
                currentScene = scene;
                return true;
            }
            return false;
        }

        public void SetOverlay(Panel panel)
        {
            overlay = panel;
        }

        public void SetCamera(CameraNode camera)
        {
            renderer.SetCamera(camera);
        }

        public void OnResize(Rectangle clientRectangle, int width, int height)
        {
            renderer.Resize(clientRectangle, width, height);
        }

        public void Render()
        {
            if (currentScene != null) renderer.Render(currentScene, overlay);
        }
    }
}
