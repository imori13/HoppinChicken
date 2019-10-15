using System;
using System.Collections.Generic;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
using FliedChicken.Objects;
using FliedChicken.Particle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.GameObjects.Objects;

namespace FliedChicken.SceneDevices
{
    /// <summary>
    /// リザルト管理かつ表示クラス
    /// </summary>
    class ResultScreen
    {
        private float cleartime;
        private int coinNum;
        private string timeText;

        Camera camera;

        public ResultScreen(Camera camera)
        {
            this.camera = camera;
        }

        /// <summary>
        /// スコア設定用メソッド
        /// </summary>
        /// <param name="cleartime"></param>
        /// <param name="coinNum"></param>
        public void SetScore(float cleartime, int coinNum)
        {
            this.cleartime = cleartime;
            this.coinNum = coinNum;
            ChangeTimerText();
        }

        /// <summary>
        /// タイム表示
        /// </summary>
        public void ChangeTimerText()
        {
            string min = (((int)cleartime) / 60).ToString();
            string se = (((int)cleartime) - int.Parse(min) * 60).ToString("00");
            string f = (cleartime - ((int)cleartime)).ToString("F2").TrimStart('0');
            timeText = min + ":" + se + f;
        }

        public void Draw(Renderer renderer)
        {
            renderer.Draw2D("Pixel", new Vector2(camera.Position.X - 600, camera.Position.Y - Screen.HEIGHT / 2),
                Color.White, 0.0f, new Vector2(1200, Screen.HEIGHT));

            renderer.DrawString(Fonts.Font12_32, "RESULT",
                new Vector2(camera.Position.X - 180, camera.Position.Y - Screen.HEIGHT / 2), Color.Red,
                0.0f, Vector2.Zero, new Vector2(3, 3));
            
            renderer.DrawString(Fonts.Font12_32, timeText,
                new Vector2(camera.Position.X - 180, camera.Position.Y), Color.Red,
                0.0f, Vector2.Zero, new Vector2(3, 3));
        }
    }
}
