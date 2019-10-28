using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
using FliedChicken.ScenesDevice;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.SceneDevices.Title;
using FliedChicken.GameObjects;
using FliedChicken.GameObjects.Enemys;
using FliedChicken.GameObjects.PlayerDevices;
using FliedChicken.GameObjects.Clouds;
using FliedChicken.GameObjects.Particle;
using FliedChicken.UI;

namespace FliedChicken.SceneDevices
{
    /// <summary>
    /// ゲームシーンの状態
    /// </summary>
    enum GamePlayState
    {
        TITLE,
        BEFOREFLY,
        FLY,
        CLEAR,
        RESULT,
        RANKING,
    }

    class GameScene : SceneBase
    {
        Camera camera;
        ObjectsManager objectsManager;
        //プレイヤー保存用
        Player player;
        //ダイブエネミー保存用
        DiveEnemy diveEnemy;
        //スコア保存用
        int score;

        //ゲームプレイシーンの状態
        public GamePlayState State { get; private set; }
        ResultScreen resultScreen;
        RankingScreen rankingScreen;
        DiveEnemyUI dEnemyUI;
        public TitleDisplayMode TitleDisplayMode { get; private set; }
        public BeforeFlyScreen BeforeFlyScreen { get; private set; }
        //コイン関連
        CoinManager coinManager;

        EnemySpawner enemySpawner;

        // 雲
        CloudManager cloudManager;

        float time = 0;

        public GameScene()
        {
            camera = new Camera();
            objectsManager = new ObjectsManager(camera, this);
            TitleDisplayMode = new TitleDisplayMode();

            resultScreen = new ResultScreen();
            rankingScreen = new RankingScreen(TitleDisplayMode);
            BeforeFlyScreen = new BeforeFlyScreen();

            coinManager = new CoinManager(objectsManager, 0.5f);
            cloudManager = new CloudManager(objectsManager);
        }

        public override void Initialize()
        {
            camera.Initialize();
            objectsManager.Initialize();

            player = new Player(camera);
            objectsManager.AddGameObject(player);
            camera.Position = player.Position;

            State = GamePlayState.TITLE;

            TitleDisplayMode.Initialize();

            enemySpawner = new EnemySpawner(player, camera, objectsManager, 64 * 10, 64 * 14, 0, 0);

            cloudManager.Initialize();

            time = 0;

            base.Initialize();
        }

        public override void Update()
        {
            Default();

            //状態によって動くUpdateメソッド
            switch (State)
            {
                case GamePlayState.TITLE:
                    Title();
                    break;
                case GamePlayState.BEFOREFLY:
                    BeforeFly();
                    break;
                case GamePlayState.FLY:
                    Fly();
                    break;
                case GamePlayState.CLEAR:
                    Clear();
                    break;
                case GamePlayState.RESULT:
                    Result();
                    break;
                case GamePlayState.RANKING:
                    Ranking();
                    break;
            }

            base.Update();
        }

        public override void Draw(Renderer renderer)
        {
            // ---------------------------------------------
            renderer.BeginCloud(camera);
            cloudManager.BackDraw(renderer);
            renderer.End();

            // ---------------------------------------------
            // タイトル画面を描画
            if (State == GamePlayState.TITLE)
            {
                renderer.Begin();
                TitleDisplayMode.Draw(renderer);
                renderer.End();
            }
            // ---------------------------------------------

            // タイトル画面の黒幕よりもプレイヤーを上に描画させたいのでこの描画順
            // オブジェクトを描画
            renderer.Begin(camera);
#if DEBUG
            //enemySpawner.DebugDraw(renderer);
#endif
            objectsManager.Draw(renderer);

            renderer.End();
            // ---------------------------------------------

            renderer.BeginCloud(camera);
            cloudManager.FrontDraw(renderer);
            renderer.End();

            // ---------------------------------------------

            renderer.Begin();

            if (State == GamePlayState.BEFOREFLY)
            {
                BeforeFlyScreen.Draw(renderer);
            }

            if (State == GamePlayState.RESULT)
            {
                ResultScreen(renderer);
            }

            if (State == GamePlayState.RANKING)
            {
                RankingScreen(renderer);
            }

            renderer.End();

            // ---------------------------------------------
            renderer.Begin();

            // 進んだ距離を表示
            if (State == GamePlayState.FLY)
            {
                SpriteFont font = Fonts.Font10_128;
                string text = player.SumDistance.ToString("0.0M");
                Vector2 size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(Screen.WIDTH / 2f, 200 * Screen.ScreenSize), Color.Black, 0, size / 2f, Vector2.One * Screen.ScreenSize);
                dEnemyUI.Draw(renderer);
            }

            if (State == GamePlayState.RESULT)
            {
                ResultScreen(renderer);
            }

            if (State == GamePlayState.RANKING)
            {
                RankingScreen(renderer);
            }
            renderer.End();
            // ---------------------------------------------
            base.Draw(renderer);
        }

        private void Default()
        {
            camera.Update();
            objectsManager.Update();
            cloudManager.Update();
        }

        private void Title()
        {
            TitleDisplayMode.Update();
            if (TitleDisplayMode.TitleFinishFlag)
            {
                diveEnemy = new DiveEnemy(camera, player);
                diveEnemy.Position = player.Position - new Vector2(0, 800);
                objectsManager.AddGameObject(diveEnemy);
                BeforeFlyScreen.Initialize(player, diveEnemy, cloudManager, camera);
                State = GamePlayState.BEFOREFLY;
            }
        }

        private void BeforeFly()
        {
            BeforeFlyScreen.Update();
            if (BeforeFlyScreen.IsDead)
            {
                BeforeFlyScreen.End();
                enemySpawner.Initialize();
                player.StartPositionY = player.Position.Y;
                player.PlayerGameStartFlag = true;
                dEnemyUI = new DiveEnemyUI(camera, diveEnemy, player);
                dEnemyUI.Initialize();
                State = GamePlayState.FLY;

                string[] bgms = new string[]
                {
                    "bgm_maoudamashii_8bit08",
                    "bgm_maoudamashii_8bit09",
                    "bgm_maoudamashii_8bit10",
                    "bgm_maoudamashii_8bit11",
                    "bgm_maoudamashii_8bit27",
                    "bgm_maoudamashii_8bit28",
                };

                GameDevice.Instance().Sound.PlayBGM(bgms[GameDevice.Instance().Random.Next(0, bgms.Length)]);
            }
        }

        private void Fly()
        {
            dEnemyUI.Update();
            if (player.HitFlag == true)
            {
                time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
                float limit = 1f;

                GameDevice.Instance().Sound.StopBGM();

                if (time >= limit)
                {
                    resultScreen.Initialize(player.SumDistance);
                    State = GamePlayState.RESULT;
                }
            }

            enemySpawner.Update();
        }

        private void Clear()
        {
            if (Input.GetKeyDown(Keys.Space))
            {
                State = GamePlayState.RESULT;
                resultScreen.Initialize(score);
            }
        }

        private void Result()
        {
            resultScreen.Update();

            if (resultScreen.IsDead)
            {
                resultScreen.End();
                rankingScreen.Initialize(resultScreen.GetWindowAlpha());
                State = GamePlayState.RANKING;
            }
        }

        private void Ranking()
        {
            rankingScreen.Update();
            if (rankingScreen.IsDead)
            {
                rankingScreen.End();
                ShutDown = true;
            }
        }

        private void ResultScreen(Renderer renderer)
        {
            resultScreen.Draw(renderer);
        }

        private void RankingScreen(Renderer renderer)
        {
            rankingScreen.Draw(renderer);
        }

        public override SceneEnum NextScene()
        {
            return SceneEnum.GameScene;
        }
    }
}
