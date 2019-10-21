using FliedChicken.Devices;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Clouds
{
    // 背景描画の雲の抽象クラス
    // 描画順が独立するためGameObjectやObjectsManagerで管理しない
    abstract class Cloud
    {
        public ObjectsManager ObjectsManager { get; set; }
        public CloudManager CloudManager { get; set; }
        public Vector2 Position { get; protected set; }
        public Vector2 Velocity { get; protected set; }
        public bool IsDead { get; protected set; }
        public float Layer { get; protected set; }

        public string AssetName { get; protected set; } = "Pixel";
        public Vector2 Size { get; protected set; } = new Vector2(250, 50);
        public Color Color { get; protected set; } = Color.White;

        public Random Random { get; protected set; } = GameDevice.Instance().Random;

        public Cloud()
        {

        }

        public virtual void Initialize()
        {
        }

        public virtual void Update()
        {
            Position += Velocity * TimeSpeed.Time;

            float left = ObjectsManager.Camera.Position.X - Screen.WIDTH / 2f - 1000f;
            float right = ObjectsManager.Camera.Position.X + Screen.WIDTH / 2f + 1000f;
            float up = ObjectsManager.Camera.Position.Y - Screen.HEIGHT / 2f - 1000f;
            float down = ObjectsManager.Camera.Position.Y + Screen.HEIGHT / 2f + 1000f;

            IsDead = ((Position.X < left) || (Position.X > right) || (Position.Y < up) || (Position.Y > down)) ? (true) : (false);
        }

        public virtual void Draw(Renderer renderer)
        {
            renderer.Draw2D(AssetName, Position, Color, 0, Vector2.One * 0.5f, Size, Layer);
        }
    }
}
