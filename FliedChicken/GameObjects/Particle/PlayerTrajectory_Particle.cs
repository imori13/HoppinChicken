using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using Microsoft.Xna.Framework;

namespace FliedChicken.GameObjects.Particle
{
    class PlayerTrajectory_Particle : Particle2D
    {
        Vector2 initScale;

        public PlayerTrajectory_Particle(
            Vector2 position,
            Vector2 direction,
            Random rand)
            : base(
                  "Pixel",
                  rand.Next(2, 4) + (float)rand.NextDouble(),
                  position,
                  direction + (MyMath.DegToVec2(rand.Next(-360, 360) + (float)rand.NextDouble()))*0.5f,
                  rand.Next(2, 4) + (float)rand.NextDouble(),
                  0.98f,
                  Color.Lerp(Color.White, Color.Blue, (float)rand.NextDouble()),
                  1,
                  Vector2.One * (rand.Next(5, 15) + (float)rand.NextDouble()),   // size
                  45, // rotation
                  rand.Next(-90, 90) + (float)rand.NextDouble(),    // rotationSpeed
                  Vector2.One * 0.5f)
        {
            initScale = scale;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update()
        {
            base.Update();

            scale = Vector2.Lerp(initScale, Vector2.Zero, GetAliveRate());
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
        }
    }
}
