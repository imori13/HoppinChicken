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

        private List<GameObject> addGameObjects = new List<GameObject>();
        private List<Particle2D> addParticles = new List<Particle2D>();

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
            addGameObjects.Add(gameobject);
        }

        public void AddParticle(Particle2D particle)
        {
            if (particle == null) { return; }

            particle.Initialize();
            addParticles.Add(particle);
        }

        public void Update()
        {
            gameobjects.ForEach(g => g.Update());
            gameobjects.AddRange(addGameObjects);
            addGameObjects.Clear();

            particles.ForEach(p => p.Update());
            particles.AddRange(addParticles);
            addParticles.Clear();

            CheckCollision();

            gameobjects.RemoveAll(g => g.IsDead);
            particles.RemoveAll(p => p.IsDead);
        }

        public void CheckCollision()
        {
            for (int i = 0; i < gameobjects.Count; i++)
            {
                for (int j = 0; j < gameobjects.Count; j++)
                {
                    if (i >= j) { continue; }
                    if (gameobjects[i] == null || gameobjects[i].IsDead || gameobjects[i].Collider == null) { continue; }
                    if (gameobjects[j] == null || gameobjects[j].IsDead || gameobjects[j].Collider == null) { continue; }

                    if (gameobjects[i].Collider.IsCollision(gameobjects[j].Collider))
                    {
                        gameobjects[i].HitAction(gameobjects[j]);
                        gameobjects[j].HitAction(gameobjects[i]);
                    }
                }
            }
        }

        public void Draw(Renderer renderer)
        {
            gameobjects.ForEach(g => g.Draw(renderer));
            particles.ForEach(p => p.Draw(renderer));
        }
    }
}
