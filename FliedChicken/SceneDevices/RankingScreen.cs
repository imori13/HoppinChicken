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
            FINISH,
        }

        Camera camera;
        List<string> rankPlayer;
        List<int> rankScore;

        private readonly string path = "ranking.txt";

        private float windowWidth;
        private float windowHeight;

        private readonly float maxWindowWidth = Screen.WIDTH - 300;
        private readonly float maxWindowHeight = Screen.HEIGHT - 100;

        readonly int rankNum = 3;

        ScreenState state;

        float startTime;
        float finishTime;

        public bool IsDead { get; private set; }

        public RankingScreen(Camera camera)
        {
            this.camera = camera;
        }

        public void Initialize()
        {
            rankPlayer = new List<string>();
            rankScore = new List<int>();

            RankingRead();

            windowWidth = 0.0f;
            windowHeight = maxWindowHeight;

            state = ScreenState.START;

            startTime = 0.0f;
            finishTime = 0.0f;

            IsDead = false;
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
            if (Input.GetKeyDown(Keys.Enter))
            {
                state = ScreenState.FINISH;
            }
        }

        private void Finish()
        {
            finishTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
            windowWidth = MathHelper.Lerp(windowWidth, 0, 0.05f);
            if (finishTime >= 1.5f)
            {
                windowWidth = 0;
                IsDead = true;
            }
        }

        public void End()
        {
            RankingWrite();
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

            for (int i = rankPlayer.Count - 1; i >= 0; i--)
            {
                if (rankScore[i] >= score)
                {
                    rankPlayer.Insert(i + 1, playerName);
                    rankScore.Insert(i + 1, score);
                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        rankPlayer.Insert(0, playerName);
                        rankScore.Insert(0, score);
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
                Color.Black, 0.0f, new Vector2(windowWidth, windowHeight));

            renderer.DrawString(Fonts.Font12_32, "RANKING",
                Screen.Vec2 / 2 - new Vector2(0, 400), Color.White * 1,
                0.0f, Fonts.Font12_32.MeasureString("RESULT") / 2, new Vector2(2, 2));

            for (int i = 0; i < rankNum; i++)
            {
                renderer.DrawString(Fonts.Font12_32, (i + 1).ToString() + '.',
                    Screen.Vec2 / 2 - new Vector2(580, 300 - 200 * i), Color.White,
                    0.0f, Vector2.Zero, new Vector2(2, 2));
            }

            for (int i = 0; i < rankNum; i++)
            {
                renderer.DrawString(Fonts.Font12_32, rankPlayer[i],
                    Screen.Vec2 / 2 - new Vector2(500, 300 - 200 * i), Color.White,
                    0.0f, Vector2.Zero, new Vector2(2, 2));
            }

            for (int i = 0; i < rankNum; i++)
            {
                renderer.DrawString(Fonts.Font12_32, rankScore[i].ToString(),
                    Screen.Vec2 / 2 - new Vector2(-200, 300 - 200 * i), Color.White,
                    0.0f, Vector2.Zero, new Vector2(2, 2));
            }
        }
    }
}
