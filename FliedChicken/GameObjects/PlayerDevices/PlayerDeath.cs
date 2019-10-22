using FliedChicken.Devices;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.PlayerDevices
{
    // 死んだプレイヤーを描画する
    class PlayerDeath
    {
        public Player Player { get; private set; }

        Random rand = GameDevice.Instance().Random;

        bool initFlag;

        Vector2 position;
        Vector2 velocity;
        float rotation;
        float rotation_speed;

        public PlayerDeath(Player Player)
        {
            this.Player = Player;
        }

        public void Initialize()
        {
            initFlag = false;
        }

        public void Update()
        {
            // ヒットした瞬間なら
            if (!initFlag)
            {
                initFlag = true;

                Player.ObjectsManager.Camera.Shake(40, 5, 0.95f);

                float XYspeed = 5;
                float randomNum = rand.Next(2);
                velocity = new Vector2(randomNum == 0 ? XYspeed : -XYspeed, -20);
                rotation = 0;
                rotation_speed = randomNum == 0 ? 15 : -15;

                position = Player.Position;
            }

            velocity.Y += 1.1f * TimeSpeed.Time;

            position += velocity * TimeSpeed.Time;

            rotation += rotation_speed * TimeSpeed.Time;
        }

        public void Draw(Renderer renderer)
        {
            renderer.Draw2D("PlayerDead", position, Color.White, MathHelper.ToRadians(rotation), new Vector2(57, 57), Vector2.One * 0.5f);
        }
    }
}
