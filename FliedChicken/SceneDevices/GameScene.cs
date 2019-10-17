using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
using FliedChicken.Particle;
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

namespace FliedChicken.SceneDevices
{
    /// <summary>
    /// ゲームシーンの状態
    /// </summary>
    enum GamePlayState
    {
        TITLE,
        FLY,
        RESTART,
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
        //ゲームクリアライン(一時的処置)
        int gameclearLine;
        //タイム保存用
        float cleartime;

        //プレイヤーネーム保存用
        string playerName;
        //スコア保存用
        int score;

        //ゲームプレイシーンの状態
        GamePlayState state;
        ResultScreen resultScreen;
        RankingScreen rankingScreen;
        TitleDisplayMode titleDisplayMode;

        //敵関連
        EnemyLaneManager laneManager;
        DiveEnemySpawner diveEnemySpawner;
        float centerX;

        //コイン関連
        CoinManager coinManager;

        public GameScene()
        {
            camera = new Camera();
            objectsManager = new ObjectsManager(camera);
            titleDisplayMode = new TitleDisplayMode();

            resultScreen = new ResultScreen(camera);
            rankingScreen = new RankingScreen(camera);

            coinManager = new CoinManager(objectsManager, 0.5f);
        }

        public override void Initialize()
        {
            camera.Initialize();
            objectsManager.Initialize();

            player = new Player(camera);
            objectsManager.AddGameObject(player);
            camera.Position = player.Position;

            cleartime = 0.0f;
            gameclearLine = 500;
            state = GamePlayState.TITLE;

            titleDisplayMode.Initialize();

            diveEnemySpawner = new DiveEnemySpawner(3.0f, objectsManager, player, camera);

            EnemyFactory.Initialize();

            base.Initialize();
        }

        public override void Update()
        {
            Default();

            //状態によって動くUpdateメソッド
            switch (state)
            {
                case GamePlayState.TITLE:
                    Title();
                    break;
                case GamePlayState.FLY:
                    Fly();
                    break;
                case GamePlayState.RESTART:
                    Restart();
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
            renderer.Begin(camera);

            if (state == GamePlayState.RESULT)
            {
                ResultScreen(renderer);
            }

            if (state == GamePlayState.RANKING)
            {
                RankingScreen(renderer);
            }

            renderer.End();

            // タイトル画面を描画
            if (state == GamePlayState.TITLE)
            {
                renderer.Begin();
                titleDisplayMode.Draw(renderer);
                renderer.End();
            }

            // タイトル画面の黒幕よりもプレイヤーを上に描画させたいのでこの描画順
            // オブジェクトを描画
            renderer.Begin(camera);


            objectsManager.Draw(renderer);

#if DEBUG
            // デバッグ用
            if (titleDisplayMode.TitleFinishFlag)
            {
                renderer.Draw2D("Pixel", new Vector2(player.Position.X, gameclearLine), Color.Red, 0, Vector2.One / 2f, new Vector2(2000, 10));
            }
#endif

            renderer.End();

#if DEBUG
            // デバッグ用描画 現在のGamePlayStateの状態を表示
            renderer.Begin();
            SpriteFont font = Fonts.Font12_32;
            string text = state.ToString();
            Vector2 size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(Screen.WIDTH / 2f, 100 * Screen.ScreenSize), Color.White * 0.5f, 0, size / 2f, Vector2.One * Screen.ScreenSize);
            renderer.End();
#endif

            base.Draw(renderer);
        }

        private void Default()
        {
            camera.Update();
            objectsManager.Update();
        }

        private void Title()
        {
            titleDisplayMode.Update();
            if (titleDisplayMode.TitleFinishFlag)
            {
                state = GamePlayState.FLY;

                // ゴールラインを設定
                gameclearLine = (int)player.Position.Y + Screen.WIDTH * 5;
                if (centerX == 0)
                    centerX = player.Position.X;

                // TODO : ここでマップを生成する
            }
        }

        private void Fly()
        {
            if (laneManager == null)
            {
                laneManager = new EnemyLaneManager(camera, 20, 10, coinManager);
                laneManager.ObjectsManager = objectsManager;
                laneManager.Position = new Vector2(centerX, player.Position.Y + Screen.HEIGHT);
                objectsManager.AddGameObject(laneManager);
            }

            diveEnemySpawner.Update();

            cleartime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

            if (player.IsDead == true)
            {
                state = GamePlayState.RESTART;
            }

            if (player.Position.Y >= gameclearLine)
            {
                player.state = PlayerState.CLEAR;
                state = GamePlayState.CLEAR;
            }
        }

        private void Restart()
        {
            ShutDown = true;
            if (laneManager != null)
            {
                laneManager.Destroy();
                laneManager = null;
                diveEnemySpawner.Shutdown();
                coinManager.ClearCoin();
            }
        }

        private void Clear()
        {
            if (Input.GetKeyDown(Keys.A))
            {
                state = GamePlayState.RESULT;
                resultScreen.Initialize(cleartime, 0);
            }
        }

        private void Result()
        {
            if (laneManager != null)
            {
                laneManager.Destroy();
                laneManager = null;
                diveEnemySpawner.Shutdown();
                coinManager.ClearCoin();
            }

            resultScreen.Update();

            if (resultScreen.IsDead)
            {
                resultScreen.End();
                rankingScreen.Initialize();
                rankingScreen.RankingChange(titleDisplayMode.keyInput.Text, 11000);
                state = GamePlayState.RANKING;
            }
        }

        private void Ranking()
        {
            rankingScreen.Update();
            if (Input.GetKeyDown(Keys.A))
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
            //rankingScreen.RankingWrite();
            return SceneEnum.GameScene;
        }
    }
}
