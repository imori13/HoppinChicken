using FliedChicken.Devices;
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

        private double radius;

        private float time01;
        private float time03;
        private float time05;
        private float timeF;

        private int attackCount;
        private int attackCountNow;
        private bool attack;


        private readonly string text = "ESCAPE!";
        private Vector2 textPosition;

        CloudManager cloudManager;
        Camera camera;

        public BeforeFlyScreen()
        {
            
        }

        public void Initialize(Player player, DiveEnemy Denemy, 
            CloudManager cloudManager, Camera camera)
        {
            this.player = player;
            this.Denemy = Denemy;

            BasePlayerPosition = player.Position;
            BaseDenemyPosition = Denemy.Position; 

            player.state = Player.PlayerState.BEFOREFLY;
            Denemy.state = DiveEnemy.State.BEFOREFLY;

            IsDead = false;
            state = State.STATE01;

            radius = 0.0f;
            time01 = 0.0f;
            time03 = 0.0f;
            time05 = 0.0f;
            timeF = 0.0f;

            attackCount = 2;
            attackCountNow = 0;
            attack = true;

            textPosition = new Vector2(Screen.Vec2.X + 400, Screen.Vec2.Y / 2);

            this.cloudManager = cloudManager;
            this.camera = camera;
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
            renderer.DrawString(Fonts.Font10_128, text, textPosition, Color.Red,
                0.0f, Fonts.Font10_128.MeasureString(text) / 2, Vector2.One);
        }

        private void Default()
        {
            player.animation.Update();
            cloudManager.Update();
        }

        private void State01()
        {
            radius += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * Math.PI;
            float sin = (float)Math.Sin(radius) * 30 + 10;
            player.Position = BasePlayerPosition + new Vector2(0, sin);
            time01 += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            if (time01 >= 1.0f)
            {
                state = State.STATE02;
            }
        }

        private void State02()
        {
            radius += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * Math.PI;
            float sin = (float)Math.Sin(radius) * 30 + 10;
            player.Position = BasePlayerPosition + new Vector2(0, sin);
            Denemy.Position += new Vector2(0, 30) * TimeSpeed.Time;
            if (Denemy.Position.Y >= player.Position.Y - 200)
            {
                state = State.STATE03;
            }
        }

        private void State03()
        {
            time03 += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            if(time03 >= 0.5f)
            {
                state = State.STATE04;
            }
        }

        private void State04()
        {
            DenemyAttack();
            if (attackCountNow >= attackCount)
            {
                state = State.STATE05;
            }
        }

        private void State05()
        {
            textPosition.X = MathHelper.Lerp(textPosition.X, Screen.Vec2.X / 2, 0.1f);
            time05 += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            if (time05 >= 1.5f)
            {
                state = State.FINISH;
            }
        }

        private void Finish()
        {
            textPosition.X = MathHelper.Lerp(textPosition.X, -400, 0.1f);
            //player.Position = new Vector2(player.Position.X, MathHelper.Lerp(player.Position.Y, BasePlayerPosition.Y, 0.01f));
            //Denemy.Position = new Vector2(Denemy.Position.X, MathHelper.Lerp(Denemy.Position.Y, BaseDenemyPosition.Y, 0.01f));
            timeF += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            if (timeF >= 1.0f)
            {
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
