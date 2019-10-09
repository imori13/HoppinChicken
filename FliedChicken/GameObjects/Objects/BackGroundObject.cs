using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Objects
{
    class BackGroundObject : GameObject
    {
        public Vector2 Size { get; private set; }

        public BackGroundObject(Vector2 size)
        {
            Size = size;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D("stage", Position, Color.White, 0.0f, Size / 2, Vector2.One);
        }

        public override void HitAction(GameObject gameObject)
        {
        }

        public override void Initialize()
        {
        }

        public override void Update()
        {
        }

        public Vector2 Top()
        {
            float halfHeight = Size.Y / 2;
            return Position - new Vector2(0, halfHeight);
        }

        public Vector2 Down()
        {
            float halfHeight = Size.Y / 2;
            return Position + new Vector2(0, halfHeight);
        }
    }
}
