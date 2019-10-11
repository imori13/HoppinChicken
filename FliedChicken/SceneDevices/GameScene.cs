using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
using FliedChicken.Objects;
using FliedChicken.Particle;
using FliedChicken.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FliedChicken.GameObjects.Objects;

namespace FliedChicken.SceneDevices
{
    class GameScene : IScene
    {
        Camera camera;
        ObjectsManager objectsManager;
        BackGround backGround;

        public GameScene()
        {
            camera = new Camera();
            objectsManager = new ObjectsManager(camera);
            backGround = new BackGround(camera);
        }

        public void Initialize()
        {
            camera.Initialize();
            objectsManager.Initialize();
            objectsManager.AddGameObject(new Player(camera));
            backGround.Initialize();
        }

        public void Update()
        {
            if (Input.GetKeyDown(Keys.Space))
            {
                Random rand = GameDevice.Instance().Random;

                for(int i = 0; i < 50; i++)
                {
                    objectsManager.AddParticle(
                        new RadiationParticle2D(Vector2.Zero, Color.Red, MyMath.RandomCircleVec2(), rand));
                }
            }

            camera.Update();
            objectsManager.Update();
            backGround.Update();
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin(camera);
            
            backGround.Draw(renderer);
            renderer.Draw2D("4k-gaming-wallpaper", Vector2.Zero, Color.White);
            renderer.Draw2D("packman", Vector2.Zero, Color.White);
            objectsManager.Draw(renderer);

            renderer.End();
        }

        public void ShutDown()
        {

        }
    }
}
