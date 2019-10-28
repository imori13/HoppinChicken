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
using System.IO;
using FliedChicken.SceneDevices.Title;

namespace FliedChicken.SceneDevices
{
    /// <summary>
    /// ランキング管理かつ表示クラス
    /// </summary>
    class RankingScreen
    {
        enum ScreenState
        {
            START,
            STATE01,
            STATE02,
            STATE03,
            FINISH,
        }

        private readonly float maxWindowWidth = Screen.WIDTH;
        private readonly float maxWindowHeight = Screen.HEIGHT - 200;

        private Vector2 maxWindowSize;
        private float windowAlpha;

        readonly int rankNum = 10;

        ScreenState state;

        public bool IsDead { get; private set; }

        private Vector2 textPosition01;

        private float time;

        private TitleDisplayMode titleDisplayMode;

        public RankingScreen(TitleDisplayMode titleDisplayMode)
        {
            this.titleDisplayMode = titleDisplayMode;
        }

        public void Initialize(float alpha)
        {

            state = ScreenState.STATE01;

            IsDead = false;

            maxWindowSize = new Vector2(maxWindowWidth, maxWindowHeight);
            windowAlpha = alpha;

            textPosition01 = new Vector2(Screen.Vec2.X, Screen.Vec2.Y / 2);

            time = 0.0f;
        }

        public void InitializeTitle()
        {
            state = ScreenState.STATE01;

            IsDead = false;

            maxWindowSize = new Vector2(maxWindowWidth, maxWindowHeight);
            windowAlpha = 0.0f;

            textPosition01 = new Vector2(Screen.Vec2.X, Screen.Vec2.Y / 2);

            time = 0.0f;
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
                state = ScreenState.STATE01;
            }
        }

        private void State01()
        {
            textPosition01 -= new Vector2(100, 0) * TimeSpeed.Time;
            if (textPosition01.X <= 0)
            {
                textPosition01.X = 0;
                state = ScreenState.STATE02;
            }
        }

        private void State02()
        {
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            if (Input.GetKeyDown(Keys.Space))
            {
                IsDead = true;
            }
        }

        private void State03()
        {

        }

        private void Finish()
        {

        }

        public void End()
        {

        }

        public void Draw(Renderer renderer)
        {
            renderer.Draw2D("Pixel", Screen.Vec2 / 2f,
                Color.Black * windowAlpha, 0.0f, maxWindowSize);

            windowAlpha = MathHelper.Lerp(windowAlpha, 0.5f, 0.1f);

            renderer.DrawString(Fonts.Font10_128, "RANKING", textPosition01 + new Vector2(Screen.WIDTH / 2f, -400), new Color(255, 91, 91),
                0.0f, Fonts.Font10_128.MeasureString("RANKING") / 2f, new Vector2(0.7f, 0.7f));

            SpriteFont font = Fonts.Font12_32;

            // スコアデータを読み込み
            Dictionary<string, float> dicionary = ScoreStream.Instance().GetScoreDictionary();

            // ディクショナリのValueが大きい順に並べる
            var dicSortData = dicionary.OrderByDescending((x) => x.Value);

            int index = 0;
            foreach (var data in dicSortData)
            {
                if (index >= 50) { break; }

                // 描画するテキスト
                string text = (index + 1).ToString("00") + "位 " + data.Key.PadRight(10, ' ') + ": " + data.Value.ToString("F2").PadLeft(8, '0') + "ｍ";

                Vector2 size = font.MeasureString(text);

                Vector2 position = Vector2.Zero;

                float y = -300;

                if (index >= 30)
                {
                    position = textPosition01 + new Vector2(Screen.WIDTH / 2f + 625, y + (size.Y * (index - 30)));
                }
                else if (index >= 15)
                {
                    position = textPosition01 + new Vector2(Screen.WIDTH / 2f, y + (size.Y * (index - 15)));
                }
                else
                {
                    position = textPosition01 + new Vector2(Screen.WIDTH / 2f - 625, y + (size.Y * index));
                }

                Color color = (titleDisplayMode.keyInput.Text == data.Key) ? (new Color(255, 91, 91, 255)) : (Color.White);

                renderer.DrawString(
                    font, text,
                    position,
                    color,
                    0, new Vector2(size.X / 2f, size.Y / 2f),
                    Vector2.One);

                index++;
            }
        }
    }
}
