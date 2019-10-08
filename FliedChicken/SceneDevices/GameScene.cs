using FliedChicken.Devices;
using FliedChicken.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.SceneDevices
{
    class GameScene : IScene
    {
        public void Initialize()
        {

        }

        public void Update()
        {

        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();

            renderer.End();
        }

        public void ShutDown()
        {

        }
    }
}
