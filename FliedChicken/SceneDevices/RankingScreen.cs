using System;
using System.Collections.Generic;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
using FliedChicken.Objects;
using FliedChicken.Particle;
using FliedChicken.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FliedChicken.GameObjects.Objects;

namespace FliedChicken.SceneDevices
{
    /// <summary>
    /// ランキング管理かつ表示クラス
    /// </summary>
    class RankingScreen
    {
        Camera camera;
        string[] rankPlayer;
        int[] rankScore;

        readonly int rankNum = 3;

        public RankingScreen(Camera camera)
        {
            this.camera = camera;
            rankPlayer = new string[rankNum];
            rankScore = new int[rankNum];
        }

        /// <summary>
        /// テキストファイルからランキングを読み込むメソッド
        /// </summary>
        public void RankingRead()
        {
            FileCheck();

            StreamReader sr = new StreamReader("ranking.txt");

            //string line = sr.ReadLine();
            //string[] value = line.Split(',');
            //for (int i = 0; i < rankNum; i++)
            //{
            //    rankPlayer[i] = value[i];
            //    rankScore[i] = int.Parse(value[i + rankNum]);
            //}

            for (int i = 0; i < rankNum; i++)
            {
                string line = sr.ReadLine();
                string[] value = line.Split(',');

                rankPlayer[i] = value[0];
                rankScore[i] = int.Parse(value[1]);
            }
            
            sr.Close();
        }

        /// <summary>
        /// テキストファイルにランキングを書き込むメソッド
        /// </summary>
        public void RankingWrite()
        {

            //string text = "";
            //for (int i = 0; i < rankNum; i++)
            //{
            //    text += rankPlayer[i];
            //    text += ",";
            //}

            //for (int i = 0; i < rankNum; i++)
            //{
            //    text += rankScore[i];
            //    if (i != rankNum)
            //    {
            //        text += ",";
            //    }
            //}

            Encoding enc = Encoding.GetEncoding("Shift_JIS");
            StreamWriter sw = new StreamWriter("ranking.txt", false, enc);

            for (int i = 0; i < rankNum; i++)
            {
                string text = "";
                text += rankPlayer[i];
                text += ',';
                text += rankScore[i].ToString();
                sw.WriteLine(text);
            }

            //sw.WriteLine(text);
            sw.Close();
        }

        /// <summary>
        /// リザルト結果からランキングを置き換えるメソッド
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="score"></param>
        public void RankingChange(string playerName, int score)
        {
            for (int i = rankNum - 1; i >= 0; i--)
            {
                if (rankScore[i] >= score)
                {
                    break;
                }
                else
                {
                    if (i != rankNum - 1)
                    {
                        rankScore[i + 1] = rankScore[i];
                        rankPlayer[i + 1] = rankPlayer[i];
                    }

                    rankScore[i] = score;
                    rankPlayer[i] = playerName;
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
            renderer.Draw2D("Pixel", new Vector2(camera.Position.X - 600, camera.Position.Y - Screen.HEIGHT / 2), 
                Color.White, 0.0f, new Vector2(1200, Screen.HEIGHT));

            renderer.DrawString(Fonts.Font32, "RANKING",
                new Vector2(camera.Position.X - 220, camera.Position.Y - Screen.HEIGHT / 2), Color.Red,
                0.0f, Vector2.Zero, new Vector2(3, 3));

            for (int i = 0; i < rankNum; i++)
            {
                renderer.DrawString(Fonts.Font32, rankPlayer[i],
                    new Vector2(camera.Position.X - 500, camera.Position.Y - 300 + 300 * i), Color.Red,
                    0.0f, Vector2.Zero, new Vector2(3, 3));
            }

            for (int i = 0; i < rankNum; i++)
            {
                renderer.DrawString(Fonts.Font32, rankScore[i].ToString(),
                    new Vector2(camera.Position.X + 200, camera.Position.Y - 300 + 300 * i), Color.Red,
                    0.0f, Vector2.Zero, new Vector2(3, 3));
            }
        }
    }
}
