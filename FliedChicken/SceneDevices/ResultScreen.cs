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
            STATE03,
            FINISH,
        }

        private float score;

        private float windowWidth;
        private float windowHeight;

        private readonly float maxWindowWidth = Screen.WIDTH - 300;
        private readonly float maxWindowHeight = Screen.HEIGHT - 100;

        private Vector2 maxWindowSize;

        private float allAlpha;

        public bool IsDead { get; private set; }

        ScreenState state;

        float startTime;
        float finishTime;

        public ResultScreen()
        {
        }

        /// <summary>
        /// スコア設定用メソッド
        /// </summary>
        /// <param name="cleartime"></param>
        /// <param name="coinNum"></param>
        public void SetScore(float score)
        {
            this.score = score;
        }

        public void Initialize(float  score)
        {
            SetScore(score);

            windowWidth = 0.0f;
            windowHeight = maxWindowHeight;
            maxWindowSize = new Vector2(maxWindowWidth, maxWindowHeight);

            allAlpha = 0.0f;

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
                case ScreenState.STATE03:
                    State03();
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
            if (startTime >= 1.5f)
            {
                windowWidth = maxWindowWidth;
                state = ScreenState.STATE01;
            }
        }

        private void State01()
        {
            allAlpha += TimeSpeed.Time * 0.01f;
            if (allAlpha >= 1)
            {
                allAlpha = 1.0f;
                state = ScreenState.STATE02;
            }
        }

        private void State02()
        {
            if (Input.GetKeyDown(Keys.A))
            {
                state = ScreenState.STATE03;
            }
        }

        private void State03()
        {
            allAlpha -= TimeSpeed.Time * 0.01f;
            if (allAlpha <= 0.0f)
            {
                allAlpha = 0.0f;
                state = ScreenState.FINISH;
            }
        }

        private void Finish()
        {
            finishTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            windowWidth = MathHelper.Lerp(windowWidth, 0, 0.05f);
            if (finishTime >= 1.5f)
            {
                windowWidth = 0.0f;
                IsDead = true;
            }
        }

        public void End()
        {

        }

        public void Draw(Renderer renderer)
        {
            renderer.Draw2D("Pixel", Screen.Vec2 / 2,
                Color.Black, 0.0f, new Vector2(windowWidth, windowHeight));

            renderer.DrawString(Fonts.Font12_32, "RESULT",
                Screen.Vec2 / 2 - new Vector2(0, 400), Color.White * allAlpha,
                0.0f, Fonts.Font12_32.MeasureString("RESULT") / 2, new Vector2(2, 2));

            renderer.DrawString(Fonts.Font12_32, "SCORE",
                Screen.Vec2 / 2 - new Vector2(0, 300), Color.White * allAlpha,
                0.0f, Fonts.Font12_32.MeasureString("SCORE") / 2, new Vector2(1.5f, 1.5f));

            renderer.DrawString(Fonts.Font12_32, score.ToString("F2") + "m",
                Screen.Vec2 / 2 - new Vector2(0, 200), Color.White * allAlpha,
                0.0f, Fonts.Font12_32.MeasureString(score.ToString("F2") + "m") / 2, new Vector2(3, 3));
        }
    }
}
