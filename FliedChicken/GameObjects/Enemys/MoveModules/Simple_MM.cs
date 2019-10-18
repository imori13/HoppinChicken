using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Enemys.MoveModules
{
    class Simple_MM : MoveModule
    {
        public Vector2 MoveDirection { get; set; }
        public float MoveSpeed { get; set; }

        public Simple_MM(GameObject GameObject, Vector2 moveDirection, float moveSpeed) : base(GameObject)
        {
            MoveDirection = Vector2.Normalize(moveDirection);
            MoveSpeed = moveSpeed;
        }

        public override void Initialize()
        {
        }

        public override void Move()
        {
            GameObject.Position += MoveDirection * MoveSpeed * TimeSpeed.Time;
        }
    }
}
