using System;
using FliedChicken.Devices;
using Microsoft.Xna.Framework;

namespace FliedChicken.GameObjects.Particle
{
    class RadiationParticle2D : Particle2D
    {
        Random rand;

        public RadiationParticle2D(
            Vector2 position,
            Color color,
            Vector2 direction,
            Random rand)
            : base(
                  "Pixel",
                  rand.Next(1, 10) + (float)rand.NextDouble(),
                  position + direction * (rand.Next(80, 100) + (float)rand.NextDouble() * Screen.ScreenSize),
                  direction,
                  rand.Next(50,150) + (float)rand.NextDouble(), // speed
                  0.9f,    // friction
                  Color.Lerp(Color.White, color, (float)rand.NextDouble()),
                  0,
                  Vector2.One * 10,   // scale
                  MyMath.Vec2ToDeg(direction) - 90,    // rotation
                  0,
                  new Vector2(0.5f, 1))
        {
            this.rand = rand;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update()
        {
            aliveTime += (float)gameDevice.GameTime.ElapsedGameTime.TotalSeconds * TimeSpeed.Time;  // 時間数えてる
            aliveRate = aliveTime / aliveLimit; // レート=現在時間/生存時間  0.25=1/4 4秒のエフェクトでいま1秒なら0.25
            if (aliveTime > aliveLimit) // 生存時間超えたらしぬ
            {
                IsDead = true;
            }

            // 回転速度を回転パラメータにぶち込み続ける
            rotation = MathHelper.Lerp(rotation_start, rotation_dest, GetAliveRate());

            // 速度*摩擦 どんどん移動速度が落ちるとかに
            speed *= friction + (1 - TimeSpeed.Time) * 0.05f;

            // 座標=移動角度*速度
            position += direction * speed * TimeSpeed.Time;

            if (speed <= 0.01f)
            {
                IsDead = true;
            }

            scale = new Vector2(scale.X + speed * 0.01f, scale.Y + speed*0.1f);
            scale = Vector2.Lerp(scale, Vector2.Zero, aliveRate);
        }
    }
}
