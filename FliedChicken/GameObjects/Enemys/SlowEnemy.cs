using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices.AnimationDevice;
using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Enemys
{
    class SlowEnemy : Enemy
    {
        public SlowEnemy(Camera camera) : base(camera)
        {
            Animation = new Animation(this, "slowenemy", new Vector2(445, 165), 8, 0.25f);
            Animation.drawSize = Vector2.One;
        }

        public override void HitAction(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        protected override bool IsDestroy()
        {
            throw new NotImplementedException();
        }

        protected override void OnDestroy()
        {
            throw new NotImplementedException();
        }
    }
}
