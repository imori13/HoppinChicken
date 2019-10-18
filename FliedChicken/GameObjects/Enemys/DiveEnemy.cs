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
    class DiveEnemy : GameObject
    {
        private Camera camera;

        private float sinWidth;
        private float elapsedTime;

        private float speedX;
        private float speedY;
        private Vector2 basePosition;

        private SpriteEffects spriteEffects;

        private Animation Animation;

        public DiveEnemy(Camera camera)
        {
            this.camera = camera;
            
            Collider = new BoxCollider(this, Vector2.One*3);
            GameObjectTag = GameObjectTag.OrangeEnemy;

            speedX = 2;
            speedY = 7;
            sinWidth = 64 * 8;
            elapsedTime = 0.0f;

            Animation = new Animation("DescentEnemy", new Vector2(490, 320), 5, 0.1f);
            Animation.Size = Vector2.One;
            Animation.GameObject = this;
        }

        public override void Initialize()
        {
            basePosition = Position;
            Animation.Initialize();
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

            Animation.Update();
        }

        public override void Draw(Renderer renderer)
        {
            Animation.Draw(renderer, Vector2.Zero, spriteEffects);
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
            //return Position.Y + Size.Y / 2 < camera.Position.Y - Screen.HEIGHT / 2;
            // TODO : あほ
            return false;
        }
    }
}
