using FliedChicken.Devices;
using FliedChicken.Scenes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.SceneDevices
{
    class GameScene : IScene
    {
        Camera Camera;

        public GameScene()
        {
            Camera = new Camera();
        }

        public void Initialize()
        {
            Camera.Initialize();
        }

        public void Update()
        {
            Camera.Update();
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin(Camera);

            renderer.Draw2D("packman", Vector2.Zero, Color.White);

            renderer.End();
        }

        public void ShutDown()
        {

        }
    }
}
