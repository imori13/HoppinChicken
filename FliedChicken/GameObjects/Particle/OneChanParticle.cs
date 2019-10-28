using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Particle
{
    class OneChanParticle : Particle2D
    {
        private GameObject followObject;
        private Vector2 originScale;

        public OneChanParticle(GameObject followObject, Vector2 scale, float rotation)
            : base("Star",
                  1.0f,
                  followObject.Position,
                  Vector2.Zero,
                  0,
                  0,
                  Color.White,
                  1.0f,
                  scale,
                  rotation,
                  360 * 1,
                  new Vector2(96, 96) * 0.5f)
        {
            this.followObject = followObject;
            originScale = scale;
        }

        public override void Update()
        {
            position = followObject.Position;
            scale = originScale * (1 - GetAliveRate());

            base.Update();
        }
    }
}
