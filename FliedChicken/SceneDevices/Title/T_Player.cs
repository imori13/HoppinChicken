using FliedChicken.Devices;
using FliedChicken.Particle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.SceneDevices.Title
{
    class T_Player
    {
        T_ParticleManager particleManager;
        Vector2 position;
        Vector2 destPosition;
        int count;
        static readonly int MAXCOUNT = 3;

        Random rand;

        float time;

        public T_Player(T_ParticleManager particleManager)
        {
            this.particleManager = particleManager;
        }

        public void Initialize()
        {
            position = new Vector2(Screen.WIDTH / 1.3f, Screen.HEIGHT / 1.3f);
            destPosition = position;
            time = -0.1f;
            rand = GameDevice.Instance().Random;
            count = 0;
        }

        public void Update()
        {
            if (count < MAXCOUNT)
            {
                if (Input.GetKeyDown(Keys.Space))
                {
                    count++;
                    destPosition -= Vector2.UnitY * 100;
                }
            }
            else
            {
                float limit = 0.05f;
                time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                if (time > limit)
                {
                    time = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        particleManager.AddParticle(new RadiationParticle2D(position, Color.Yellow, MyMath.RandomCircleVec2(), rand));
                    }
                }
            }

            position = Vector2.Lerp(position, destPosition, 0.1f);
        }

        public void Draw(Renderer renderer)
        {
            renderer.Draw2D("packman", position, Color.White);
        }

        public bool IsClear()
        {
            return count >= MAXCOUNT;
        }
    }
}
