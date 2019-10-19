using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.GameObjects.Collision;
using FliedChicken.GameObjects.Enemys.MoveModules;
using FliedChicken.Devices;
using FliedChicken.Devices.AnimationDevice;
using FliedChicken.Utilities;

namespace FliedChicken.GameObjects.Enemys
{
    class HighSpeedEnemy : Enemy
    {
        private float moveDirection;

        private Timer particleTimer;

        public HighSpeedEnemy(Camera camera) : base(camera)
        {
            Size = new Vector2(400, 140);
            Collider = new BoxCollider(this, Size);

            Animation = new Animation(this, "highspeed_enemy", new Vector2(400, 140), 6, 0.25f);
            Animation.drawSize = Vector2.One;

            GameObjectTag = GameObjectTag.RedEnemy;

            var random = GameDevice.Instance().Random;
            moveDirection = random.Next(0, 2) == 0 ? -1 : 1;
            float speed = random.Next(5, 8) * 2;
            MoveModule = new Simple_MM(this, new Vector2(moveDirection, 0), speed);

            if (moveDirection < 0)
                SpawnPosFunc = SpawnPosition.ScreenRight;
            else
                SpawnPosFunc = SpawnPosition.ScreenLeft;
        }
        public override void Initialize()
        {
            base.Initialize();

            particleTimer = new Timer(0.2f);

            MoveModule.Initialize();
        }

        public override void Update()
        {
            base.Update();

            MoveModule.Move();

            if (particleTimer.IsTime())
            {
                var particlePos = Position + new Vector2(Size.X * 0.25f * -moveDirection, Size.Y * 0.6f);

                ObjectsManager.AddParticle(new Particle.LingerRotateParticle2D(particlePos, GameDevice.Instance().Random));
                particleTimer.MaxTime = GameDevice.Instance().Random.Next(3, 5 + 1) * 0.01f;
                particleTimer.Reset();
            }
        }

        public override void HitAction(GameObject gameObject)
        {
        }

        protected override bool IsDestroy()
        {
            float side = Position.X + Size.X / 2 * -moveDirection;
            float sideLimit = Camera.Position.X + Screen.WIDTH / 2 * moveDirection;
            bool isOverSide = moveDirection > 0 ? side > sideLimit : side < sideLimit;

            float down = Position.Y + Size.Y / 2;
            float upLimit = Camera.Position.Y - Screen.HEIGHT / 2;
            bool isOverDown = down < upLimit;

            return isOverSide || isOverDown;
        }

        protected override void OnDestroy()
        {

        }
    }
}
