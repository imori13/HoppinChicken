using FliedChicken.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.SceneDevices.Title
{
    class KeyInput
    {
        public string Text { get; private set; }
        float time;
        float limit = 0.05f;
        bool flag;

        public KeyInput()
        {

        }

        public void Initialize()
        {
            Text = "";
        }

        public void Update()
        {
            Keys[] keys = Input.GetPressedKey();

            if (Text.Length < 10)
            {
                foreach (var key in keys)
                {
                    if (((int)key >= 48 && (int)key <= 57))
                    {
                        Text += key.ToString().Substring(1, 1);
                    }
                    else if (key == Keys.OemBackslash)
                    {
                        Text += "_";
                    }
                    else
                    {
                        if (Input.GetKey(Keys.LeftShift) || Input.GetKey(Keys.RightShift))
                        {
                            Text += key.ToString().ToUpper();
                        }
                        else
                        {
                            Text += key.ToString().ToLower();
                        }
                    }
                }
            }

            if (Text.Length > 0 && Input.GetKey(Keys.Back))
            {
                if (Input.GetKeyDown(Keys.Back))
                {
                    time = -0.5f;
                    Text = Text.Remove(Text.Length - 1, 1);
                }

                time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                if (time > limit)
                {
                    time = 0;
                    Text = Text.Remove(Text.Length - 1, 1);
                }
            }

            if (Input.GetKeyUp(Keys.Back))
            {
                time = 0;
            }
        }

        public void Draw(Renderer renderer, float rate, Vector2 pos)
        {
            if (rate != 0 && Text == "") { flag = true; Text = "Player"; }

            SpriteFont font = Fonts.Font12_32;
            Vector2 size = font.MeasureString(Text);

            renderer.DrawString(font, Text, pos,
                 (flag) ? new Color(0, 0, 0, 0) : new Color(255, 91, 91),
                0, size / 2f, Vector2.One * (1 - (1 - rate) / 5f) * Screen.ScreenSize);
        }
    }
}
