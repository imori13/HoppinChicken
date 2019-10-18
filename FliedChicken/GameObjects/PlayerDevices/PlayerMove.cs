using FliedChicken.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FliedChicken.GameObjects.PlayerDevices
{
    class PlayerMove
    {
        Player player;

        private float fallSpeed;

        private Vector2 destPosition;

        private float time;
        private bool inputflag;

        private float fallTime;

        public PlayerMove(Player player)
        {
            this.player = player;
        }

        public void Initialize()
        {
            inputflag = false;
            fallSpeed = 10;
        }

        public Vector2 Velocity()
        {
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

            Vector2 Velocity = player.Velocity;

            // 落下処理
            bool fallFlag = false;
            float destFallSpeed = 10;
            if (Input.GetKey(Keys.Space))
            {
                fallTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                if (fallTime > 0.2f)
                {
                    destFallSpeed = 20;
                    fallFlag = true;
                }
            }
            else if (Input.GetKeyUp(Keys.Space))
            {
                fallTime = 0;
            }

            fallSpeed = MathHelper.Lerp(fallSpeed, destFallSpeed, 0.1f);

            // 左右移動

            float speed = (fallFlag) ? (1) : (5);
            if (Input.GetKey(Keys.Right))
            {
                Velocity = new Vector2(Velocity.X + speed, Velocity.Y) * TimeSpeed.Time;
            }

            if (Input.GetKey(Keys.Left))
            {
                Velocity = new Vector2(Velocity.X - speed, Velocity.Y) * TimeSpeed.Time;
            }

            // ジャンプ処理
            if (Input.GetKeyDown(Keys.Space) || inputflag)
            {
                if (time >= 0.15f)
                {
                    time = 0;
                    inputflag = false;
                    Velocity = new Vector2(Velocity.X, -15);
                }
                else
                {
                    inputflag = true;
                }
            }

            Velocity = Vector2.Lerp(Velocity, Vector2.UnitY * fallSpeed, 0.25f);

            return Velocity;
        }

        public Vector2 Move()
        {
            destPosition += player.Velocity;

            return Vector2.Lerp(player.Position, destPosition, 0.2f);
        }
    }
}
