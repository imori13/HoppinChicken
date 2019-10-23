using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FliedChicken.Devices;
using Microsoft.Xna.Framework;

namespace FliedChicken.GameObjects.Particle
{
    class LingerRotateParticle2D : Particle2D
    {
        private Vector2 originScale;
        private Vector2 maxScale;
        private Random rand;

        public LingerRotateParticle2D(
            Vector2 position,
            Random rand)
            : base(
                  "Pixel",
                  rand.Next(1, 3 + 1) + (float)rand.NextDouble(),
                  position,
                  Vector2.Zero,
                  0,
                  0,
                  Color.White,
                  0.8f,
                  Vector2.One * rand.Next(16, 32 + 1),
                  rand.Next(-180, 180),
                  rand.Next(120, 180),
                  new Vector2(0.5f, 0.5f))
        {
            this.rand = rand;
            originScale = scale;
            maxScale = scale * 2;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update()
        {
            base.Update();

            alpha = MathHelper.Clamp(1.0f - aliveRate, 0.0f, 0.8f);
            color = Color.White * alpha;
            scale = Easing2D.SineOut(aliveTime, aliveLimit, originScale, maxScale);
        }
    }
}
