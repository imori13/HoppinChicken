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

        List<string> rankPlayer;
        List<int> rankScore;

        List<float> rankAlpha;
        private int alphaCount;

        private readonly string path = "ranking.txt";

        private readonly float maxWindowWidth = Screen.WIDTH;
        private readonly float maxWindowHeight = Screen.HEIGHT - 200;

        private Vector2 maxWindowSize;
        private float windowAlpha;

        readonly int rankNum = 10;

        ScreenState state;

        public bool IsDead { get; private set; }

        private Vector2 textPosition01;

        private float time;

        private string myName;
        private float myScore;
        private int myRank;

        public RankingScreen()
        {
        }

        public void Initialize(float alpha)
        {
            rankPlayer = new List<string>();
            rankScore = new List<int>();
            rankAlpha = new List<float>();

            for (int i = 0; i < rankNum; i++)
            {
                rankAlpha.Add(0.0f);
            }

            RankingRead();

            state = ScreenState.STATE01;

            IsDead = false;

            maxWindowSize = new Vector2(maxWindowWidth, maxWindowHeight);
            windowAlpha = alpha;

            textPosition01 = new Vector2(Screen.Vec2.X, Screen.Vec2.Y / 2);

            time = 0.0f;
        }

        public void InitializeTitle()
        {
            rankPlayer = new List<string>();
            rankScore = new List<int>();
            rankAlpha = new List<float>();

            for (int i = 0; i < rankNum; i++)
            {
                rankAlpha.Add(0.0f);
            }

            RankingRead();

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
            if (textPosition01.X <= 50)
            {
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

        /// <summary>
        /// テキストファイルからランキングを読み込むメソッド
        /// </summary>
        public void RankingRead()
        {
            FileCheck();

            StreamReader sr = new StreamReader(path);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] value = line.Split(',');

                rankPlayer.Add(value[0]);
                rankScore.Add(int.Parse(value[1]));
            }

            sr.Close();
        }

        /// <summary>
        /// テキストファイルにランキングを書き込むメソッド
        /// </summary>
        public void RankingWrite()
        {
            Encoding enc = Encoding.GetEncoding("Shift_JIS");
            StreamWriter sw = new StreamWriter(path, false, enc);

            for (int i = 0; i < rankPlayer.Count; i++)
            {
                string text = "";
                text += rankPlayer[i];
                text += ',';
                text += rankScore[i].ToString();
                sw.WriteLine(text);
            }

            sw.Close();
        }

        /// <summary>
        /// リザルト結果からランキングを置き換えるメソッド
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="score"></param>
        public void RankingChange(string playerName, int score)
        {
            if (playerName == "Player")
            {
                int num = 1;
                foreach (var name in rankPlayer)
                {
                    if (name.Contains("Player"))
                    {
                        num++;
                    }
                }

                playerName = "Player" + num.ToString();
            }

            myName = playerName;
            myScore = score;

            for (int i = rankPlayer.Count - 1; i >= 0; i--)
            {
                if (rankScore[i] >= score)
                {
                    rankPlayer.Insert(i + 1, playerName);
                    rankScore.Insert(i + 1, score);
                    myRank = i + 1;
                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        rankPlayer.Insert(0, playerName);
                        rankScore.Insert(0, score);
                        myRank = 0;
                    }
                }
            }
        }

        public void FileCheck()
        {
            if (!File.Exists(path))
            {
                StreamWriter sw = File.CreateText(path);

                for (int i = 0; i < rankNum; i++)
                {
                    sw.WriteLine("----,00000");
                }

                sw.Close();
            }
        }

        public void Draw(Renderer renderer)
        {
            renderer.Draw2D("Pixel", Screen.Vec2 / 2,
                Color.Black * windowAlpha, 0.0f, maxWindowSize);

            renderer.DrawString(Fonts.Font10_128, "RANKING", textPosition01 - new Vector2(0, 300), Color.White,
                0.0f, new Vector2(0, Fonts.Font10_128.MeasureString("RANKING").Y / 2), new Vector2(0.7f, 0.7f));

            for (int i = 0; i < 5; i++)
            {
                renderer.DrawString(Fonts.Font10_128, (i + 1).ToString() + ',',
                    textPosition01 + new Vector2(500,- 300 + (150 * i)), Color.White,
                    0.0f, new Vector2(0, Fonts.Font10_128.MeasureString((i + 1).ToString()).Y / 2), 
                    new Vector2(0.5f, 0.5f));
            }

            for (int i = 0; i < rankNum - 5; i++)
            {
                renderer.DrawString(Fonts.Font10_128, (i + 6).ToString() + ',',
                    textPosition01 + new Vector2(1100, - 300 + (150 * i)), Color.White,
                    0.0f, new Vector2(0, Fonts.Font10_128.MeasureString((i + 1).ToString()).Y / 2),
                    new Vector2(0.5f, 0.5f));
            }
            
            for (int i = 0; i < 5; i++)
            {
                renderer.DrawString(Fonts.Font10_128, rankPlayer[i],
                    textPosition01 + new Vector2(850, - 310 + (150 * i)), Color.White,
                    0.0f, Fonts.Font10_128.MeasureString(rankPlayer[i]), new Vector2(0.3f, 0.3f));

                renderer.DrawString(Fonts.Font10_128, rankScore[i] + "m",
                    textPosition01 + new Vector2(900, -280 + (150 * i)), Color.White,
                    0.0f, new Vector2(Fonts.Font10_128.MeasureString(rankScore[i] + "m").X,
                    Fonts.Font10_128.MeasureString(rankScore[i] + "m").Y / 2), new Vector2(0.3f, 0.3f));
            }

            for (int i = 0; i < 5; i++)
            {
                renderer.DrawString(Fonts.Font10_128, rankPlayer[i + 5],
                    textPosition01 + new Vector2(1450, -310 + (150 * i)), Color.White,
                    0.0f, Fonts.Font10_128.MeasureString(rankPlayer[i + 5]), new Vector2(0.3f, 0.3f));

                renderer.DrawString(Fonts.Font10_128, rankScore[i + 5] + "m",
                    textPosition01 + new Vector2(1500, -280 + (150 * i)), Color.White,
                    0.0f, new Vector2(Fonts.Font10_128.MeasureString(rankScore[i + 5] + "m").X,
                    Fonts.Font10_128.MeasureString(rankScore[i + 5] + "m").Y / 2), new Vector2(0.3f, 0.3f));
            }
        }

        public void DrawResult(Renderer renderer)
        {
            renderer.Draw2D("Pixel", Screen.Vec2 / 2,
                Color.Black * windowAlpha, 0.0f, maxWindowSize);

            renderer.DrawString(Fonts.Font10_128, "RANKING", textPosition01, Color.White,
                0.0f, new Vector2(0, Fonts.Font10_128.MeasureString("RANKING").Y / 2), new Vector2(0.7f, 0.7f));

            for (int i = 0; i < 5; i++)
            {
                renderer.DrawString(Fonts.Font10_128, (i + 1).ToString() + ',',
                    textPosition01 + new Vector2(500, -300 + (150 * i)), Color.White,
                    0.0f, new Vector2(0, Fonts.Font10_128.MeasureString((i + 1).ToString()).Y / 2),
                    new Vector2(0.5f, 0.5f));
            }

            for (int i = 0; i < rankNum - 5; i++)
            {
                renderer.DrawString(Fonts.Font10_128, (i + 6).ToString() + ',',
                    textPosition01 + new Vector2(1100, -300 + (150 * i)), Color.White,
                    0.0f, new Vector2(0, Fonts.Font10_128.MeasureString((i + 1).ToString()).Y / 2),
                    new Vector2(0.5f, 0.5f));
            }

            for (int i = 0; i < 5; i++)
            {
                renderer.DrawString(Fonts.Font10_128, rankPlayer[i],
                    textPosition01 + new Vector2(850, -310 + (150 * i)), Color.White,
                    0.0f, Fonts.Font10_128.MeasureString(rankPlayer[i]), new Vector2(0.3f, 0.3f));

                renderer.DrawString(Fonts.Font10_128, rankScore[i] + "m",
                    textPosition01 + new Vector2(900, -280 + (150 * i)), Color.White,
                    0.0f, new Vector2(Fonts.Font10_128.MeasureString(rankScore[i] + "m").X,
                    Fonts.Font10_128.MeasureString(rankScore[i] + "m").Y / 2), new Vector2(0.3f, 0.3f));
            }

            for (int i = 0; i < 5; i++)
            {
                renderer.DrawString(Fonts.Font10_128, rankPlayer[i + 5],
                    textPosition01 + new Vector2(1450, -310 + (150 * i)), Color.White,
                    0.0f, Fonts.Font10_128.MeasureString(rankPlayer[i + 5]), new Vector2(0.3f, 0.3f));

                renderer.DrawString(Fonts.Font10_128, rankScore[i + 5] + "m",
                    textPosition01 + new Vector2(1500, -280 + (150 * i)), Color.White,
                    0.0f, new Vector2(Fonts.Font10_128.MeasureString(rankScore[i + 5] + "m").X,
                    Fonts.Font10_128.MeasureString(rankScore[i + 5] + "m").Y / 2), new Vector2(0.3f, 0.3f));
            }
        }
    }
}
