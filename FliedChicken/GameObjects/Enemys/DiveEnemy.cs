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

        private SpriteEffects spriteEffects;

        private Animation Animation;

        private readonly float stopTime = 2.0f;
        private float stopCount;

        public State state;

        private float minPlayerDistance;
        private float maxPlayerDistance;

        private float moveSpeed;

        public DiveEnemy(Camera camera, Player player)
        {
            this.camera = camera;
            this.player = player;

            Collider = new CircleCollider(this, 192 * 0.35f);
            Position = player.Position - Vector2.UnitY * 100f;
            GameObjectTag = GameObjectTag.DiveEnemy;

            Animation = new Animation(this, "DiveEnemy", new Vector2(216, 152), 4, 0.05f);

            maxPlayerDistance = Screen.HEIGHT * 0.75f;
            minPlayerDistance = Screen.HEIGHT * 0.25f;
        }

        public override void Initialize()
        {
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

            UpdatePlayerDistance();
            Position = new Vector2(Position.X, Math.Max(player.Position.Y - maxPlayerDistance, Position.Y));
        }

        private void BeforeFly()
        {

        }

        private void Forming()
        {
            float deltaTime = TimeSpeed.Time;

            spriteEffects = SpriteEffects.None;

            Vector2 playerPos = player.Position;

            float newX = MathHelper.Lerp(Position.X, playerPos.X, 0.2f * deltaTime);
            float newY = Math.Min(Position.Y + moveSpeed * deltaTime, playerPos.Y - minPlayerDistance);
            Position = new Vector2(newX, newY);
        }

        private void Stop()
        {
            stopCount += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

            Animation.Color = (stopCount % 0.05f <= 0.025f) ? (Color.White * 1) : (Color.White * 0.1f);

            if (stopCount >= stopTime)
            {
                Animation.Color = Color.White;
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
            if (gameObject.GameObjectTag == GameObjectTag.OneChanBom)
            {
                state = State.STOP;
                minPlayerDistance -= 20;
            }

            if (gameObject is KillerEnemy)
            {
                state = State.STOP;
                minPlayerDistance -= 20;
            }
        }

        private void UpdatePlayerDistance()
        {
            //Aランクの手前ぐらいの落下距離で最大
            float sineY = Easing2D.SineIn(player.SumDistance, 600, new Vector2(0,4), new Vector2(0, 9.5f)).Y;
            moveSpeed = sineY;

            minPlayerDistance -= moveSpeed * TimeSpeed.Time;
            minPlayerDistance = Math.Max(0, minPlayerDistance);
        }
    }
}
