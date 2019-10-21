using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using Microsoft.Xna.Framework;

namespace FliedChicken.GameObjects.Clouds
{
    // 手前に描画する雲
    class FrontCloud : Cloud
    {
        Vector2 prevCameraPos;
        float speed;
        Vector2 initSize;

        public FrontCloud(Vector2 Position)
        {
            this.Position = Position;
        }

        public override void Initialize()
        {
            base.Initialize();

            Random rand = GameDevice.Instance().Random;
            Size = new Vector2(rand.Next(250, 350), rand.Next(100, 200));
            Color = Color.Lerp(Color.Gray, new Color(200, 200, 200), (float)rand.NextDouble());
            speed = Random.Next(-7000, 7000) / 10000f;

            Layer = (speed + 0.7f) / (speed - 0.7f);

            initSize = Size;
            Size = Vector2.Zero;
        }

        public override void Update()
        {
            base.Update();

            Size = Vector2.Lerp(Size, initSize, 0.1f);

            Vector2 direction = ObjectsManager.Camera.Position - prevCameraPos;

            Position += direction * speed;

            prevCameraPos = ObjectsManager.Camera.Position;
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
        }
    }
}
