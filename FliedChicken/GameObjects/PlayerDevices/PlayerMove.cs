using FliedChicken.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.PlayerDevices
{
    class PlayerMove
    {
        Player player;

        private static readonly float FALLMAXSPEED = 10;

        public Vector2 Velocity { get; private set; }   // 保守性がなくてすまんが！！！これをPlayerクラスでVelocity = playerMove.Velocityするのを忘れないでくれ！！！
        private Vector2 destPosition;

        private float time;
        private bool inputflag;

        public PlayerMove(Player player)
        {
            this.player = player;
        }

        public void Initialize()
        {
            inputflag = false;
        }

        public Vector2 Update()
        {
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

            Vector2 Vel = player.Velocity;

            // 落下処理
            Vel = Vector2.Lerp(Vel, Vector2.UnitY * FALLMAXSPEED, 0.25f);

            // ジャンプ処理
            if (Input.GetKeyDown(Keys.Space) || inputflag)
            {
                if (time >= 0.15f)
                {
                    time = 0;
                    inputflag = false;
                    Vel = new Vector2(Vel.X, -25);
                }
                else
                {
                    inputflag = true;
                }
            }

            float speed = 5;

            if (Input.GetKey(Keys.Right))
            {
                Vel = new Vector2(Vel.X + speed, Vel.Y) * TimeSpeed.Time;
            }

            if (Input.GetKey(Keys.Left))
            {
                Vel = new Vector2(Vel.X - speed, Vel.Y) * TimeSpeed.Time;
            }

            destPosition += Vel;

            this.Velocity = Vel;

            return Vector2.Lerp(player.Position, destPosition, 0.2f);
        }
    }
}
