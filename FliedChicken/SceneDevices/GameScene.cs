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
using FliedChicken.GameObjects.Enemys;
using FliedChicken.GameObjects.PlayerDevices;

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
        //スコア保存用
        int score;

        //ゲームプレイシーンの状態
        GamePlayState state;
        ResultScreen resultScreen;
        RankingScreen rankingScreen;
        TitleDisplayMode titleDisplayMode;

        //コイン関連
        CoinManager coinManager;

        EnemySpawner enemySpawner;

        public GameScene()
        {
            camera = new Camera();
            objectsManager = new ObjectsManager(camera);
            titleDisplayMode = new TitleDisplayMode();

            resultScreen = new ResultScreen();
            rankingScreen = new RankingScreen();

            coinManager = new CoinManager(objectsManager, 0.5f);
        }

        public override void Initialize()
        {
            camera.Initialize();
            objectsManager.Initialize();

            player = new Player(camera);
            objectsManager.AddGameObject(player);
            camera.Position = player.Position;

            state = GamePlayState.TITLE;

            titleDisplayMode.Initialize();

            enemySpawner = new EnemySpawner(player, camera, objectsManager, 64 * -3, 64 * 10, 2, 4);
            enemySpawner.Initialize();

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
            renderer.Begin();

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

#if DEBUG
            enemySpawner.DebugDraw(renderer);
#endif
            objectsManager.Draw(renderer);

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
                
                // TODO : ここでマップを生成する
            }
        }

        private void BeforeFly()
        {

        }

        private void Fly()
        {
            if (player.IsDead == true)
            {
                player.state = PlayerState.CLEAR;
                state = GamePlayState.CLEAR;
            }

            enemySpawner.Update();
        }
        
        private void Clear()
        {
            if (Input.GetKeyDown(Keys.A))
            {
                state = GamePlayState.RESULT;
                resultScreen.Initialize(score);
            }
        }

        private void Result()
        {
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
            //rankingScreen.RankingWrite();
            return SceneEnum.GameScene;
        }
    }
}
