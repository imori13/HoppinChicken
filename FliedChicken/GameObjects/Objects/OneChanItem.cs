using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using FliedChicken.Devices;
using FliedChicken.GameObjects.Collision;

namespace FliedChicken.GameObjects.Objects
{
    class OneChanItem : GameObject
    {
        public bool PlayerHitEnabled { get; set; }

        private float drawScale;
        private float timeElapsed;

        private float rotation;
        private float rotateSpeed = 3.0f;
        private IOneChanItemCarrier carrier;

        public OneChanItem(IOneChanItemCarrier carrier = null)
        {
            GameObjectTag = GameObjectTag.OneChanceItem;
            Collider = new CircleCollider(this, 50f);
            this.carrier = carrier;

            drawScale = 1.0f;
            PlayerHitEnabled = (carrier == null);
        }

        public override void Initialize()
        {

        }

        public override void Update()
        {
            if (carrier == null) return;

            Position = carrier.GetItemPosition();
            rotation += rotateSpeed * TimeSpeed.Time;

            timeElapsed += TimeSpeed.Time * 3;
            float absSin = Math.Abs((float)Math.Sin(MathHelper.ToRadians(timeElapsed)));
            drawScale = 1 + absSin * 0.5f;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D("Star", Position, Color.White, rotation, Vector2.One * drawScale);
        }

        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.Player)
            {
                IsDead = true;
            }
        }

        public void Destroy()
        {
            IsDead = true;
        }
    }
}
