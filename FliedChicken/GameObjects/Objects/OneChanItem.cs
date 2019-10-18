using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Collision;

namespace FliedChicken.GameObjects.Objects
{
    class OneChanItem : GameObject
    {
        public OneChanItem()
        {
            GameObjectTag = GameObjectTag.OneChanceItem;
            Collider = new CircleCollider(this, 50f);
        }

        public override void Initialize()
        {

        }

        public override void Update()
        {

        }
        public override void Draw(Renderer renderer)
        {

        }

        public override void HitAction(GameObject gameObject)
        {

        }
    }
}
