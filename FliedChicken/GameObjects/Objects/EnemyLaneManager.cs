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
        private int preGenerateCount;

        private CoinManager coinManager;

        public EnemyLaneManager(Camera camera, int laneCount, int preGenerateCount, CoinManager coinManager = null)
        {
            this.camera = camera;
            this.laneCount = laneCount - preGenerateCount;
            this.preGenerateCount = preGenerateCount;

            laneQueue = new Queue<EnemyLane>();
            this.coinManager = coinManager;
        }

        public override void Initialize()
        {
            float basePosition = Position.Y;
            for (int i = 0; i < preGenerateCount; i++)
            {
                var newLane = new EnemyLane();
                newLane.Position = new Vector2(Position.X, basePosition + newLane.LaneInfo.height / 2);

                newLane.ObjectsManager = ObjectsManager;

                ObjectsManager.AddGameObject(newLane);
                laneQueue.Enqueue(newLane);

                basePosition = newLane.LaneInfo.height / 2 + newLane.Position.Y;
                coinManager.GenerateCoin(newLane);
            }
        }

        public override void Update()
        {
            DestroyOutOfScreen();

            if (laneCount == 0) return;

            var lastLane = laneQueue.Last();
            if (lastLane.Position.Y < camera.Position.Y + Screen.HEIGHT)
            {
                float basePosition = lastLane.Position.Y + lastLane.LaneInfo.height / 2;

                var newLane = new EnemyLane();
                newLane.Position = new Vector2(Position.X, basePosition + newLane.LaneInfo.height / 2);
                newLane.ObjectsManager = ObjectsManager;

                ObjectsManager.AddGameObject(newLane);
                laneQueue.Enqueue(newLane);
                laneCount--;

                coinManager.GenerateCoin(newLane);
            }
        }

        public override void Draw(Renderer renderer)
        {
        }

        public override void HitAction(GameObject gameObject)
        {
        }

        private void DestroyOutOfScreen()
        {
            if (laneQueue.Count == 0) return;

            if (laneQueue.Peek().IsOutOfScreenUp(camera))
                laneQueue.Dequeue().Destory();
        }

        public void Destroy()
        {
            IsDead = true;
            foreach (var lane in laneQueue)
            {
                lane.Destory();
            }
        }
    }
}
