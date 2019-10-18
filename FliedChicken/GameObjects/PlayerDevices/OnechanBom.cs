using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Collision;
using Microsoft.Xna.Framework;

namespace FliedChicken.GameObjects.PlayerDevices
{
    class OnechanBom : GameObject
    {
        Player player;
        float time;
        float deathLimit = 0.5f;

        public OnechanBom(Player player, Vector2 position)
        {
            this.player = player;
            GameObjectTag = GameObjectTag.OneChanBom;
            Collider = new CircleCollider(this, 500f);
            Position = position;
        }

        public override void Initialize()
        {
            time = 0;
        }

        public override void Update()
        {
            Position = player.Position;

            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

            if (time >= deathLimit)
            {
                IsDead = true;
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
