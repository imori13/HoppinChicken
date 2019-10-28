﻿using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
using FliedChicken.ScenesDevice;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.SceneDevices.Title;
using FliedChicken.GameObjects;
using FliedChicken.GameObjects.Enemys;
using FliedChicken.GameObjects.PlayerDevices;
using FliedChicken.GameObjects.Clouds;
using FliedChicken.GameObjects.Particle;

namespace FliedChicken.SceneDevices
{
    class BeforeFlyScreen
    {
        enum State
        {
            STATE01,
            STATE02,
            STATE03,
            STATE04,
            STATE05,
            FINISH,
        }
        private Player player;
        private DiveEnemy Denemy;

        private Vector2 BasePlayerPosition;
        private Vector2 BaseDenemyPosition;

        public bool IsDead { get; private set; }

        private State state;

        private float time;

        private int attackCount;
        private int attackCountNow;
        private bool attack;


        private readonly string text = "ESCAPE!";
        private Vector2 textPosition;

        CloudManager cloudManager;
        Camera camera;

        private Vector2 downSpeed;

        float normalParticleTime = 0;

        Random rand = GameDevice.Instance().Random;

        public BeforeFlyScreen()
        {
            
        }

        public void Initialize(Player player, DiveEnemy Denemy, 
            CloudManager cloudManager, Camera camera)
        {
            this.player = player;
            this.Denemy = Denemy;

            BasePlayerPosition = camera.Position;
            BaseDenemyPosition = BasePlayerPosition - new Vector2(0, 300); 

            player.state = Player.PlayerState.BEFOREFLY;
            Denemy.state = DiveEnemy.State.BEFOREFLY;

            IsDead = false;
            state = State.STATE01;

            time = 0.0f;

            attackCount = 2;
            attackCountNow = 0;
            attack = true;

            textPosition = new Vector2(Screen.Vec2.X + 400, Screen.Vec2.Y / 2);

            this.cloudManager = cloudManager;
            this.camera = camera;

            downSpeed = new Vector2(0, 11);
        }

        public  void Update()
        {
            Default();
            switch (state)
            {
                case State.STATE01:
                    State01();
                    break;
                case State.STATE02:
                    State02();
                    break;
                case State.STATE03:
                    State03();
                    break;
                case State.STATE04:
                    State04();
                    break;
                case State.STATE05:
                    State05();
                    break;
                case State.FINISH:
                    Finish();
                    break;
            }
        }

        public void Draw(Renderer renderer)
        {
            renderer.DrawString(Fonts.Font10_128, text, textPosition, new Color(255, 91, 91),
                0.0f, Fonts.Font10_128.MeasureString(text) / 2, Vector2.One);
        }

        private void Default()
        {
            camera.Position += downSpeed;
            player.Position += downSpeed;
            Denemy.Position += downSpeed;

            float limit = 0.1f;
            normalParticleTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * TimeSpeed.Time;
            while (normalParticleTime >= limit)
            {
                normalParticleTime -= limit;
                player.ObjectsManager.AddBackParticle(
                    new PlayerTrajectory_Particle(player.Position + MyMath.RandomCircleVec2() * 10f, -player.Velocity / 100f, rand));
            }

            BasePlayerPosition = camera.Position;
            BaseDenemyPosition = BasePlayerPosition - new Vector2(0, 300);
            cloudManager.Update();
        }

        private void State01()
        {
            player.Position = new Vector2(MathHelper.Lerp(player.Position.X, BasePlayerPosition.X, 0.1f), MathHelper.Lerp(player.Position.Y, BasePlayerPosition.Y, 0.1f));

            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            if (time >= 1.0f)
            {
                time = 0.0f;
                Denemy.Position = BaseDenemyPosition;
                state = State.STATE02;
            }
        }

        private void State02()
        {
            Denemy.Position += new Vector2(0, 30) * TimeSpeed.Time;
            if (Denemy.Position.Y >= player.Position.Y - 200)
            {
                time = 0.0f;
                state = State.STATE03;
            }
        }

        private void State03()
        {
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            if(time >= 0.5f)
            {
                time = 0.0f;
                state = State.STATE04;
            }
        }

        private void State04()
        {
            DenemyAttack();
            if (attackCountNow >= attackCount)
            {
                time = 0.0f;
                state = State.STATE05;
            }
        }

        private void State05()
        {
            textPosition.X = MathHelper.Lerp(textPosition.X, Screen.Vec2.X / 2, 0.1f);
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            if (time >= 1.5f)
            {
                time = 0.0f;
                state = State.FINISH;
            }
        }

        private void Finish()
        {
            textPosition.X = MathHelper.Lerp(textPosition.X, -400, 0.1f);
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            if (time >= 1.0f)
            {
                time = 0.0f;
                player.state = Player.PlayerState.FLY;
                Denemy.state = DiveEnemy.State.FORMING;
                IsDead = true;
            }

        }

        public void End()
        {

        }

        private void DenemyAttack()
        {
            if (attack)
            {
                Denemy.Position += new Vector2(0, 30);
                if (Denemy.Position.Y >= player.Position.Y)
                {
                    attack = false;
                }
            }
            else
            {
                Denemy.Position -= new Vector2(0, 15);
                if (Denemy.Position.Y <= player.Position.Y - 200)
                {
                    attack = true;
                    attackCountNow++;
                }
            }
        }
    }
}
