using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using FliedChicken.Devices;
using FliedChicken.Utilities;

namespace FliedChicken.GameObjects.Enemys
{

    enum EnemyDirection
    {
        LEFT = -1,
        RIGHT = 1,
    }

    struct LaneInfo
    {
        public string enemyName;

        public float width;
        public float height;

        public float moveSpeed;
        public float interval;

        public EnemyDirection enemyDirection;
    }

    class EnemyLane : GameObject
    {
        public LaneInfo LaneInfo { get; private set; }

        private Timer spawnTimer;
        private List<Enemy> enemyList = new List<Enemy>();

        public EnemyLane()
        {
            LaneInfo = GenerateLaneInfo();

            spawnTimer = new Timer(LaneInfo.interval);
        }

        public override void Initialize()
        {
            spawnTimer.Reset();
            PreGenerateEnemy();
        }

        public override void Update()
        {
            if (spawnTimer.IsTime())
            {
                var newEnemy = EnemyFactory.Create(LaneInfo.enemyName);
                newEnemy.Position = new Vector2(Position.X + LaneInfo.width / 2 * -(int)LaneInfo.enemyDirection, Position.Y);
                newEnemy.MoveSpeed = LaneInfo.moveSpeed;

                enemyList.Add(newEnemy);
                ObjectsManager.AddGameObject(newEnemy);

                spawnTimer.Reset();
            }
        }

        public override void Draw(Renderer renderer)
        {
#if DEBUG
            renderer.Draw2D(
                "stage",
                Position,
                new Color(1, 1, 1, 0.5f),
                0.0f,
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
            enemyList.ForEach(enemy => enemy.Destroy());
        }

        private void PreGenerateEnemy()
        {
            float basePos = LaneInfo.width / 2 * -(int)LaneInfo.enemyDirection;
            float moveValue = LaneInfo.moveSpeed * LaneInfo.interval;

            var newEnemy = EnemyFactory.Create(LaneInfo.enemyName);
            newEnemy.Position = new Vector2(basePos + moveValue, Position.Y);
            newEnemy.MoveSpeed = LaneInfo.moveSpeed;

            enemyList.Add(newEnemy);
            ObjectsManager.AddGameObject(newEnemy);
        }

        private LaneInfo GenerateLaneInfo()
        {
            var random = GameDevice.Instance().Random;

            EnemyDirection direction = EnemyDirection.LEFT;
            if (random.Next(0, 2) == 0)
                direction = EnemyDirection.LEFT;
            else
                direction = EnemyDirection.RIGHT;

            string enemyName = RandomSelector.Select(EnemyFactory.GetEnemyNameList());
            var selectEnemy = EnemyFactory.Create(enemyName);

            var laneInfo = new LaneInfo()
            {
                width = Screen.WIDTH,
                height = selectEnemy.Size.Y,

                enemyName = enemyName,
                moveSpeed = selectEnemy.GetRandomizedSpeed() * (int)direction,
                interval = selectEnemy.GetRandomizedInterval(),

                enemyDirection = direction,
            };

            return laneInfo;
        }
    }
}
