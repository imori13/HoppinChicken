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
            float basePosition = Position.Y;
            for (int i = 0; i < laneCount; i++)
            {
                var newLane = new EnemyLane();
                newLane.Position = new Vector2(Position.X, basePosition + newLane.LaneInfo.height / 2);

                newLane.ObjectsManager = ObjectsManager;

                ObjectsManager.AddGameObject(newLane);
                laneQueue.Enqueue(newLane);

                basePosition = newLane.LaneInfo.height / 2 + newLane.Position.Y;
            }
        }

        public override void Update()
        {
            if (laneQueue.Peek().IsOutOfScreenUp(camera))
            {
                laneQueue.Dequeue().Destory();
            }

            var lastLane = laneQueue.Last();
            if (lastLane.Position.Y < camera.Position.Y + Screen.HEIGHT)
            {
                float basePosition = lastLane.Position.Y + lastLane.LaneInfo.height / 2;

                var newLane = new EnemyLane();
                newLane.Position = new Vector2(Position.X, basePosition + newLane.LaneInfo.height / 2);
                newLane.ObjectsManager = ObjectsManager;

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
    }
}
