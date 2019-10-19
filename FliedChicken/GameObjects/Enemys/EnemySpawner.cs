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

        private int spawnInterval_Min;
        private int spawnInterval_Max;

        private Timer spawnTimer;

        private WeightSelectHelper<Func<Enemy>>[] spawnFunctions;

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
            int spawnInterval_Min,
            int spawnInterval_Max)
        {
            this.player = player;
            this.camera = camera;
            this.objectsManager = objectsManager;
            this.spawnRange_Max = spawnRange_Max;

            this.spawnRange_Min = spawnRange_Min;
            this.spawnRange_Max = spawnRange_Max;

            this.spawnInterval_Min = spawnInterval_Min;
            this.spawnInterval_Max = spawnInterval_Max;
        }

        public void Initialize()
        {
            RandomizeTimer();

            spawnFunctions = new[]
                {
                    new WeightSelectHelper<Func<Enemy>>(5, new Func<Enemy>(() => new NormalEnemy(camera))),
                    new WeightSelectHelper<Func<Enemy>>(3, new Func<Enemy>(() => new HighSpeedEnemy(camera))),
                    new WeightSelectHelper<Func<Enemy>>(1, new Func<Enemy>(() => new SlowEnemy(camera))),
                    new WeightSelectHelper<Func<Enemy>>(2, new Func<Enemy>(() => new ThornEnemy(camera)))
                };
        }

        public void Update()
        {
            if (spawnTimer.IsTime())
            {
                var enemy = RandomSelector.WeightSelect(spawnFunctions).Invoke();

                Vector2 randomPos = new Vector2(0, GetPosY());
                if (enemy is ThornEnemy)    //仮置きでクラスごとに判定
                    randomPos = Vector2.Zero;

                enemy.Position = enemy.SpawnPosFunc.Invoke(enemy.Size) + camera.Position + randomPos;
                enemy.ObjectsManager = objectsManager;
                objectsManager.AddGameObject(enemy);

                RandomizeTimer();
            }
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

        private void RandomizeTimer()
        {
            var random = GameDevice.Instance().Random;
            spawnTimer = new Timer(random.Next(spawnInterval_Min, spawnInterval_Max + 1) + (float)random.NextDouble());
        }

        private float GetPosY()
        {
            var random = GameDevice.Instance().Random;
            return random.Next(spawnRange_Min, spawnRange_Max + 1) + (float)random.NextDouble();
        }
    }
}
