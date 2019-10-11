using FliedChicken.Devices;
using Microsoft.Xna.Framework;
using System;

namespace FliedChicken.GameObjects.Objects
{
    class BackGroundObject : GameObject
    {
        private string textureName;
        private Vector2 textureSize;

        public BackGroundObject(string textureName, Vector2 textureSize)
        {
            this.textureName = textureName;
            this.textureSize = textureSize;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D(textureName, Position, Color.White, 0.0f, new Vector2(Screen.WIDTH / textureSize.X, Screen.HEIGHT / textureSize.Y));
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

        public Vector2 Up()
        {
            float halfHeight = Screen.HEIGHT / 2;
            return Position - new Vector2(0, halfHeight);
        }

        public Vector2 Down()
        {
            float halfHeight = Screen.HEIGHT / 2;
            return Position + new Vector2(0, halfHeight);
        }

        public bool IsOutOfScreenUp(Camera camera)
        {
            float screenUp = camera.Position.Y - Screen.HEIGHT / 2;
            return Down().Y < screenUp;
        }
    }
}
