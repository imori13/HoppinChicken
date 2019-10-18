using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.GameObjects.Enemys.AttackModules;
using FliedChicken.GameObjects.Enemys.MoveModules;
using FliedChicken.Devices;
using FliedChicken.Devices.AnimationDevice;

namespace FliedChicken.GameObjects.Enemys
{
    class ThornEnemy : Enemy
    {
        public ThornEnemy(Camera camera) : base(camera)
        {
            Size = new Vector2(64, 64);
            TextureName = "thorn";

            GameObjectTag = GameObjectTag.RedEnemy;
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
