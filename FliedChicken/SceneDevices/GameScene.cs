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
using FliedChicken.GameObjects.Objects;

namespace FliedChicken.SceneDevices
{
    /// <summary>
    /// ゲームシーンの状態
    /// </summary>
    enum GamePlayState
    {
        STOP,
        FLY,
        RESTART,
        CLEAR,
        RESULT,
        RANKING,
    }

    //ゲームシーン管理クラス
    class GameScene : IScene
    {
        Camera camera;
        ObjectsManager objectsManager;
        SceneManager sceneManager;
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

        public GameScene(SceneManager sceneManager)
        {
            camera = new Camera();
            objectsManager = new ObjectsManager(camera);
            this.sceneManager = sceneManager;
        }

        public void Initialize()
        {
            camera.Initialize();
            objectsManager.Initialize();
            
            player = new Player(camera);
            objectsManager.AddGameObject(player);

            cleartime = 0.0f;
            gameclearLine = 500;
            state = GamePlayState.STOP;

            resultScreen = new ResultScreen(camera);
            rankingScreen = new RankingScreen(camera);
        }

        public void Update()
        {
            //if (Input.GetKeyDown(Keys.Space))
            //{
            //    Random rand = GameDevice.Instance().Random;

            //    for (int i = 0; i < 50; i++)
            //    {
            //        objectsManager.AddParticle(
            //            new RadiationParticle2D(Vector2.Zero, Color.Red, MyMath.RandomCircleVec2(), rand));
            //    }
            //}

            //どの状態でもUpdateで動くメソッド
            Default();

            //状態によって動くUpdateメソッド
            switch (state)
            {
                case GamePlayState.STOP:
                    Stop();
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
        }

        private void Default()
        {
            camera.Update();
            objectsManager.Update();
        }

        private void Stop()
        {
            if (Input.GetKeyDown(Keys.A))
            {
                state = GamePlayState.FLY;
            }
        }

        private void Fly()
        {
            cleartime += TimeSpeed.Time;

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
            sceneManager.ChangeScene(SceneEnum.GameScene);
        }

        private void Clear()
        {
            if (Input.GetKeyDown(Keys.A))
            {
                resultScreen.SetScore(cleartime, 0);
                state = GamePlayState.RESULT;
            }
        }

        private void Result()
        {
            if (Input.GetKeyDown(Keys.A))
            {
                rankingScreen.RankingRead();
                rankingScreen.RankingChange("souya", 11000);
                state = GamePlayState.RANKING;
            }
        }

        private void Ranking()
        {
            if (Input.GetKeyDown(Keys.A))
            {
                sceneManager.ChangeScene(SceneEnum.TitleScene);
            }
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin(camera);

            renderer.Draw2D("4k-gaming-wallpaper", Vector2.Zero, Color.White);
            objectsManager.Draw(renderer);

            if (state == GamePlayState.RESULT)
            {
                ResultScreen(renderer);
            }

            if (state == GamePlayState.RANKING)
            {
                RankingScreen(renderer);
            }

            renderer.End();
        }

        private void ResultScreen(Renderer renderer)
        {
            resultScreen.Draw(renderer);
        }

        private void RankingScreen(Renderer renderer)
        {
            rankingScreen.Draw(renderer);
        }



        public void ShutDown()
        {
            rankingScreen.RankingWrite();
        }
    }
}
