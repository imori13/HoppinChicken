using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Collision
{
    // 軸に平行な矩形。回転はしない
    class BoxCollider : Collider
    {
        // 中心からのX,Y,Zの距離
        public Vector2 Size { get; private set; }

        public BoxCollider(GameObject gameobject, Vector2 size)
            : base(gameobject, ColliderEnum.Box)
        {
            Size = size;
        }

        public override bool IsCollision(Collider collider)
        {
            switch (collider.ColliderEnum)
            {
                case ColliderEnum.None: return false;
                case ColliderEnum.Box: return BoxCollision(collider as BoxCollider);
                case ColliderEnum.Circle: return CircleCollision(collider as CircleCollider);
            }

            return false;
        }

        public bool BoxCollision(BoxCollider collider)
        {
            if (gameobject.Position.X - Size.X/2f < collider.gameobject.Position.X + collider.Size.X / 2f &&
                gameobject.Position.X + Size.X / 2f > collider.gameobject.Position.X - collider.Size.X / 2f &&
                gameobject.Position.Y - Size.Y / 2f < collider.gameobject.Position.Y + collider.Size.Y / 2f &&
                gameobject.Position.Y + Size.Y / 2f > collider.gameobject.Position.Y - collider.Size.Y / 2f)
            {
                return true;
            }

            return false;
        }

        // BoxとCircle
        public bool CircleCollision(CircleCollider collider)
        {
            Vector2 nearPoint = Vector2.Clamp(collider.gameobject.Position, gameobject.Position - Size, gameobject.Position + Size);

            if (Vector2.DistanceSquared(nearPoint, collider.gameobject.Position) < (collider.Radius * collider.Radius))
            {
                return true;
            }

            return false;
        }
    }
}
