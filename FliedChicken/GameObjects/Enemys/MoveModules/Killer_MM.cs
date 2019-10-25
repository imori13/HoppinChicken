using FliedChicken.Devices;
using FliedChicken.GameObjects.Particle;
using FliedChicken.GameObjects.PlayerDevices;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Enemys.MoveModules
{
    class Killer_MM : MoveModule
    {
        Player player;

        public Vector2 Velocity { get; private set; }
        float speed;
        Random rand;

        bool isChase;

        float destAngle;
        float time;
        float limitTime = 0.025f;

        float detectAngle = 0.25f;

        public Killer_MM(GameObject GameObject) : base(GameObject)
        {

        }

        public override void Initialize()
        {
            rand = GameDevice.Instance().Random;
            isChase = false;
            speed = 1.0f;

            player = GameObject.ObjectsManager.Player;

            Velocity = new Vector2(0, -1 * speed);
        }

        public override void Move()
        {
            // 移動
            Vector2 direction = player.Position - GameObject.Position;
            direction.Normalize();

            Vector2 vel = Velocity;
            vel.Normalize();

            isChase = (Vector2.Distance(vel, direction) <= detectAngle) ? (true) : (isChase);

            if (Vector2.Distance(vel, direction) <= detectAngle)
            {
                (GameObject as Enemy).Animation.Color = Color.Red;

                Velocity = Vector2.Lerp(Velocity, direction, 0.1f);
            }
            else
            {
                (GameObject as Enemy).Animation.Color = Color.Yellow;
            }

            GameObject.Position += Velocity * speed * TimeSpeed.Time;

            // 回転
            // 線形補完を使った時３６０度→０度みたいにうごくとぐるっとなるので、それの回避
            float distance = MyMath.Vec2ToDeg(Velocity) - destAngle % 360;
            if (Math.Abs(distance) >= 180)
            {
                distance = (distance > 0) ? (distance - 360) : (distance + 360);
            }
            destAngle += distance;
            (GameObject as Enemy).Animation.Radian = MathHelper.Lerp((GameObject as Enemy).Animation.Radian, MathHelper.ToRadians(destAngle), 0.1f);

            // 速度
            float destSpeed = 0;
            destSpeed = (isChase) ? (7f) : (1f);
            speed = MathHelper.Lerp(speed, destSpeed, 0.02f);

            // パーティクル
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * TimeSpeed.Time;
            if (time >= limitTime)
            {
                time = 0;
                GameObject.ObjectsManager.AddBackParticle(new KillerTrajectory_Particle(GameObject.Position, -Velocity, rand));
            }
        }
    }
}
