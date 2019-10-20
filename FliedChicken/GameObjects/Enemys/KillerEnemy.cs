using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using FliedChicken.Devices.AnimationDevice;
using FliedChicken.GameObjects.Enemys.AttackModules;
using FliedChicken.GameObjects.Enemys.MoveModules;
using FliedChicken.GameObjects.PlayerDevices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FliedChicken.GameObjects.Enemys
{
    class KillerEnemy : Enemy
    {
        public KillerEnemy(Camera camera) : base(camera)
        {
            AttackModule = new NotAttack_AM(this);
            MoveModule = new Killer_MM(this);

            Animation = new Animation(this, "normal_enemy", new Vector2(320, 320), 5, 0.05f);
            Animation.Color = Color.Red;
            Animation.drawSize = Vector2.One * 0.5f * 0.5f;

            GameObjectTag = GameObjectTag.OrangeEnemy;

            SpawnPosFunc = SpawnPosition.ScreenDownRandomX;
        }

        public override void Initialize()
        {
            AttackModule.Initialize();
            MoveModule.Initialize();

            SpriteEffect = SpriteEffects.FlipHorizontally;
        }

        public override void Update()
        {
            AttackModule.Attack();
            MoveModule.Move();
            Animation.Update();
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
        }

        public override void HitAction(GameObject gameObject)
        {

        }

        protected override bool IsDestroy()
        {
            return false;
        }

        protected override void OnDestroy()
        {

        }
    }
}
