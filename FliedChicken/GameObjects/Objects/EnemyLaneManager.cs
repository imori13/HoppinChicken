using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Objects
{
    class EnemyLaneManager : GameObject
    {
        private Camera camera;
        private Queue<EnemyLane> laneQueue;
        private int laneCount;

        public EnemyLaneManager(Camera camera, int laneCount)
        {
            this.camera = camera;
            this.laneCount = Math.Max(1, laneCount);

            laneQueue = new Queue<EnemyLane>();
        }

        public override void Initialize()
        {
            for (int i = 0; i < laneCount; i++)
            {
                var newLane = new EnemyLane();
                //レーン情報の読み込みとか実装したら高さに合わせるようにする
                newLane.Position = Position + new Vector2(0, 128 * i);

                ObjectsManager.AddGameObject(newLane);
                laneQueue.Enqueue(newLane);
            }
        }

        public override void Update()
        {
            var hoge = laneQueue.Peek();
            if (laneQueue.Peek().IsOutOfScreenUp(camera))
            {
                laneQueue.Dequeue().Destory();

                var newLane = new EnemyLane();
                //レーン情報の読み込みとか実装したら高さに合わせるようにする
                newLane.Position = new Vector2(0, GetLowestYPosition());

                ObjectsManager.AddGameObject(newLane);
                laneQueue.Enqueue(newLane);
            }
        }

        public override void Draw(Renderer renderer)
        {
        }

        public override void HitAction(GameObject gameObject)
        {
        }

        private float GetLowestYPosition()
        {
            return laneQueue.Max<EnemyLane, float>(lane => lane.Position.Y + lane.LaneInfo.height);
        }
    }
}
