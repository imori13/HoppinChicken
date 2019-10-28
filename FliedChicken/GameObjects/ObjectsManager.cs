using FliedChicken.Devices;
using FliedChicken.GameObjects;
using FliedChicken.GameObjects.Enemys;
using FliedChicken.GameObjects.Particle;
using FliedChicken.GameObjects.PlayerDevices;
using FliedChicken.SceneDevices;
using System.Collections.Generic;

namespace FliedChicken.GameObjects
{
    // 複数Managerをひとつにまとめて、1つの参照で複数のマネージャーにアクセスしたい
    // 保守性などない
    class ObjectsManager
    {
        private List<GameObject> gameobjects = new List<GameObject>();
        private List<Particle2D> backParticles = new List<Particle2D>();
        private List<Particle2D> frontParticles = new List<Particle2D>();

        private List<GameObject> addGameObjects = new List<GameObject>();
        private List<Particle2D> addBackParticles = new List<Particle2D>();
        private List<Particle2D> addFrontParticles = new List<Particle2D>();

        public Player Player { get; private set; }
        public DiveEnemy DiveEnemy { get; private set; }
        public Camera Camera { get; private set; }
        public GameScene GameScene { get; private set; }

        public ObjectsManager(Camera camera, GameScene GameScene)
        {
            this.Camera = camera;
            this.GameScene = GameScene;
        }

        public void Initialize()
        {
            gameobjects.Clear();
            backParticles.Clear();
            addBackParticles.Clear();
            frontParticles.Clear();
            addFrontParticles.Clear();
        }

        public void AddGameObject(GameObject gameobject)
        {
            if (gameobject == null) { return; }

            gameobject.ObjectsManager = this;
            addGameObjects.Add(gameobject);

            gameobject.Initialize();

            if (gameobject is Player) { Player = gameobject as Player; }
            if (gameobject is DiveEnemy) { DiveEnemy = gameobject as DiveEnemy; }
        }

        public void AddBackParticle(Particle2D particle)
        {
            if (particle == null) { return; }

            particle.Initialize();
            addBackParticles.Add(particle);
        }

        public void AddFrontParticle(Particle2D particle)
        {
            if (particle == null) { return; }

            particle.Initialize();
            addFrontParticles.Add(particle);
        }

        public void Update()
        {
            gameobjects.AddRange(addGameObjects);
            addGameObjects.Clear();
            gameobjects.ForEach(g => g.Update());

            frontParticles.AddRange(addFrontParticles);
            addFrontParticles.Clear();
            frontParticles.ForEach(p => p.Update());

            backParticles.AddRange(addBackParticles);
            addBackParticles.Clear();
            backParticles.ForEach(p => p.Update());

            CheckCollision();

            gameobjects.RemoveAll(g => g.IsDead);
            frontParticles.RemoveAll(p => p.IsDead);
            backParticles.RemoveAll(p => p.IsDead);
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
            backParticles.ForEach(p => p.Draw(renderer));
            gameobjects.ForEach(g => g.Draw(renderer));

            if (DebugMode.DebugFlag)
            {
                foreach (var g in gameobjects)
                {
                    if (g.Collider != null)
                    {
                        g.Collider.Draw(renderer);
                    }
                }
            }

            frontParticles.ForEach(p => p.Draw(renderer));
        }
    }
}
