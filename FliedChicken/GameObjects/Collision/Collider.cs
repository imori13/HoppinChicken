using FliedChicken.Devices;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Collision
{
    abstract class Collider
    {
        public GameObject gameobject { get; protected set; }
        public ColliderEnum ColliderEnum { get; private set; }

        public Collider(GameObject gameobject, ColliderEnum colliderEnum)
        {
            this.gameobject = gameobject;
            this.ColliderEnum = colliderEnum;
        }

        public abstract bool IsCollision(Collider collider);

        public abstract void Draw(Renderer renderer);
    }
}
