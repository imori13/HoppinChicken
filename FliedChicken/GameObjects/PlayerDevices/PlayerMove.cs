using FliedChicken.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FliedChicken.GameObjects.PlayerDevices
{
    public enum PlayerMoveState
    {
        None,
        Jump,
    }

    class PlayerMove
    {
        public PlayerMoveState PlayerMoveState { get; private set; }

        Player player;

        public float FallSpeed { get; private set; }

        private Vector2 destPosition;

        private float time;
        private bool inputflag;

        private float fallTime;
        Random rand = GameDevice.Instance().Random;

        public PlayerMove(Player player)
        {
            this.player = player;
        }

        public void Initialize()
        {
            PlayerMoveState = PlayerMoveState.None;
            inputflag = false;
            FallSpeed = 10;
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
            destPosition += player.Velocity * TimeSpeed.Time;

            return Vector2.Lerp(player.Position, destPosition, 0.2f * TimeSpeed.Time);
        }

        Vector2 MoveVelocity()
        {
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * TimeSpeed.Time;

            Vector2 Velocity = player.Velocity;

            PlayerMoveState = PlayerMoveState.None;

            float destFallSpeed = 10f;

            // 左右移動

            float speed = 8f;
            if ((Input.GetKey(Keys.Right) || Input.GetLeftStickState(0).X > 0.5f || Input.IsPadButtonHold(Buttons.DPadRight, 0) || Input.GetRightStickState(0).X > 0.5f)
                && !player.ObjectsManager.GameScene.TitleDisplayMode.RankingON)
            {
                Velocity.X = MathHelper.Lerp(Velocity.X, speed, 0.1f * TimeSpeed.Time);
                destFallSpeed = 8f;
            }
            else if ((Input.GetKey(Keys.Left) || Input.GetLeftStickState(0).X < -0.5f || Input.IsPadButtonHold(Buttons.DPadLeft, 0) || Input.GetRightStickState(0).X < -0.5f)
                && !player.ObjectsManager.GameScene.TitleDisplayMode.RankingON)
            {
                Velocity.X = MathHelper.Lerp(Velocity.X, -speed, 0.1f * TimeSpeed.Time);
                destFallSpeed = 7.5f;
            }
            else
            {
                Velocity.X = MathHelper.Lerp(Velocity.X, 0, 0.1f * TimeSpeed.Time);
            }

            // ジャンプ処理
            if ((Input.GetKeyDown(Keys.Space) || Input.IsPadButtonDown(Buttons.B, 0) || Input.IsPadButtonDown(Buttons.A, 0) || inputflag)
                && !player.ObjectsManager.GameScene.TitleDisplayMode.RankingON)
            {
                if (time >= 0.1f)
                {
                    time = 0;
                    inputflag = false;
                    Velocity = new Vector2(Velocity.X, -10);

                    player.PlayerScale.Jump();

                    PlayerMoveState = PlayerMoveState.Jump;

                    GameDevice.Instance().Sound.PlaySE("Jump0" + rand.Next(1, 6).ToString());
                }
                else
                {
                    inputflag = true;

                    PlayerMoveState = PlayerMoveState.Jump;
                }
            }

            FallSpeed = MathHelper.Lerp(FallSpeed, destFallSpeed, 0.1f);

            Velocity.Y = MathHelper.Lerp(Velocity.Y, FallSpeed, 0.1f * TimeSpeed.Time);

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
