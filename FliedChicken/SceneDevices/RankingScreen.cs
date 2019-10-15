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
        Camera camera;
        List<string> rankPlayer;
        List<int> rankScore;

        readonly int rankNum = 3;

        public RankingScreen(Camera camera)
        {
            this.camera = camera;
            rankPlayer = new List<string>();
            rankScore = new List<int>();
        }

        /// <summary>
        /// テキストファイルからランキングを読み込むメソッド
        /// </summary>
        public void RankingRead()
        {
            FileCheck();

            StreamReader sr = new StreamReader("ranking.txt");

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
            StreamWriter sw = new StreamWriter("ranking.txt", false, enc);

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
            if (!File.Exists("ranking.txt"))
            {
                StreamWriter sw = File.CreateText("ranking.txt");

                for (int i = 0; i < rankNum; i++)
                {
                    sw.WriteLine("----,00000");
                }

                sw.Close();
            }
        }

        public void Draw(Renderer renderer)
        {
            renderer.Draw2D("Pixel", new Vector2(camera.Position.X, camera.Position.Y), 
                Color.White, 0.0f, new Vector2(1200, Screen.HEIGHT));

            renderer.DrawString(Fonts.Font12_32, "RANKING",
                new Vector2(camera.Position.X - 220, camera.Position.Y - Screen.HEIGHT / 2), Color.Red,
                0.0f, Vector2.Zero, new Vector2(3, 3));

            for (int i = 0; i < rankNum; i++)
            {
                renderer.DrawString(Fonts.Font12_32, rankPlayer[i],
                    new Vector2(camera.Position.X - 500, camera.Position.Y - 300 + 300 * i), Color.Red,
                    0.0f, Vector2.Zero, new Vector2(3, 3));
            }

            for (int i = 0; i < rankNum; i++)
            {
                renderer.DrawString(Fonts.Font12_32, rankScore[i].ToString(),
                    new Vector2(camera.Position.X + 200, camera.Position.Y - 300 + 300 * i), Color.Red,
                    0.0f, Vector2.Zero, new Vector2(3, 3));
            }
        }
    }
}
