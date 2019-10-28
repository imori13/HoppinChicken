using System;
using System.Collections.Generic;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.GameObjects.Enemys;
using FliedChicken.GameObjects.PlayerDevices;

namespace FliedChicken.UI
{
    class DiveEnemyUI
    {
        //表示するかどうか
        bool display;

        Vector2 position;
        Player player;
        DiveEnemy diveEnemy;
        Camera camera;
        Vector2 size;

        public DiveEnemyUI(Camera camera, DiveEnemy diveEnemy, Player player)
        {
            this.player = player;
            this.camera = camera;
            this.diveEnemy = diveEnemy;
        }

        public void Initialize()
        {
            display = false;
            position = new Vector2(diveEnemy.Position.X - camera.Position.X + Screen.WIDTH / 2.0f, 50);
            size = Vector2.One;
        }

        public void Update()
        {
            DisplayOFF();
            if (display)
            {
                DisplayON();
            }
        }

        private void DisplayON()
        {
            position = new Vector2(diveEnemy.Position.X - camera.Position.X + Screen.WIDTH / 2.0f, 50);
            if (diveEnemy.Position.Y >= camera.Position.Y - Screen.HEIGHT / 2.0f)
            {
                display = false;
            }
        }

        private void DisplayOFF()
        {
            if (diveEnemy.Position.Y < camera.Position.Y - Screen.HEIGHT / 2.0f)
            {
                display = true;
            }
        }

        public void Draw(Renderer renderer)
        {
            if (display)
            {
                float limit = 500;

                float distance = MathHelper.Clamp(Vector2.Distance(camera.Position - new Vector2(0, Screen.HEIGHT / 2f), diveEnemy.Position), 0, limit);

                float rate = MathHelper.Lerp(1, 0.5f, distance / limit);
                size = Vector2.Lerp(size, Vector2.One * rate, 0.1f);

                renderer.Draw2D("DiveEnemyUI", position, Color.White, 0, size);
            }
        }
    }
}
