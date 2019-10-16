using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices.AnimationDevice;

namespace FliedChicken.GameObjects.Objects
{
    class HighSpeedEnemy : Enemy
    {
        public HighSpeedEnemy() : base
            (width: 64 * 6,
             height: 64 * 4,
             collWidth: 64 * 6,
             collHeight: 64 * 4,
             minSpeed: 16,
             maxSpeed: 18,
             minInterval: 7,
             maxInterval: 10)
        {
            Animation = new Animation("highspeed_enemy", new Vector2(400, 140), 6, 0.25f);
            Animation.GameObject = this;
        }

        public override Enemy Clone()
        {
            return new HighSpeedEnemy();
        }
    }
}
