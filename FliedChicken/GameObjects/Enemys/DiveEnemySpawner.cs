using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices;
using FliedChicken.Utilities;
using FliedChicken.GameObjects.Objects;
using FliedChicken.GameObjects.PlayerDevices;

namespace FliedChicken.GameObjects.Enemys
{
    class DiveEnemySpawner
    {
        private bool spawnFlag;
        private Timer spawnInterval;

        private Player player;
        private Camera camera;
        private DiveEnemy instance;
        private ObjectsManager objectsManager;

        public DiveEnemySpawner(float minInterval, ObjectsManager objManager, Player player, Camera camera)
        {
            spawnInterval = new Timer(minInterval);
            objectsManager = objManager;

            this.player = player;
            this.camera = camera;

            spawnFlag = false;
        }

        public void Initialize()
        {
            instance = null;
        }

        public void Update()
        {
            bool outOfScreenUp = true;
            if (instance != null) outOfScreenUp = instance.IsOutOfScreenUp();

            RollSpawnChance();

            if (outOfScreenUp && spawnFlag && spawnInterval.IsTime())
            {
                SpawnNew();
                spawnInterval.Reset();
            }
        }

        public void Shutdown()
        {
            if (instance != null)
                instance.Destroy();
            instance = null;
            spawnFlag = false;
            spawnInterval.Reset();
        }

        private void SpawnNew()
        {
            spawnFlag = false;

            if (instance != null)
                instance.Destroy();

            var newObject = new DiveEnemy(camera);
            newObject.Position = new Vector2(player.Position.X, player.Position.Y - Screen.HEIGHT / 2 + 256);
            objectsManager.AddGameObject(newObject);

            instance = newObject;
        }

        private void RollSpawnChance()
        {
            if (spawnFlag) return;

            var random = GameDevice.Instance().Random;
            if (random.NextDouble() < 0.05)
            {
                spawnFlag = true;
            }
        }
    }
}
