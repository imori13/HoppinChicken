﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Collision;
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

            Animation = new Animation(this, "Killer_Passive", new Vector2(60, 60), 1, 100000f);
            Collider = new CircleCollider(this, 30);

            GameObjectTag = GameObjectTag.RedEnemy;

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

            base.Update();
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
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
            float rightLimit = Camera.Position.X + Screen.WIDTH / 2f + 500f;
            float leftLimit = Camera.Position.X - Screen.WIDTH / 2f - 500f;
            float downLimit = Camera.Position.Y + Screen.HEIGHT / 2f + 500f;

            // ダイブエネミーにもキラーが当たってほしいので、ダイブエネミーにもあたるように分岐
            float up01 = Camera.Position.Y - Screen.WIDTH / 2f - 500f;  // カメラの上
            float up02 = ObjectsManager.DiveEnemy.Position.Y - 1000f;// ダイブエネミーの上
            float upLimit = (up01 < up02) ? (up01) : (up02); // より小さい方を選択

            //return ((Position.X > rightLimit) || (Position.X < leftLimit) || (Position.Y > downLimit) || (Position.Y < upLimit));
            return ((Position.X > rightLimit) || (Position.X < leftLimit) || (Position.Y < upLimit));
        }

        protected override void OnDestroy()
        {

        }
    }
}
