using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices;
using FliedChicken.Utilities;
using FliedChicken.GameObjects.Bullets;

namespace FliedChicken.GameObjects.Enemys.AttackModules
{
    class SimpleShoot_AM : AttackModule
    {
        private ObjectsManager objectsManager;
        private Timer shootTimer;
        private Vector2 shootPos;
        private Vector2 shootDirection;

        public SimpleShoot_AM(
            GameObject GameObject,
            ObjectsManager objectsManager,
            Vector2 shootPos,
            Vector2 shootDirection) : base(GameObject)
        {
            shootTimer = new Timer(1);
            this.objectsManager = objectsManager;
            this.shootPos = shootPos;
            this.shootDirection = shootDirection;
        }

        public override void Attack()
        {
            if (shootTimer.IsTime())
            {
                var bullet = new NormalBullet(objectsManager.Camera,shootDirection, 16.0f);
                bullet.Position = shootPos + GameObject.Position;
                objectsManager.AddGameObject(bullet);
                shootTimer.Reset();
            }
        }

        public override void Initialize()
        {
        }
    }
}
