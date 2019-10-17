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
            (width: 400 * 0.9f,
             height: 140 * 0.9f,
             collWidth: 400 * 0.8f,
             collHeight: 140 * 0.55f,
             minSpeed: 10,
             maxSpeed: 13,
             minInterval: 7,
             maxInterval: 10)
        {
            Animation = new Animation("highspeed_enemy", new Vector2(400, 140), 6, 0.25f);
            Animation.GameObject = this;
            Animation.Size = Vector2.One;
        }

        public override Enemy Clone()
        {
            return new HighSpeedEnemy();
        }
    }
}
