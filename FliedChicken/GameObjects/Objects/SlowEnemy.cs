using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices.AnimationDevice;

namespace FliedChicken.GameObjects.Objects
{
    class SlowEnemy : Enemy
    {
        public SlowEnemy() : base
            (width: 64 * 10,
             height: 64 * 8,
             collWidth: 64 * 10,
             collHeight: 64 * 8,
             minSpeed: 1,
             maxSpeed: 3,
             minInterval: 12,
             maxInterval: 15)
        {
            Animation = new Animation("slowenemy", new Vector2(445, 165), 8, 0.25f);
            Animation.GameObject = this;
        }

        public override Enemy Clone()
        {
            return new SlowEnemy();
        }
    }
}
