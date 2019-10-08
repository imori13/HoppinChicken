using FliedChicken.Devices;
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

namespace FliedChicken.SceneDevices
{
    class GameScene : IScene
    {
        Camera camera;
        ObjectsManager objectsManager;

        public GameScene()
        {
            camera = new Camera();
            objectsManager = new ObjectsManager(camera);
        }

        public void Initialize()
        {
            camera.Initialize();
            objectsManager.Initialize();
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
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin(camera);

            renderer.Draw2D("packman", Vector2.Zero, Color.White);
            objectsManager.Draw(renderer);

            renderer.End();
        }

        public void ShutDown()
        {

        }
    }
}
