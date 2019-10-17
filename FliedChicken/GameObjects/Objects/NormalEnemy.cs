using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices.AnimationDevice;
using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Objects
{
    class NormalEnemy : Enemy
    {
        public NormalEnemy() : base
            (width: 320 * 0.5f,
            height: 320 * 0.5f,
            collWidth: 320 * 0.4f,
            collHeight: 320 * 0.2f,
            minSpeed: 5,
            maxSpeed: 8,
            minInterval:2,
            maxInterval:4)
        {
            Animation = new Animation("normal_enemy", new Vector2(320, 320), 5, 0.25f);
            Animation.GameObject = this;
            Animation.Size = Vector2.One * 0.5f;
        }

        public override Enemy Clone()
        {
            return new NormalEnemy();
        }
    }
}
