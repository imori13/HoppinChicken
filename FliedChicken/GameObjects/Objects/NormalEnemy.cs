using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices.AnimationDevice;

namespace FliedChicken.GameObjects.Objects
{
    class NormalEnemy : Enemy
    {
        public NormalEnemy() : base
            (width: 64 * 3,
            height: 64 * 3,
            collWidth: 64 * 3,
            collHeight: 64 * 3,
            minSpeed: 4,
            maxSpeed: 9,
            minInterval:2,
            maxInterval:4)
        {
            Animation = new Animation("normal_enemy", new Vector2(320, 320), 5, 0.25f);
            Animation.GameObject = this;
        }

        public override Enemy Clone()
        {
            return new NormalEnemy();
        }
    }
}
