using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using Microsoft.Xna.Framework;

namespace FliedChicken.GameObjects.Particle
{
    class ExplosionParticle2D : Particle2D
    {
        Vector2 initScale;

        public ExplosionParticle2D(
            Vector2 position,
            Vector2 direction,
            Color color,
            Random rand
            ) : base(
                "Pixel",
                rand.Next(0, 1) + (float)rand.NextDouble(),
                position,
                direction,
                rand.Next(25, 50) + (float)rand.NextDouble(),  // speed
                0.9f,   // friction
                Color.Lerp(color, Color.White, (float)rand.NextDouble()),
                1,  // alpha
                Vector2.One * (rand.Next(20, 100) + (float)rand.NextDouble()),  // scale
                rand.Next(360) + (float)rand.NextDouble(),   // rotation
                rand.Next(-45, 45) + (float)rand.NextDouble(), // rotation_speed
                Vector2.One / 2f  // origin
                )
        {
            initScale = scale;
        }

        public override void Initialize()
        {
            speed *= 1 / scale.Length() * 25;

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
            speed *= friction + (1 - TimeSpeed.Time) * 0.1f;

            // 座標=移動角度*速度
            position += direction * speed * TimeSpeed.Time;

            scale = Vector2.Lerp(initScale, Vector2.Zero, GetAliveRate());
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
        }
    }
}
