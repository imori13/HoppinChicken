using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FliedChicken.GameObjects.Collision;
using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Objects
{
    class DiveEnemy : GameObject
    {
        private Camera camera;

        private float sinWidth;
        private float elapsedTime;

        private float speedX;
        private float speedY;
        private Vector2 basePosition;

        private SpriteEffects spriteEffects;

        private Vector2 Size { get; set; }

        public DiveEnemy(Camera camera)
        {
            this.camera = camera;

            Size = new Vector2(64, 64);
            Collider = new BoxCollider(this, Size);
            GameObjectTag = GameObjectTag.Enemy;

            speedX = 2;
            speedY = 7;
            sinWidth = 64 * 8;
            elapsedTime = 0.0f;
        }

        public override void Initialize()
        {
            basePosition = Position;
        }

        public override void Update()
        {
            float deltaTime = TimeSpeed.Time;
            elapsedTime += deltaTime;
            float newX = sinWidth * (float)Math.Sin(MathHelper.ToRadians(speedX * elapsedTime));

            spriteEffects = SpriteEffects.None;
            if (newX < sinWidth / 2)
                spriteEffects = SpriteEffects.None;
            else if (newX > sinWidth / 2)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Position = basePosition + new Vector2(newX, speedY * elapsedTime);
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D("packman", Position, Color.Brown, 0.0f, Vector2.One, spriteEffects);
        }

        public override void HitAction(GameObject gameObject)
        {
        }

        public void Destroy()
        {
            IsDead = true;
        }

        public bool IsOutOfScreenUp()
        {
            return Position.Y + Size.Y / 2 < camera.Position.Y - Screen.HEIGHT / 2;
        }
    }
}
