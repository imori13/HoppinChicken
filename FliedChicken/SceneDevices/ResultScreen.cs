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

        private readonly float maxWindowWidth = Screen.WIDTH;
        private readonly float maxWindowHeight = Screen.HEIGHT - 200;

        private float windowAlpha;

        private Vector2 maxWindowSize;

        public bool IsDead { get; private set; }

        ScreenState state;

        private Vector2 textPosition01;
        private Vector2 textPosition02;
        private Vector2 textPosition04;

        private float scoreAlpha;

        private readonly int scoreCheckNum = 4;
        private string[] scoreCheckST;
        private float[] scoreCheckFL;
       
        private int checkNum;
        
        public ResultScreen()
        {
        }

        /// <summary>
        /// スコア設定用メソッド
        /// </summary>
        /// <param name="cleartime"></param>
        /// <param name="coinNum"></param>
        private void SetScore(float score)
        {
            this.score = score;
        }

        private void ScoreCheck(float score)
        {
            int scoreCheck = scoreCheckNum - 1;
            for (int i = scoreCheckNum - 2; i >= 0; i--)
            {
                if (scoreCheckFL[i] <= score)
                {
                    scoreCheck = i;
                }
            }
            checkNum = scoreCheck;
        }

        public void Initialize(float score)
        {
            SetScore(score);

            maxWindowSize = new Vector2(maxWindowWidth, maxWindowHeight);
            windowAlpha = 0.0f;

            IsDead = false;
            state = ScreenState.START;

            textPosition01 = new Vector2(Screen.Vec2.X, 200);
            textPosition02 = new Vector2(Screen.Vec2.X * 4 / 5, Screen.Vec2.Y * 1 / 2);
            textPosition04 = new Vector2(Screen.Vec2.X / 2, Screen.Vec2.Y * 3 / 4);

            scoreAlpha = 0.0f;
            checkNum = 0;

            scoreCheckST = new string[scoreCheckNum];
            scoreCheckFL = new float[scoreCheckNum - 1];

            scoreCheckST = new string[]
            {
                "S", "A", "B", "C"
            };

            scoreCheckFL = new float[]
            {
                750, 500, 250
            };

            ScoreCheck(score);
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
            windowAlpha += 0.1f * TimeSpeed.Time;
            if (windowAlpha >= 0.5f)
            {
                windowAlpha = 0.5f;
                state = ScreenState.STATE01;
            }
        }

        private void State01()
        {
            textPosition01 -= new Vector2(100, 0) * TimeSpeed.Time;
            if (textPosition01.X <= 50)
            {
                textPosition01.X = 50;
                state = ScreenState.STATE02;
            }
        }

        private void State02()
        {
            scoreAlpha += 0.1f * TimeSpeed.Time;
            if (scoreAlpha >= 1.0f)
            {
                scoreAlpha = 1.0f;
                state = ScreenState.STATE03;
            }
        }

        private void State03()
        {
            if (Input.GetKeyDown(Keys.Space) || Input.IsPadButtonDown(Buttons.B, 0) || Input.IsPadButtonDown(Buttons.A, 0))
            {
                state = ScreenState.FINISH;
            }
        }

        private void Finish()
        {
            textPosition01 -= new Vector2(100, 0) * TimeSpeed.Time;
            textPosition02 -= new Vector2(100, 0) * TimeSpeed.Time;
            scoreAlpha -= 0.1f * TimeSpeed.Time;
            if (textPosition02.X <= 0.0f)
            {
                textPosition02.X = 0.0f;
                IsDead = true;
            }
        }

        public void End()
        {

        }

        public float GetWindowAlpha()
        {
            return windowAlpha;
        }

        public void Draw(Renderer renderer)
        {
            renderer.Draw2D("Pixel", Screen.Vec2 / 2,
                Color.Black * windowAlpha, 0.0f, maxWindowSize);

            renderer.DrawString(Fonts.Font10_128, "RESULT", textPosition01, Color.White, 
                0.0f, new Vector2(0, Fonts.Font10_128.MeasureString("RESULT").Y / 2), Vector2.One);

            renderer.DrawString(Fonts.Font10_128, "SCORE", textPosition01 + new Vector2(0, 150), Color.White,
                0.0f, new Vector2(0, Fonts.Font10_128.MeasureString("SCORE").Y / 2), new Vector2(0.7f, 0.7f));

            renderer.DrawString(Fonts.Font10_128, score.ToString("F2") + "m", textPosition02, Color.White * scoreAlpha,
                0.0f, new Vector2(Fonts.Font10_128.MeasureString(score.ToString("F2") + "m").X, Fonts.Font10_128.MeasureString(score.ToString() + "m").Y / 2),
                new Vector2(1.5f, 1.5f));

            renderer.DrawString(Fonts.Font10_128, "rank", textPosition01 + new Vector2(500, 500), Color.White,
                0.0f, Fonts.Font10_128.MeasureString("rank") / 2, new Vector2(0.7f, 0.7f));

            renderer.DrawString(Fonts.Font10_128, scoreCheckST[checkNum], textPosition04, Color.White * scoreAlpha,
                0.0f, Fonts.Font10_128.MeasureString(scoreCheckST[checkNum])/ 2, new Vector2(1.5f, 1.5f));
        }
    }
}
