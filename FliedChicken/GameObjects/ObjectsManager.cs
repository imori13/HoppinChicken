using FliedChicken.Devices;
using FliedChicken.GameObjects;
using FliedChicken.Particle;
using System.Collections.Generic;

namespace FliedChicken.Objects
{
    // 複数Managerをひとつにまとめて、1つの参照で複数のマネージャーにアクセスしたい
    // 保守性などない
    class ObjectsManager
    {
        private Camera camera;

        private List<GameObject> gameobjects = new List<GameObject>();
        private List<Particle2D> particles = new List<Particle2D>();

        public ObjectsManager(Camera camera)
        {
            this.camera = camera;
        }

        public void Initialize()
        {
            gameobjects.Clear();
            particles.Clear();
        }

        public void AddGameObject(GameObject gameobject)
        {
            if (gameobject == null) { return; }

            gameobject.Initialize();

            gameobject.ObjectsManager = this;

            gameobjects.Add(gameobject);
        }

        public void AddParticle(Particle2D particle)
        {
            if (particle == null) { return; }

            particle.Initialize();

            particles.Add(particle);
        }

        public void Update()
        {
            gameobjects.ForEach(g => g.Update());
            particles.ForEach(p => p.Update());

            gameobjects.RemoveAll(g => g.IsDead);
            particles.RemoveAll(p => p.IsDead);
        }

        public void Draw(Renderer renderer)
        {
            gameobjects.ForEach(g => g.Draw(renderer));
            particles.ForEach(p => p.Draw(renderer));
        }
    }
}
