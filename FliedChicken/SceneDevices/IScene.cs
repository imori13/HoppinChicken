using FliedChicken.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.Scenes
{
    interface IScene
    {
        void Initialize();
        void Update();
        void Draw(Renderer renderer);
        void ShutDown();
    }
}
