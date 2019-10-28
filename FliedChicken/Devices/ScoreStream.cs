using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.Devices
{
    class ScoreStream
    {
        private static ScoreStream instance;

        static readonly string filePath = "./ScoreData";
        static readonly string fileName = "Score.txt";

        private ScoreStream()
        {

        }

        // シングルトン
        public static ScoreStream Instance()
        {
            if (instance == null)
            {
                instance = new ScoreStream();
            }

            return instance;
        }

        private void GenerateCheck()
        {
            // ない場合、フォルダ作成
            if (!Directory.Exists(filePath))
            {
                // フォルダ作成
                Directory.CreateDirectory(filePath);
            }

            // ファイルがない場合、生成
            if (!File.Exists(filePath + "/" + fileName))
            {
                // テキストファイル作成
                FileStream file = File.Create(filePath + "/" + fileName);
                file.Close();
            }
        }

        public void AddScore(string name, float distance)
        {
            GenerateCheck();

            using (StreamWriter sw = new StreamWriter(filePath + "/" + fileName, true))
            {
                // 名前(string) | 距離(float) , 名前(string) | 距離(float) ,
                // 名前(string) | 距離(float) , 名前(string) | 距離(float) ,
                // 名前(string) | 距離(float) , 名前(string) | 距離(float) ,
                // のようになる。
                sw.WriteLine(name + "|" + distance + ",");
            }
        }

        public Dictionary<string, float> GetScoreDictionary()
        {
            GenerateCheck();

            Dictionary<string, float> scoreDictionary = new Dictionary<string, float>();

            using (StreamReader sr = new StreamReader(filePath + "/" + fileName))
            {
                while (!sr.EndOfStream)
                {
                    string[] array = sr.ReadLine().Split(',');

                    for (int i = 0; i < array.Length; i++)
                    {
                        // 名前と距離のデータが入っている
                        string[] data = array[i].Split('|');

                        // データの長さが2個じゃなかったらスキップ
                        if (data.Length != 2) { continue; }
                        if (data[0] == "") { continue; }

                        // data[1]は距離に当たるので、floatに変換して成功したら次
                        if (float.TryParse(data[1], out float num))
                        {
                            // すでに同じ名前のやつがいたら、大きいほうを選択して代入
                            if (scoreDictionary.ContainsKey(data[0]))
                            {
                                scoreDictionary[data[0]] = MathHelper.Max(scoreDictionary[data[0]], num);
                            }
                            // 名前が被っていなかったらそのまま追加
                            else
                            {
                                scoreDictionary.Add(data[0], num);
                            }
                        }
                    }
                }
            }

            return scoreDictionary;
        }

        // 名前入力が空白だった場合、被らない名前を生成する
        public string NewPlayerName()
        {
            var dictionary = GetScoreDictionary();

            string newPlayerName;

            int index = 1;

            while (true)
            {
                newPlayerName = "Player" + index.ToString("000");

                if (!dictionary.ContainsKey(newPlayerName))
                {
                    break;
                }

                index++;
            }

            return newPlayerName;
        }
    }
}
