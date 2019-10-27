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
    class TitleDisplayMode
    {
        float destSizeY;
        float startBack01;  // 最初上がってくるの背景１
        float startBack02;  // 最初上がってくるの背景２
        float finishBack01; // 横にどいていく背景
        float finishBack02;
        float finishBack03;
        public KeyInput keyInput { get; private set; }
        bool startFlag;
        bool finishingFlag;
        public bool TitleFinishFlag { get; private set; }

        Vector2 gameStartTextPos;
        Vector2 inputKeyPos;
        float rate;

        float endingTime;

        RankingScreen rankingScreen;

        bool rankingON;

        public TitleDisplayMode()
        {
            keyInput = new KeyInput();
            rankingScreen = new RankingScreen();
        }

        public void Initialize()
        {
            destSizeY = 0;
            keyInput.Initialize();
            gameStartTextPos = new Vector2(Screen.WIDTH / 2f + 250, 150);
            inputKeyPos = new Vector2(Screen.WIDTH / 2f, Screen.HEIGHT - 200 * Screen.ScreenSize);
            rate = 0;
            finishBack01 = Screen.WIDTH / 2f;
            finishBack02 = Screen.WIDTH / 2f;
            finishBack03 = Screen.WIDTH / 2f;

            TitleFinishFlag = false;
            startFlag = false;
            endingTime = 0.0f;
            finishingFlag = false;
            startBack01 = 0;
            startBack02 = 0;

            rankingON = false;
        }

        public void Update()
        {
            if (!startFlag)
            {
                if (rankingON)
                {
                    rankingScreen.Update();
                    if (rankingScreen.IsDead)
                    {
                        rankingON = false;
                    }
                }
                else
                {
                    // ゲージが満タンになった
                    if (destSizeY >= 1080)
                    {
                        startFlag = true;
                    }

                    if (Input.GetKeyDown(Keys.Space))
                    {
                        destSizeY += 300;
                    }

                    destSizeY -= 10 * TimeSpeed.Time;

                    destSizeY = MathHelper.Clamp(destSizeY, 0, 1080);

                    keyInput.Update();

                    if (Input.GetKeyDown(Keys.Enter))
                    {
                        rankingScreen.InitializeTitle();
                        rankingON = true;
                    }
                }
            }
            else
            {
                endingTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                if (endingTime > 2f)
                {
                    if (endingTime > 3f)
                    {
                        // 背景がどきはじめる
                        finishingFlag = true;

                        finishBack01 = MathHelper.Lerp(finishBack01, 0, 0.1f);
                        if (endingTime > 3.25f)
                        {
                            finishBack02 = MathHelper.Lerp(finishBack02, 0, 0.11f);
                            if (endingTime > 3.5f)
                            {
                                finishBack03 = MathHelper.Lerp(finishBack03, 0, 0.12f);

                                if (endingTime > 5)
                                {
                                    TitleFinishFlag = true;
                                }
                            }
                        }
                    }

                    // 文字が画面からどく
                    gameStartTextPos = Easing2D.BackIn(endingTime - 2, 0.5f, new Vector2(Screen.WIDTH / 2f, 150), new Vector2(Screen.WIDTH / 2f, -100), 2f);
                    inputKeyPos = Easing2D.BackIn(endingTime - 2, 0.5f, new Vector2(Screen.WIDTH / 2f, Screen.HEIGHT - 200 * Screen.ScreenSize), new Vector2(Screen.WIDTH / 2f, 1200), 2f);
                }
                else
                {
                    // 文字が表示される
                    destSizeY = 1080;
                    gameStartTextPos = Vector2.Lerp(gameStartTextPos, new Vector2(Screen.WIDTH / 2f, 150), 0.05f);
                    rate = MathHelper.Lerp(rate, 1, 0.05f);
                }
            }

            startBack01 = MathHelper.Lerp(startBack01, destSizeY, 0.1f);
            startBack02 = MathHelper.Lerp(startBack02, destSizeY, 0.2f);
        }

        public void Draw(Renderer renderer)
        {
            // タイトル
            SpriteFont font;
            string text;
            Vector2 size;

            if (!finishingFlag)
            {
                renderer.Draw2D("FlyedChickenTitle", new Vector2(Screen.WIDTH / 2f, 200 * Screen.ScreenSize), Color.White, 0, new Vector2(250, 125.5f), Vector2.One * Screen.ScreenSize * 1.5f);

                font = Fonts.Font10_128;
                text = "PlayerName";
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(100 * Screen.ScreenSize, Screen.HEIGHT / 2f -20*Screen.ScreenSize), Color.White, 0,Vector2.Zero, Vector2.One * Screen.ScreenSize);
            }

            // 黒幕
            if (finishingFlag)
            {
                Color color;

                color = new Color(150, 150, 150);
                renderer.Draw2D("Pixel", new Vector2(0, Screen.HEIGHT / 2f), color, 0, new Vector2(0, 0.5f), new Vector2(finishBack03 * Screen.ScreenSize, Screen.HEIGHT));
                renderer.Draw2D("Pixel", new Vector2(Screen.WIDTH, Screen.HEIGHT / 2f), color, 0, new Vector2(1, 0.5f), new Vector2(finishBack03 * Screen.ScreenSize, Screen.HEIGHT));

                color = new Color(100, 100, 100);
                renderer.Draw2D("Pixel", new Vector2(0, Screen.HEIGHT / 2f), color, 0, new Vector2(0, 0.5f), new Vector2(finishBack02 * Screen.ScreenSize, Screen.HEIGHT));
                renderer.Draw2D("Pixel", new Vector2(Screen.WIDTH, Screen.HEIGHT / 2f), color, 0, new Vector2(1, 0.5f), new Vector2(finishBack02 * Screen.ScreenSize, Screen.HEIGHT));

                color = new Color(50, 50, 50);
                renderer.Draw2D("Pixel", new Vector2(0, Screen.HEIGHT / 2f), color, 0, new Vector2(0, 0.5f), new Vector2(finishBack01 * Screen.ScreenSize, Screen.HEIGHT));
                renderer.Draw2D("Pixel", new Vector2(Screen.WIDTH, Screen.HEIGHT / 2f), color, 0, new Vector2(1, 0.5f), new Vector2(finishBack01 * Screen.ScreenSize, Screen.HEIGHT));
            }
            else
            {
                renderer.Draw2D("Pixel", new Vector2(Screen.WIDTH / 2f, Screen.HEIGHT), new Color(210, 210, 75), 0, new Vector2(0.5f, 1), new Vector2(Screen.WIDTH, startBack02 * Screen.ScreenSize));
                renderer.Draw2D("Pixel", new Vector2(Screen.WIDTH / 2f, Screen.HEIGHT), new Color(50, 50, 50), 0, new Vector2(0.5f, 1), new Vector2(Screen.WIDTH, startBack01 * Screen.ScreenSize));
            }

            // ゲームスタート
            font = Fonts.Font10_128;
            text = "GameStart";
            size = font.MeasureString(text);
            renderer.DrawString(font, text, gameStartTextPos, Color.White * rate, 0, size / 2f, Vector2.One * Screen.ScreenSize);

            keyInput.Draw(renderer, rate, inputKeyPos);

            if (rankingON)
            {
                rankingScreen.Draw(renderer);
            }
        }
    }
}
