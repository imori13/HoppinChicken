using FliedChicken.Devices;
using FliedChicken.Scenes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FliedChicken.GameObjects.Objects;

namespace FliedChicken.SceneDevices
{
    class GameScene : IScene
    {
        Camera Camera;
        BackGround backGround;

        public GameScene()
        {
            Camera = new Camera();
            backGround = new BackGround(Camera);
        }

        public void Initialize()
        {
            Camera.Initialize();
            backGround.Initialize();
        }

        public void Update()
        {
            Camera.Update();
            backGround.Update();
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin(Camera);

            backGround.Draw(renderer);
            renderer.Draw2D("packman", Vector2.Zero, Color.White);

            renderer.End();
        }

        public void ShutDown()
        {

        }
    }
}
