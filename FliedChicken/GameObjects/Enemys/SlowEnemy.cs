using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Enemys.AttackModules;
using FliedChicken.GameObjects.Enemys.MoveModules;
using FliedChicken.GameObjects.Collision;
using FliedChicken.GameObjects.PlayerDevices;

namespace FliedChicken.GameObjects.Enemys
{
    class SlowEnemy : Enemy
    {
        private float moveDirection;

        public SlowEnemy(Camera camera) : base(camera)
        {
            Size = new Vector2(650, 275);
            Collider = new BoxCollider(this, Size);

            //Animation = new Animation(this, "slowenemy", new Vector2(300, 105), 8, 0.1f);
            //Animation.drawSize = Vector2.One * 2.0f;

            TextureName = "SlowEnemy";

            GameObjectTag = GameObjectTag.OrangeEnemy;

            var random = GameDevice.Instance().Random;
            moveDirection = random.Next(0, 2) == 0 ? -1 : 1;
            float speed = random.Next(5, 8) / 2;
            MoveModule = new Simple_MM(this, new Vector2(moveDirection, 0), speed);

            SpawnPosFunc = SpawnPosition.ScreenDownCenterRandomX;
        }

        public override void Initialize()
        {
            base.Initialize();

            AttackModule = new NotAttack_AM(this);

            MoveModule.Initialize();
            AttackModule.Initialize();
        }

        public override void Update()
        {
            base.Update();

            MoveModule.Move();
            AttackModule.Attack();
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D(TextureName, Position, Color.White, 0, Vector2.One * 2.5f, SpriteEffect);
        }

        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.OneChanBom)
            {
                IsDead = true;
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
