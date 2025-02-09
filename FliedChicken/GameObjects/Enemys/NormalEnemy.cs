﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.GameObjects.Objects;
using FliedChicken.GameObjects.PlayerDevices;
using FliedChicken.GameObjects.Collision;
using FliedChicken.GameObjects.Enemys.AttackModules;
using FliedChicken.GameObjects.Enemys.MoveModules;
using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Enemys
{
    class NormalEnemy : Enemy, IOneChanItemCarrier
    {
        private float moveDirection;

        public OneChanItem OneChanItem { get; set; }

        public NormalEnemy(Camera camera)
            : base(camera)
        {
            Size = new Vector2(100, 60);

            //Animation = new Animation(this, "normal_enemy", new Vector2(210, 210), 5, 0.25f);
            //Animation.drawSize = Vector2.One * 0.5f;

            Animation = new Animation(this, "NormalEnemy", new Vector2(144, 104), 5, 0.1f);
            Animation.drawSize = Vector2.One;

            GameObjectTag = GameObjectTag.OrangeEnemy;

            var random = GameDevice.Instance().Random;
            moveDirection = random.Next(0, 2) == 0 ? -1 : 1;
            float speed = random.Next(5, 8);
            MoveModule = new Simple_MM(this, new Vector2(moveDirection, 0), speed);

            if (moveDirection < 0)
                SpawnPosFunc = SpawnPosition.SreenDownRandomXRight;
            else
                SpawnPosFunc = SpawnPosition.SreenDownRandomXLeft;
        }

        public override void Initialize()
        {
            base.Initialize();

            Collider = new BoxCollider(this, Size);

            MoveModule.Initialize();
        }

        public override void Update()
        {
            base.Update();

            MoveModule.Move();
        }

        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.OneChanBom)
            {
                IsDead = true;
                DestroyEffect(Vector2.One);
            }

            if (gameObject.GameObjectTag == GameObjectTag.Player)
            {
                if (OneChanItem != null)
                    OneChanItem.PlayerHitEnabled = true;
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
            if (OneChanItem != null)
                OneChanItem.Destroy();
        }

        public Vector2 GetItemPosition()
        {
            return Position + new Vector2(0, -Size.Y / 4);
        }
    }
}
