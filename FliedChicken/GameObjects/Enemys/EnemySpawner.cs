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
        }

        public void Update()
        {
            if (spawnTimer.IsTime())
            {
                //仮置きで標準のやつだけ
                var enemy = new NormalEnemy(camera);

                float posX = GetPosX(enemy) + camera.Position.X;
                float posY = GetPosY() + camera.Position.Y;

                enemy.Position = new Vector2(posX, posY);
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

        private float GetPosX(Enemy enemy)
        {
            float basePosition = 0.0f;

            switch (enemy.spawnPosType)
            {
                case SpawnPositionType.Left:
                    basePosition = -Screen.WIDTH / 2 + enemy.Size.X / 2;
                    break;
                case SpawnPositionType.Right:
                    basePosition = Screen.WIDTH / 2 - enemy.Size.X / 2;
                    break;
            }

            return basePosition;
        }

        private float GetPosY()
        {
            var random = GameDevice.Instance().Random;
            return random.Next(spawnRange_Min, spawnRange_Max + 1) + (float)random.NextDouble();
        }
    }
}
