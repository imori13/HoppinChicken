using FliedChicken.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.Scenes
{
    class SceneManager
    {
        private Dictionary<SceneEnum, IScene> scenes;
        private IScene currentScene;

        public SceneManager()
        {
            scenes = new Dictionary<SceneEnum, IScene>();
        }

        public void AddScene(SceneEnum sceneEnum, IScene scene)
        {
            Debug.Assert(scene != null, "追加されたシーン[ " + sceneEnum + " ]がnullです");
            Debug.Assert(!scenes.ContainsKey(sceneEnum), "すでに同じシーンEnumが使われています[ " + sceneEnum + " ]");

            scenes.Add(sceneEnum, scene);
        }

        public void ChangeScene(SceneEnum sceneEnum)
        {
            Debug.Assert(scenes.ContainsKey(sceneEnum), "変更指定したシーンがまだ登録されていません[ " + sceneEnum + " ]");

            if (currentScene != null)
            {
                currentScene.ShutDown();
            }

            currentScene = scenes[sceneEnum];
            currentScene.Initialize();
        }

        public void Update()
        {
            if (currentScene == null) { return; }
            currentScene.Update();
        }

        public void Draw(Renderer renderer)
        {
            if (currentScene == null) { return; }
            currentScene.Draw(renderer);
        }
    }
}
