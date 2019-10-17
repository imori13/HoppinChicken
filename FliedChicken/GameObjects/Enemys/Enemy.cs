using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FliedChicken.GameObjects.Collision;
using FliedChicken.Devices;
using FliedChicken.Devices.AnimationDevice;

namespace FliedChicken.GameObjects.Enemys
{
    abstract class Enemy : GameObject
    {
        public float MoveSpeed { get; set; }
        public Vector2 Size { get; protected set; }

        public int MinSpeed { get; protected set; }
        public int MaxSpeed { get; protected set; }

        public int MinInterval { get; protected set; }
        public int MaxInterval { get; protected set; }
        public Animation Animation { get; protected set; }

        public Vector2 DrawOffset { get; protected set; }

        private Vector2 colliderSize;

        private SpriteEffects spriteEffects;

        protected Enemy(
            float width,
            float height,
            float collWidth,
            float collHeight,
            int minSpeed,
            int maxSpeed,
            int minInterval,
            int maxInterval)
        {
            Size = new Vector2(width, height);
            colliderSize = new Vector2(collWidth, collHeight);
            Collider = new BoxCollider(this, colliderSize);
            GameObjectTag = GameObjectTag.Enemy;

            MinSpeed = minSpeed;
            MaxSpeed = maxSpeed;

            MinInterval = minInterval;
            MaxInterval = maxInterval;

            DrawOffset = new Vector2(0.5f, 0.5f);
        }

        public abstract Enemy Clone();

        public override void Initialize()
        {
            Animation.Initialize();
        }

        public override void Update()
        {
            Position += new Vector2(MoveSpeed, 0) * TimeSpeed.Time;
            Animation.Update();

            spriteEffects = SpriteEffects.None;
            if (MoveSpeed > 0)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public override void Draw(Renderer renderer)
        {
            Animation.Draw(renderer, DrawOffset, spriteEffects);
        }

        public override void HitAction(GameObject gameObject)
        {
        }

        public void Destroy()
        {
            IsDead = true;
        }

        public float GetRandomizedSpeed()
        {
            Random rand = GameDevice.Instance().Random;
            return rand.Next(MinSpeed, MaxSpeed + 1) + (float)rand.NextDouble();
        }

        public float GetRandomizedInterval()
        {
            Random rand = GameDevice.Instance().Random;
            return rand.Next(MinInterval, MaxInterval + 1) + (float)rand.NextDouble();
        }
    }
}
