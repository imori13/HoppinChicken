using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FliedChicken.GameObjects.Collision;
using FliedChicken.GameObjects.PlayerDevices;
using FliedChicken.Devices;
using FliedChicken.Devices.AnimationDevice;

namespace FliedChicken.GameObjects.Enemys
{
    class DiveEnemy : GameObject
    {
        public enum State
        {
            BEFOREFLY,
            FORMING,
            STOP
        }

        private Camera camera;
        private Player player;

        private float sinWidth;
        private float elapsedTime;

        private float speedX;
        private float speedY;
        private Vector2 basePosition;

        private SpriteEffects spriteEffects;

        private Animation Animation;

        private readonly float stopTime = 2.0f;
        private float stopCount;

        public State state;

        public DiveEnemy(Camera camera, Player player)
        {
            this.camera = camera;
            this.player = player;

            Collider = new BoxCollider(this, Vector2.One * 3);
            GameObjectTag = GameObjectTag.DiveEnemy;

            speedX = 2;
            speedY = 10;
            sinWidth = 64 * 8;
            elapsedTime = 0.0f;

            Animation = new Animation(this, "DiveEnemy", new Vector2(297, 192), 5, 0.1f);
            Animation.drawSize = Vector2.One * 0.5f;
        }

        public override void Initialize()
        {
            basePosition = Position;
            Animation.Initialize();

            state = State.FORMING;
            stopCount = 0.0f;
        }

        public override void Update()
        {
            Default();
            switch (state)
            {
                case State.BEFOREFLY:
                    BeforeFly();
                    break;
                case State.FORMING:
                    Forming();
                    break;
                case State.STOP:
                    Stop();
                    break;
            }
        }

        private void Default()
        {
            Animation.Update();
        }

        private void BeforeFly()
        {

        }

        private void Forming()
        {
            float deltaTime = TimeSpeed.Time;
            //elapsedTime += deltaTime;
            //float newX = sinWidth * (float)Math.Sin(MathHelper.ToRadians(speedX * elapsedTime));

            spriteEffects = SpriteEffects.None;
            //if (newX < sinWidth / 2)
            //    spriteEffects = SpriteEffects.None;
            //else if (newX > sinWidth / 2)
            //    spriteEffects = SpriteEffects.FlipHorizontally;

            Position = new Vector2(MathHelper.Lerp(Position.X, player.Position.X, 0.025f),
                MathHelper.Clamp(basePosition.Y + speedY * deltaTime, basePosition.Y, player.Position.Y));
        }

        private void Stop()
        {
            stopCount += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            if (stopCount >= stopTime)
            {
                stopCount = 0.0f;
                state = State.FORMING;
            }
        }

        public override void Draw(Renderer renderer)
        {
            Animation.Draw(renderer, Vector2.Zero, spriteEffects);
        }

        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.RedEnemy)
            {
                state = State.STOP;
            }
        }

        public void Destroy()
        {
            IsDead = true;
        }

        public bool IsOutOfScreenUp()
        {
            //return Position.Y + Size.Y / 2 < camera.Position.Y - Screen.HEIGHT / 2;
            // TODO : あほ
            return false;
        }
    }
}
