using FliedChicken.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.UI
{
    class T_KeyWindow
    {
        string text;
        float time;
        float limit = 0.05f;

        public T_KeyWindow()
        {

        }

        public void Initialize()
        {
            text = "";
        }

        public void Update()
        {
            Keys[] keys = Input.GetPressedKey();

            if (text.Length < 20)
            {
                foreach (var key in keys)
                {
                    if (key.ToString() == "Space") { text += " "; }
                    else { text += key; }
                }
            }

            if (text.Length > 0 && Input.GetKey(Keys.Back))
            {
                if (Input.GetKeyDown(Keys.Back))
                {
                    time = -0.5f;
                    text = text.Remove(text.Length - 1, 1);
                }

                time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                if (time > limit)
                {
                    time = 0;
                    text = text.Remove(text.Length - 1, 1);
                }
            }

            if (Input.GetKeyUp(Keys.Back))
            {
                time = 0;
            }
        }

        public void Draw(Renderer renderer)
        {
            SpriteFont font = Fonts.Font32;
            Vector2 size = font.MeasureString(text);

            renderer.Draw2D("Pixel", Screen.Vec2 / 2f, Color.Black, 0,  size + new Vector2(200, 0));

            renderer.DrawString(font, text, Screen.Vec2 / 2f, Color.White, 0, size / 2f, Vector2.One);
        }
    }
}
