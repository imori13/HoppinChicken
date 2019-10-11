using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
using FliedChicken.Objects;
using FliedChicken.Particle;
using FliedChicken.ScenesDevice;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FliedChicken.SceneDevices
{
    enum GamePlayState
    {
        STOP,
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
        //ゲームクリアライン
        int gameclearLine;
        //タイム保存用
        float cleartime;

        GamePlayState state;

        ResultScreen resultScreen;
        EnemyLaneManager laneManager;

        public GameScene()
        {
            camera = new Camera();
            objectsManager = new ObjectsManager(camera);
        }

        public override void Initialize()
        {
            camera.Initialize();
            objectsManager.Initialize();

            player = new Player(camera);
            objectsManager.AddGameObject(player);

            cleartime = 0.0f;
            gameclearLine = 500;
            state = GamePlayState.STOP;

            resultScreen = new ResultScreen();
            laneManager = new EnemyLaneManager(camera, 10);
            laneManager.ObjectsManager = objectsManager;
            objectsManager.AddGameObject(laneManager);

            base.Initialize();
        }

        public override void Update()
        {
            camera.Position += new Vector2(0, 8) * TimeSpeed.Time;

            if (Input.GetKeyDown(Keys.Space))
            {
                Random rand = GameDevice.Instance().Random;

                for (int i = 0; i < 50; i++)
                {
                    objectsManager.AddParticle(
                        new RadiationParticle2D(Vector2.Zero, Color.Red, MyMath.RandomCircleVec2(), rand));
                }
            }

            Default();

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
            }

            base.Update();
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
            ShutDown = true;
        }

        private void Clear()
        {
            if (Input.GetKeyDown(Keys.A))
            {
                state = GamePlayState.RESULT;
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Begin(camera);

            renderer.Draw2D("4k-gaming-wallpaper", Vector2.Zero, Color.White);
            objectsManager.Draw(renderer);

            if (state == GamePlayState.RESULT)
            {

            }

            if (state == GamePlayState.RANKING)
            {

            }

            renderer.End();

            base.Draw(renderer);
        }

        public override SceneEnum NextScene()
        {
            return SceneEnum.TitleScene;
        }
    }
}
