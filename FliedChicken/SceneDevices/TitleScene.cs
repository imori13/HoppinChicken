using FliedChicken.Devices;
using FliedChicken.Scenes;
using FliedChicken.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.SceneDevices
{
    class TitleScene : IScene
    {
        T_KeyWindow keyWindow;

        public TitleScene()
        {
            keyWindow = new T_KeyWindow();
        }

        public void Initialize()
        {
            keyWindow.Initialize();
        }

        public void Update()
        {
            keyWindow.Update();
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            keyWindow.Draw(renderer);
            renderer.End();
        }

        public void ShutDown()
        {

        }
    }
}
