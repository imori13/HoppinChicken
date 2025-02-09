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

        public bool IsDead { get; private set; }

        private State state;

        private float time;

        private int attackCount;
        private int attackCountNow;
        private bool attack;
        Vector2 offset = Vector2.Zero;

        private readonly string text = "ESCAPE!";
        private Vector2 textPosition;

        Camera camera;

        public BeforeFlyScreen()
        {

        }

        public void Initialize(Player player, DiveEnemy Denemy,
            CloudManager cloudManager, Camera camera)
        {
            this.player = player;
            this.Denemy = Denemy;

            player.state = Player.PlayerState.BEFOREFLY;
            Denemy.state = DiveEnemy.State.BEFOREFLY;

            IsDead = false;
            state = State.STATE01;

            time = 0.0f;

            attackCount = 2;
            attackCountNow = 0;
            attack = true;

            textPosition = new Vector2(Screen.Vec2.X + 400, Screen.Vec2.Y / 2);
            
            this.camera = camera;
        }

        public void Update()
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
            renderer.Draw2D("ESCAPE!!", textPosition, Color.White, 0);
        }

        private void Default()
        {

        }

        private void State01()
        {
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            if (time >= 0.5f)
            {
                Denemy.Position = player.Position - new Vector2(0, 800);
                time = 0.0f;
                state = State.STATE02;
            }
        }

        private void State02()
        {
            state = State.STATE03;
        }

        private void State03()
        {
            Denemy.Position = Vector2.Lerp(Denemy.Position, player.Position - new Vector2(0, 300), 0.1f);
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            if (time >= 0.5f)
            {
                time = 0.0f;
                offset = Vector2.Zero;
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
            Denemy.Position = Vector2.Lerp(Denemy.Position, player.Position - new Vector2(0, 300), 0.1f);
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
            Denemy.Position = Vector2.Lerp(Denemy.Position, player.Position - new Vector2(0, 300), 0.1f);
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
            Vector2 destOffset = Vector2.Zero;

            if (attack)
            {
                destOffset = new Vector2(0, -100);
                offset = Vector2.Lerp(offset, destOffset, 0.1f);

                if (Vector2.Distance(offset, destOffset) <= 30f)
                {
                    attack = false;
                }
            }
            else
            {
                destOffset = new Vector2(0, 300);
                offset = Vector2.Lerp(offset, destOffset, 0.2f);

                if (Vector2.Distance(offset, destOffset) <= 30f)
                {
                    GameDevice.Instance().Sound.PlaySE("DiveEnemySound");
                    attack = true;
                    attackCountNow++;
                }
            }

            Denemy.Position = Vector2.Lerp(Denemy.Position, player.Position - new Vector2(0, 300) + offset, 0.1f);
        }
    }
}
