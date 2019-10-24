using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
using FliedChicken.GameObjects.PlayerDevices;
using FliedChicken.Utilities;

namespace FliedChicken.GameObjects.Enemys
{
    class EnemySpawner
    {
        private Player player;
        private Camera camera;
        private ObjectsManager objectsManager;

        private int spawnRange_Min;
        private int spawnRange_Max;

        private int spawnDistance_Min;
        private int spawnDistance_Max;

        private Random random;

        private WeightSelectHelper<Func<Enemy>>[] spawnFunctions;

        private float distanceSum;
        private Vector2 prevCameraPos;

        /// <summary>
        /// 難易度
        /// </summary>
        public int Difficulty { get; set; }

        public EnemySpawner(
            Player player,
            Camera camera,
            ObjectsManager objectsManager,
            int spawnRange_Min,
            int spawnRange_Max,
            int spawnDistance_Min,
            int spawnDistance_Max)
        {
            this.player = player;
            this.camera = camera;
            this.objectsManager = objectsManager;
            this.spawnRange_Max = spawnRange_Max;

            this.spawnRange_Min = spawnRange_Min;
            this.spawnRange_Max = spawnRange_Max;

            this.spawnDistance_Min = spawnDistance_Min;
            this.spawnDistance_Max = spawnDistance_Max;
        }

        public void Initialize()
        {
            random = GameDevice.Instance().Random;
            prevCameraPos = camera.Position;

            spawnFunctions = new[]
                {
                    new WeightSelectHelper<Func<Enemy>>(5, new Func<Enemy>(() => new NormalEnemy(camera))),
                    new WeightSelectHelper<Func<Enemy>>(3, new Func<Enemy>(() => new HighSpeedEnemy(camera))),
                    new WeightSelectHelper<Func<Enemy>>(1, new Func<Enemy>(() => new SlowEnemy(camera))),
                    new WeightSelectHelper<Func<Enemy>>(5, new Func<Enemy>(() => new ThornEnemy(camera))),
                    new WeightSelectHelper<Func<Enemy>>(2, new Func<Enemy>(() => new KillerEnemy(camera)))
                };
        }

        public void Update()
        {
            distanceSum += Vector2.Distance(camera.Position, prevCameraPos);
            while (distanceSum >= 0)
            {
                distanceSum -= 128f;
                SpawnEnemy();
            }
            prevCameraPos = camera.Position;
        }

        public void DebugDraw(Renderer renderer)
        {
            renderer.Draw2D(
                "Pixel",
                camera.Position + new Vector2(0, spawnRange_Min + (spawnRange_Max - spawnRange_Min) / 2),
                Color.Red,
                0.0f,
                new Vector2(32, spawnRange_Max - spawnRange_Min));
        }

        private float GetPosY()
        {
            return random.Next(spawnRange_Min, spawnRange_Max + 1) + (float)random.NextDouble();
        }

        private void SpawnEnemy()
        {
            var enemy = RandomSelector.WeightSelect(spawnFunctions).Invoke();
            SpawnWithOneChanItem(enemy);

            Vector2 randomPos = new Vector2(0, GetPosY());
            //if (enemy is ThornEnemy)    //仮置きでクラスごとに判定
            //    randomPos = Vector2.Zero;

            enemy.Position = enemy.SpawnPosFunc.Invoke(enemy.Size) + camera.Position + randomPos;
            enemy.ObjectsManager = objectsManager;
            objectsManager.AddGameObject(enemy);
        }

        private void SpawnWithOneChanItem(Enemy enemy)
        {
            if (!(enemy is IOneChanItemCarrier)) return;
            //5%の確率で出現
            if (!(random.Next(0, 100) < 20)) return;

            var carrier = enemy as IOneChanItemCarrier;
            carrier.OneChanItem = new OneChanItem(carrier);
            objectsManager.AddGameObject(carrier.OneChanItem);
        }
    }
}
