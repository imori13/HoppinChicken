using System;
using System.Collections.Generic;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
using FliedChicken.Particle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.SceneDevices
{
    /// <summary>
    /// リザルト管理かつ表示クラス
    /// </summary>
    class ResultScreen
    {
        enum ScreenState
        {
            START,
            STATE01,
            STATE02,
            FINISH,
        }

        private float cleartime;
        private int coinNum;
        private string timeText;

        private float windowWidth;
        private float windowHeight;

        private readonly float maxWindowWidth = Screen.WIDTH - 300;
        private readonly float maxWindowHeight = Screen.HEIGHT - 100;

        private Vector2 maxWindowSize;

        private float textSize01;
        private float textSize02;
        private float timeTextSize;

        private float timeAlpha;
        private float allAlpha;

        public bool IsDead { get; private set; }

        Camera camera;

        ScreenState state;

        float startTime;
        float finishTime;

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

        public void Initialize(float cleatime, int coinNum)
        {
            SetScore(cleatime, coinNum);

            windowWidth = 0.0f;
            windowHeight = maxWindowHeight;
            maxWindowSize = new Vector2(maxWindowWidth, maxWindowHeight);

            textSize01 = 0.0f;
            textSize02 = 0.0f;
            timeTextSize = 3.0f;

            timeAlpha = 0.0f;
            allAlpha = 1.0f;

            IsDead = false;
            state = ScreenState.START;

            startTime = 0.0f;
            finishTime = 0.0f;
        }

        public void Update()
        {
            Default();

            switch (state)
            {
                case ScreenState.START:
                    Start();
                    break;
                case ScreenState.STATE01:
                    State01();
                    break;
                case ScreenState.STATE02:
                    State02();
                    break;
                case ScreenState.FINISH:
                    Finish();
                    break;
            }
        }

        private void Default()
        {

        }

        private void Start()
        {
            startTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            windowWidth = MathHelper.Lerp(windowWidth, maxWindowWidth, 0.05f);
            textSize01 = MathHelper.Lerp(textSize01, 2, 0.05f);
            textSize02 = MathHelper.Lerp(textSize02, 1.5f, 0.05f);
            allAlpha += TimeSpeed.Time * 0.01f;
            if (startTime >= 1.5f)
            {
                windowWidth = maxWindowWidth;
                textSize01 = 2;
                textSize02 = 1.5f;
                allAlpha = 1.0f;
                state = ScreenState.STATE01;
            }
        }

        private void State01()
        {
            timeAlpha += TimeSpeed.Time * 0.01f;
            if (timeAlpha >= 1)
            {
                state = ScreenState.STATE02;
            }
        }

        private void State02()
        {
            if (Input.GetKeyDown(Keys.A))
            {
                state = ScreenState.FINISH;
            }
        }

        private void Finish()
        {
            finishTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            windowWidth = MathHelper.Lerp(windowWidth, 0, 0.05f);
            textSize01 = MathHelper.Lerp(textSize01, 0, 0.05f);
            textSize02 = MathHelper.Lerp(textSize02, 0, 0.05f);
            timeTextSize = MathHelper.Lerp(timeTextSize, 0, 0.05f);
            allAlpha -= TimeSpeed.Time * 0.01f;
            if (finishTime >= 1.5f)
            {
                windowWidth = 0;
                textSize01 = 0;
                textSize02 = 0;
                timeTextSize = 0;
                IsDead = true;
            }
        }

        public void End()
        {

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
            renderer.Draw2D("Pixel", new Vector2(camera.Position.X, camera.Position.Y),
                Color.Black, 0.0f, new Vector2(windowWidth, windowHeight));

            renderer.DrawString(Fonts.Font12_32, "RESULT",
                new Vector2(camera.Position.X, camera.Position.Y - 400), Color.White * allAlpha,
                0.0f, Fonts.Font12_32.MeasureString("RESULT") / 2, new Vector2(textSize01, 2));

            renderer.DrawString(Fonts.Font12_32, "TIME",
                new Vector2(camera.Position.X, camera.Position.Y - 300), Color.White * allAlpha,
                0.0f, Fonts.Font12_32.MeasureString("TIME") / 2, new Vector2(textSize02, 1.5f));

            renderer.DrawString(Fonts.Font12_32, timeText,
                new Vector2(camera.Position.X, camera.Position.Y - 200), Color.White * allAlpha * timeAlpha,
                0.0f, Fonts.Font12_32.MeasureString(timeText) / 2, new Vector2(timeTextSize, 3));
        }
    }
}
