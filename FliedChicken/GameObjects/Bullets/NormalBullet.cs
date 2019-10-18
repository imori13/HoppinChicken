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
        public NormalBullet(Vector2 moveDirection, float moveSpeed)
        {
            TextureName = "Pixel";
            MoveModule = new Simple_MM(this, moveDirection, moveSpeed);
            Collider = new BoxCollider(this, new Vector2(128, 128));
        }

        public override void Update()
        {
            base.Update();
            MoveModule.Move();
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D(TextureName, Position, Color.White, 0.0f, new Vector2(128, 128), SpriteEffects);
        }
    }
}
