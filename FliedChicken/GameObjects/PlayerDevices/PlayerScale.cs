using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.PlayerDevices
{
    class PlayerScale
    {
        Player player;

        private const float xExpandSpeed = 1.0f;
        private const float xShrinkSpeed = 0.05f;

        private const float yExpandSpeed = 0.05f;
        private const float yShrinkSpeed = 1.0f;

        private const float xMinScale = 1.0f;
        private const float xMaxScale = 1.25f;

        private const float yMinScale = 0.7f;
        private const float yMaxScale = 1.0f;

        private Vector2 preVelocity;

        public Vector2 DrawScale { get; private set; }

        public PlayerScale(Player player)
        {
            this.player = player;
        }

        public void Initialize()
        {
            DrawScale = new Vector2(1, 1);
        }

        public void Update()
        {
            float newX = DrawScale.X;
            float newY = DrawScale.Y;

            //前フレームの速度より早い場合
            if (player.Velocity.Y >= preVelocity.Y)
            {
                //縦に伸びて横に縮む
                newX = MathHelper.Lerp(newX, xMinScale, xShrinkSpeed);
                newY = MathHelper.Lerp(newY, yMaxScale, yExpandSpeed);
            }
            else
            {
                //横に伸びて縦に縮む
                newX = MathHelper.Lerp(newX, xMaxScale, xExpandSpeed);
                newY = MathHelper.Lerp(newY, yMinScale, yShrinkSpeed);
            }

            DrawScale = new Vector2(newX, newY);

            preVelocity = player.Velocity;
        }
    }
}
