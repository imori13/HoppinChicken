using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.GameObjects.Enemys.AttackModules;
using FliedChicken.GameObjects.Enemys.MoveModules;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Collision;

namespace FliedChicken.GameObjects.Enemys
{
    class ThornEnemy : Enemy
    {
        public ThornEnemy(Camera camera) : base(camera)
        {
            Size = new Vector2(1,1);
            TextureName = "Mine";

            GameObjectTag = GameObjectTag.RedEnemy;

            SpawnPosFunc = SpawnPosition.ScreenDownCenterRandomX;

            Collider = new CircleCollider(this, 36);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.OneChanBom)
            {
                IsDead = true;
                DestroyEffect(Vector2.One);
            }
        }

        public override void Draw(Renderer renderer)
        {
            //画像が無かったのでPixel描画用
            renderer.Draw2D(TextureName, Position, Color.White, 45.0f, Size);
        }

        protected override bool IsDestroy()
        {
            float down = Position.Y + Size.Y / 2;
            float upLimit = Camera.Position.Y - Screen.HEIGHT / 2;
            bool isOverDown = down < upLimit;

            return isOverDown;
        }

        protected override void OnDestroy()
        {
        }
    }
}
