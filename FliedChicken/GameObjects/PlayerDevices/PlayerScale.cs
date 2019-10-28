using FliedChicken.Devices;
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

        private const float xExpandSpeed = 0.5f;
        private const float xShrinkSpeed = 0.1f;

        private const float yExpandSpeed = 0.1f;
        private const float yShrinkSpeed = 0.5f;

        private const float xMinScale = 1.0f;
        private const float xMaxScale = 1.25f;

        private const float yMinScale = 0.7f;
        private const float yMaxScale = 1.0f;

        private Vector2 preVelocity;

        public Vector2 DrawScale { get; private set; }

        bool flag;
        float time;
        float limit = 0.1f;

        public PlayerScale(Player player)
        {
            this.player = player;
        }

        public void Initialize()
        {
            DrawScale = new Vector2(1, 1);
            flag = false;
        }

        public void Update()
        {
            float newX = DrawScale.X;
            float newY = DrawScale.Y;
            
            if (flag)
            {
                time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                //横に伸びて縦に縮む
                newX = MathHelper.Lerp(newX, xMaxScale, xExpandSpeed);
                newY = MathHelper.Lerp(newY, yMinScale, yShrinkSpeed);
                
                if (time >= limit)
                {
                    time = 0;
                    flag = false;
                }
            }
            else
            {
                //縦に伸びて横に縮む
                newX = MathHelper.Lerp(newX, xMinScale, xShrinkSpeed);
                newY = MathHelper.Lerp(newY, yMaxScale, yExpandSpeed);
            }

            DrawScale = new Vector2(newX, newY);

            preVelocity = player.Velocity;
        }

        public void Jump()
        {
            flag = true;
        }
    }
}
