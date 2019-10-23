using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Collision;
using FliedChicken.GameObjects.Enemys.MoveModules;

namespace FliedChicken.GameObjects.Bullets
{
    class NormalBullet : Bullet
    {
        float rotation;
        float rotationSpeed;
        Random rand;
        Camera Camera;

        public NormalBullet(Camera Camera,Vector2 moveDirection, float moveSpeed)
        {
            TextureName = "Pixel";
            MoveModule = new Simple_MM(this, moveDirection, moveSpeed);
            Collider = new BoxCollider(this, new Vector2(20, 20));
            rand = GameDevice.Instance().Random;

            rotation = rand.Next(0, 360) + (float)rand.NextDouble();
            rotationSpeed = (rand.Next(2) == 0) ? (rand.Next(-30, -20) + (float)rand.NextDouble()) : (rand.Next(20, 30) + (float)rand.NextDouble());

            this.Camera = Camera;
        }

        public override void Update()
        {
            base.Update();
            MoveModule.Move();

            float rightLimit = Camera.Position.X + Screen.WIDTH / 2f + 500f;
            float leftLimit = Camera.Position.X - Screen.WIDTH / 2f - 500f;
            float downLimit = Camera.Position.Y + Screen.HEIGHT / 2f + 500f;
            float upLimit = Camera.Position.Y - Screen.HEIGHT / 2f - 500f;

            IsDead = ((Position.X > rightLimit) || (Position.X < leftLimit) || (Position.Y > downLimit) || (Position.Y < upLimit));

            rotation += rotationSpeed * TimeSpeed.Time;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D(TextureName, Position, Color.Red, MathHelper.ToRadians(rotation), new Vector2(20, 20), SpriteEffects);
        }
    }
}
