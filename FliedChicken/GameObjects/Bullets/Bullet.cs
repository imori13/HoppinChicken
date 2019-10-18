using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Enemys.MoveModules;

namespace FliedChicken.GameObjects.Bullets
{
    abstract class Bullet : GameObject
    {
        protected string TextureName { get; set; }
        protected MoveModule MoveModule { get; set; }
        protected SpriteEffects SpriteEffects { get; set; }

        private Vector2 previousPosition;

        public Bullet()
        {
        }

        public override void Initialize()
        {
        }

        public override void Update()
        {
            SetDrawDirection();
            previousPosition = Position;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D(TextureName, Position, Color.White, SpriteEffects);
        }

        public override void HitAction(GameObject gameObject)
        {
        }

        protected void SetDrawDirection()
        {
            if (previousPosition.X < Position.X)
                SpriteEffects = SpriteEffects.FlipHorizontally;
            else
                SpriteEffects = SpriteEffects.None;
        }
    }
}
