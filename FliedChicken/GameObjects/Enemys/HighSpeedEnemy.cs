using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.GameObjects.Collision;
using FliedChicken.GameObjects.Particle;
using FliedChicken.GameObjects.Enemys.MoveModules;
using FliedChicken.Devices;
using FliedChicken.Utilities;

namespace FliedChicken.GameObjects.Enemys
{
    class HighSpeedEnemy : Enemy
    {
        private float moveDirection;

        private Vector2 particlePosition;
        private Timer particleTimer;

        public HighSpeedEnemy(Camera camera) : base(camera)
        {
            Size = new Vector2(150, 50);
            Collider = new BoxCollider(this, Size);

            //Animation = new Animation(this, "highspeed_enemy", new Vector2(240, 96), 6, 0.25f);
            //Animation.drawSize = Vector2.One;
            TextureName = "HighSpeed";

            GameObjectTag = GameObjectTag.RedEnemy;

            var random = GameDevice.Instance().Random;
            moveDirection = random.Next(0, 2) == 0 ? -1 : 1;
            float speed = random.Next(5, 8) * 2;
            MoveModule = new Simple_MM(this, new Vector2(moveDirection, 0), speed);

            if (moveDirection < 0)
                SpawnPosFunc = SpawnPosition.SreenDownRandomXRight;
            else
                SpawnPosFunc = SpawnPosition.SreenDownRandomXLeft;
        }
        public override void Initialize()
        {
            base.Initialize();

            particlePosition = new Vector2(Size.X * 0.55f * -moveDirection, Size.Y * 0.17f);
            particleTimer = new Timer(0.1f);

            MoveModule.Initialize();
        }

        public override void Update()
        {
            base.Update();

            MoveModule.Move();

            if (particleTimer.IsTime())
            {
                ObjectsManager.AddBackParticle(new LingerRotateParticle2D(Position + particlePosition, GameDevice.Instance().Random));
                particleTimer.MaxTime = GameDevice.Instance().Random.Next(3, 5 + 1) * 0.01f;
                particleTimer.Reset();
            }
        }

        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.OneChanBom)
            {
                IsDead = true;
                DestroyEffect(Vector2.One);
            }
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
