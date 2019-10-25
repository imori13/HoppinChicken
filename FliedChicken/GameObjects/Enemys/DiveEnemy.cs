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

        private Vector2 basePosition;

        private SpriteEffects spriteEffects;

        private Animation Animation;

        private readonly float stopTime = 2.0f;
        private float stopCount;

        public State state;

        private const float moveSpeed = 4.0f;
        private const float accelAngle = 10.0f;

        public DiveEnemy(Camera camera, Player player)
        {
            this.camera = camera;
            this.player = player;

            Collider = new BoxCollider(this, Vector2.One * 3);
            GameObjectTag = GameObjectTag.DiveEnemy;

            Animation = new Animation(this, "DiveEnemy", new Vector2(297, 192), 5, 0.05f);
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

            spriteEffects = SpriteEffects.None;

            Vector2 playerDir = Vector2.Normalize(player.Position - Position);
            Velocity = Vector2.Lerp(Velocity, playerDir, 0.25f);

            float playerDegree = MyMath.Vec2ToDeg(playerDir);
            float currentDegree = MyMath.Vec2ToDeg(Velocity);
            float degreePercentage = Math.Max(0, 1 - Math.Abs(playerDegree -  currentDegree) / accelAngle);

            Position += Velocity * moveSpeed * degreePercentage * deltaTime;
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
