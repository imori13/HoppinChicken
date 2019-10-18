using System;
using System.Collections.Generic;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
using FliedChicken.Objects;
using FliedChicken.Particle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.UI
{
    class DiveEnemyUI
    {
        //表示するかどうか
        bool display;

        Vector2 position;
        DiveEnemy diveEnemy;
        Camera camera;

        public DiveEnemyUI(Camera camera, DiveEnemy diveEnemy)
        {
            this.camera = camera;
            this.diveEnemy = diveEnemy;
        }

        public void Initialize()
        {
            display = false;
            position = Vector2.Zero;
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
            position = new Vector2(diveEnemy.Position.X - camera.Position.X, 0);
            if (diveEnemy.Position.Y >= camera.Position.Y - Screen.HEIGHT / 2.0f)
            {
                display = false;
            }
        }
        
        private void DisplayOFF()
        {
            position = Vector2.Zero;
            if (diveEnemy.Position.Y < camera.Position.Y - Screen.HEIGHT / 2.0f)
            {
                display = true;
            }
        }

        public void Draw(Renderer renderer)
        {
            if (display)
            {
                renderer.Draw2D("", position, Color.White);
            }
        }
    }
}
