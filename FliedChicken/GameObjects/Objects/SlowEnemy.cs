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
            (width: 445 * 0.9f,
             height: 165 * 0.9f,
             collWidth: 445 * 0.8f,
             collHeight: 165 * 0.6f,
             minSpeed: 1,
             maxSpeed: 3,
             minInterval: 12,
             maxInterval: 15)
        {
            Animation = new Animation("slowenemy", new Vector2(445, 165), 8, 0.25f);
            Animation.GameObject = this;
            Animation.Size = Vector2.One;
        }

        public override Enemy Clone()
        {
            return new SlowEnemy();
        }
    }
}
