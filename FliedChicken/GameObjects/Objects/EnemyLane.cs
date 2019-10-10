using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Objects
{
    struct LaneInfo
    {
        public float width;
        public float height;


    }

    class EnemyLane : GameObject
    {
        public LaneInfo LaneInfo { get; private set; }

        public EnemyLane()
        {
        }

        public override void Initialize()
        {
            LaneInfo = new LaneInfo()
            {
                width = 128 * 10,
                height = 128,
            };
        }

        public override void Update()
        {
        }

        public override void Draw(Renderer renderer)
        {
#if DEBUG
            renderer.Draw2D(
                "stage",
                Position,
                new Color(1, 1, 1, 0.5f),
                0.0f, new Vector2(704, 832) / 2,
                new Vector2(LaneInfo.width / 704, LaneInfo.height / 832)
                );
#endif
        }

        public override void HitAction(GameObject gameObject)
        {
        }

        public bool IsOutOfScreenUp(Camera camera)
        {
            float screenUp = camera.Position.Y - Screen.HEIGHT / 2;
            float down = Position.Y + LaneInfo.height / 2;
            return down < screenUp;
        }

        public void Destory()
        {
            IsDead = true;
        }

        private LaneInfo RandomizeLaneInfo()
        {
            var laneInfo = new LaneInfo();

            

            return laneInfo;
        }
    }
}
