using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.GameObjects.Collision;
using FliedChicken.GameObjects.Enemys.AttackModules;
using FliedChicken.GameObjects.Enemys.MoveModules;
using FliedChicken.Devices;
using FliedChicken.Devices.AnimationDevice;

namespace FliedChicken.GameObjects.Enemys
{
    class NormalEnemy : Enemy
    {
        private float moveDirection;

        public NormalEnemy(Camera camera)
            : base(camera)
        {
            Size = new Vector2(320, 320) * 0.5f;
            
            Animation = new Animation(this, "normal_enemy", new Vector2(320, 320), 5, 0.25f);
            Animation.drawSize = Vector2.One * 0.5f;

            GameObjectTag = GameObjectTag.OrangeEnemy;

            var random = GameDevice.Instance().Random;
            moveDirection = random.Next(0, 2) == 0 ? -1 : 1;
            float speed = random.Next(5, 8);
            MoveModule = new Simple_MM(this, new Vector2(moveDirection, 0), speed);
            spawnPosType = moveDirection < 0 ? SpawnPositionType.Right : SpawnPositionType.Left;
        }

        public override void Initialize()
        {
            base.Initialize();

            Collider = new BoxCollider(this, Size);

            MoveModule.Initialize();
            AttackModule = new SimpleShoot_AM(this, ObjectsManager, new Vector2(64 * moveDirection, 0), new Vector2(moveDirection, 0));

            AttackModule.Initialize();
        }

        public override void Update()
        {
            base.Update();

            MoveModule.Move();
            AttackModule.Attack();
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
