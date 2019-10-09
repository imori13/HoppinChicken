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
            backGround = new BackGround(new Vector2(0, 832 * 2), new Vector2(0, -832 * 2));
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

            renderer.Draw2D("packman", Vector2.Zero, Color.White);
            backGround.Draw(renderer);

            renderer.End();
        }

        public void ShutDown()
        {

        }
    }
}
