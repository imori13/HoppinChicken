using FliedChicken.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FliedChicken.GameObjects.PlayerDevices
{
    public enum PlayerMoveState
    {
        None,
        Fall,
        Jump,
    }

    class PlayerMove
    {
        public PlayerMoveState PlayerMoveState { get; private set; }

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
            PlayerMoveState = PlayerMoveState.None;
            inputflag = false;
            fallSpeed = 10;
        }

        public Vector2 Velocity()
        {
            if (DebugMode.DebugFlag)
            {
                return DebugEditVelocity();
            }

            return MoveVelocity();
        }

        public Vector2 Move()
        {
            destPosition += player.Velocity;

            return Vector2.Lerp(player.Position, destPosition, 0.2f);
        }

        Vector2 MoveVelocity()
        {
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * TimeSpeed.Time;

            Vector2 Velocity = player.Velocity;

            PlayerMoveState = PlayerMoveState.None;

            // 落下処理
            bool fallFlag = false;
            float destFallSpeed = 5;
            if (Input.GetKey(Keys.Space))
            {
                fallTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                if (fallTime > 0.2f)
                {
                    destFallSpeed = 15;
                    fallFlag = true;

                    PlayerMoveState = PlayerMoveState.Fall;
                }
            }
            else if (Input.GetKeyUp(Keys.Space))
            {
                fallTime = 0;
            }

            fallSpeed = MathHelper.Lerp(fallSpeed, destFallSpeed, 0.1f);

            // 左右移動

            float speed = (fallFlag) ? (0.5f) : (1.5f);
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
                if (time >= 0.1f)
                {
                    time = 0;
                    inputflag = false;
                    Velocity = new Vector2(Velocity.X, -5);

                    PlayerMoveState = PlayerMoveState.Jump;

                    player.animation = new Animation(player, "PlayerFly", Vector2.One * 114, 3, 0.1f);
                    player.animation.drawSize = Vector2.One * 0.5f;
                    player.animation.RepeatFlag = false;

                    player.animation.Color = (player.MutekiFlag) ? (Color.Yellow) : (Color.White);
                }
                else
                {
                    inputflag = true;

                    PlayerMoveState = PlayerMoveState.Jump;
                }
            }

            Velocity = Vector2.Lerp(Velocity, Vector2.UnitY * fallSpeed, 0.25f);

            return Velocity;
        }

        Vector2 DebugEditVelocity()
        {
            Vector2 destVelocity = Vector2.Zero;

            float speed = 10;

            if (Input.GetKey(Keys.Right))
            {
                destVelocity.X = speed;
            }

            if (Input.GetKey(Keys.Left))
            {
                destVelocity.X = -speed;
            }

            if (Input.GetKey(Keys.Down))
            {
                destVelocity.Y = speed;
            }

            if (Input.GetKey(Keys.Up))
            {
                destVelocity.Y = -speed;
            }

            Vector2 Velocity = player.Velocity;

            Velocity = Vector2.Lerp(Velocity, destVelocity, 0.2f);

            return Velocity;
        }
    }
}
