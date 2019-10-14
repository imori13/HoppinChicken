using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.GameObjects.Collision;
using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Objects
{
    class DiveEnemy : GameObject
    {
        private static DiveEnemy instance;
        private static bool spawnFlag;

        private float sinWidth;
        private float elapsedTime;

        private float speedX;
        private float speedY;

        private Player player;
        private Camera camera;
        private Vector2 basePosition;

        private Vector2 Size { get; set; }

        public DiveEnemy(Player player, Camera camera, Vector2 size, float speedX, float speedY)
        {
            this.player = player;
            this.camera = camera;

            Size = size;
            Collider = new BoxCollider(this, Size);

            this.speedX = speedX;
            this.speedY = speedY;
            sinWidth = 64 * 8;
            elapsedTime = 0.0f;

            if (instance == null)
                instance = this;
        }

        public override void Initialize()
        {
            basePosition = Position;
        }

        public override void Update()
        {
            float deltaTime = TimeSpeed.Time;
            elapsedTime += deltaTime;
            float newX = sinWidth * (float)Math.Sin(MathHelper.ToRadians(speedX * elapsedTime));

            Position = basePosition + new Vector2(newX, speedY * elapsedTime);

            RollSpawnChance();

            if (IsOutOfScreenUp() && spawnFlag)
            {
                SpawnNew();
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D("Pixel", Position, Color.Brown, 0.0f, Size);
        }

        public override void HitAction(GameObject gameObject)
        {
        }

        private static void SpawnNew()
        {
            spawnFlag = false;

            if (instance != null)
                instance.IsDead = true;

            var newObject = new DiveEnemy(instance.player, instance.camera, instance.Size, instance.speedX, instance.speedY);
            newObject.Position = new Vector2(instance.player.Position.X, instance.player.Position.Y - 64 * 8);
            newObject.ObjectsManager = instance.ObjectsManager;

            newObject.ObjectsManager.AddGameObject(newObject);

            instance = newObject;
        }

        private static void RollSpawnChance()
        {
            if (spawnFlag) return;

            var random = GameDevice.Instance().Random;
            if (random.Next(0, 800) < 1)
            {
                spawnFlag = true;
            }
        }

        private bool IsOutOfScreenUp()
        {
            return Position.Y + Size.Y / 2 < camera.Position.Y - Screen.HEIGHT / 2;
        }
    }
}
