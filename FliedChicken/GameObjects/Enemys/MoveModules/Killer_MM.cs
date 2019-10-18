using FliedChicken.GameObjects.PlayerDevices;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Enemys.MoveModules
{
    class Killer_MM : MoveModule
    {
        Player player;

        Vector2 velocity;

        public Killer_MM(GameObject GameObject) : base(GameObject)
        {
            player = GameObject.ObjectsManager.Player;
        }

        public override void Initialize()
        {

        }

        public override void Move()
        {
            Vector2 direction = player.Position - GameObject.Position;
            direction.Normalize();
            float speed = 1;
            velocity = Vector2.Lerp(velocity, direction * speed, 0.1f);
        }
    }
}
