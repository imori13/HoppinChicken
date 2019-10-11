using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
using FliedChicken.Objects;
using FliedChicken.Particle;
using FliedChicken.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.SceneDevices
{
    class ResultScreen
    {
        private float clearTime;

        public ResultScreen()
        {

        }

        public void Draw(Renderer renderer)
        {

        }

        public void SetScore(float time)
        {
            clearTime = time;
        }
    }
}
