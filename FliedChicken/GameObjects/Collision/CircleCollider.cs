using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Collision
{
    class CircleCollider : Collider
    {
        public float Radius { get; private set; }

        public CircleCollider(GameObject gameobject, float radius)
            : base(gameobject, ColliderEnum.Circle)
        {
            this.Radius = radius;
        }

        public override bool IsCollision(Collider collider)
        {
            switch (collider.ColliderEnum)
            {
                case ColliderEnum.None: return false;
                case ColliderEnum.Circle: return CircleCollision(collider as CircleCollider);
                case ColliderEnum.Box: return BoxCollision(collider as BoxCollider);
            }

            return false;
        }

        // 円と円の当たり判定
        bool CircleCollision(CircleCollider collider)
        {
            if (Vector2.DistanceSquared(gameobject.Position, collider.gameobject.Position)
                < (Radius + collider.Radius) * (Radius + collider.Radius))
            {
                return true;
            }

            return false;
        }

        // 円と四角形
        bool BoxCollision(BoxCollider collider)
        {
            Vector2 nearPoint = 
                Vector2.Clamp(
                    gameobject.Position, collider.gameobject.Position - collider.Size,
                    collider.gameobject.Position + collider.Size);

            if (Vector2.DistanceSquared(nearPoint, gameobject.Position) < (Radius * Radius))
            {
                return true;
            }

            return false;
        }
    }
}
