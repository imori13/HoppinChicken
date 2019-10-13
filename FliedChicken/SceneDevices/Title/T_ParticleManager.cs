using FliedChicken.Devices;
using FliedChicken.Particle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.SceneDevices.Title
{
    class T_ParticleManager
    {
        List<Particle2D> particles = new List<Particle2D>();

        public T_ParticleManager()
        {

        }

        public void Initialize()
        {
            particles.Clear();
        }

        public void AddParticle(Particle2D particle)
        {
            if (particle == null) { return; }

            particle.Initialize();
            particles.Add(particle);
        }

        public void Update()
        {
            particles.ForEach(p => p.Update());
            particles.RemoveAll(p => p.IsDead);
        }

        public void Draw(Renderer renderer)
        {
            particles.ForEach(p => p.Draw(renderer));
        }
    }
}
