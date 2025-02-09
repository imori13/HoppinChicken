﻿using FliedChicken.Devices;
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

        float detectAngle = 0.5f;
        float chaseAngle = 0.08f;

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

            float angleDiff = Vector2.Distance(vel, direction);

            if (!isChase)
            {
                if ((angleDiff <= detectAngle) && (Vector2.Distance(GameObject.Position, player.Position) <= 500f))
                {
                    isChase = true;
                    ((Enemy)GameObject).Animation = new Animation(GameObject, "Killer_Active", new Vector2(60, 60), 1, 10000);
                }
            }
            else
            {
                if (angleDiff <= detectAngle)
                {
                    if (angleDiff > chaseAngle)
                        Velocity = Vector2.Lerp(Velocity, direction, 0.1f);
                }
            }

            if (Velocity.Y >= 0)
            {
                Velocity = new Vector2(Velocity.X, 0);
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
            (GameObject as Enemy).Animation.Radian = MathHelper.Lerp((GameObject as Enemy).Animation.Radian, MathHelper.ToRadians(destAngle + 90), 0.1f);

            // 速度
            float destSpeed = 0;
            destSpeed = (isChase) ? (16f) : (1f);
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
