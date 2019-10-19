using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Enemys
{
    static class SpawnPosition
    {
        public static Vector2 ScreenUp(Vector2 objectSize)
        {
            return new Vector2(0, Screen.HEIGHT / 2 - objectSize.Y / 2);
        }

        public static Vector2 ScreenDown(Vector2 objectSize)
        {
            return new Vector2(0, Screen.HEIGHT / 2 + objectSize.Y / 2);
        }

        public static Vector2 ScreenDownRandomX(Vector2 objectSize)
        {
            var random = GameDevice.Instance().Random;

            return ScreenDown(objectSize) + new Vector2(random.Next(0, Screen.WIDTH + 1) - Screen.WIDTH / 2, 0);
        }

        public static Vector2 ScreenRight(Vector2 objectSize)
        {
            return new Vector2(Screen.WIDTH / 2 + objectSize.X / 2, 0);
        }

        public static Vector2 ScreenLeft(Vector2 objectSize)
        {
            return new Vector2(-Screen.WIDTH / 2 - objectSize.X / 2, 0);
        }
    }
}
