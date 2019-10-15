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
    class Coin : GameObject
    {
        private CoinManager coinManager;

        public Coin(CoinManager coinManager)
        {
            this.coinManager = coinManager;
            Collider = new BoxCollider(this, new Vector2(128, 128));
        }

        public override void Initialize()
        {
        }

        public override void Update()
        {
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D("Pixel", Position, Color.LightGoldenrodYellow, 0.0f, new Vector2(128, 128));
        }

        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.Player)
            {
                IsDead = true;
                coinManager.OnGetCoin();
            }
        }

        public void Destroy()
        {
            IsDead = true;
        }
    }
}
