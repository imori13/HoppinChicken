using FliedChicken.Devices;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Clouds
{
    class CloudManager
    {
        public ObjectsManager ObjectsManager { get; set; }

        private List<BackCloud> backCloud = new List<BackCloud>();
        private List<FrontCloud> frontCloud = new List<FrontCloud>();

        private Vector2 prevCameraPos;
        private float distanceSum;

        Random rand = GameDevice.Instance().Random;

        public CloudManager(ObjectsManager ObjectsManager)
        {
            this.ObjectsManager = ObjectsManager;
        }

        public void Initialize()
        {
            backCloud.Clear();
            frontCloud.Clear();

            distanceSum = 0;
        }

        public void AddCloud(Cloud cloud)
        {
            if (cloud == null) { return; }

            cloud.Initialize();
            cloud.CloudManager = this;
            cloud.ObjectsManager = ObjectsManager;

            if (cloud is BackCloud) { backCloud.Add(cloud as BackCloud); }
            else if (cloud is FrontCloud) { frontCloud.Add(cloud as FrontCloud); }
        }

        public void Update()
        {
            backCloud.ForEach(b => b.Update());
            backCloud.RemoveAll(b => b.IsDead);

            frontCloud.ForEach(f => f.Update());
            frontCloud.RemoveAll(f => f.IsDead);


            distanceSum += Vector2.Distance(ObjectsManager.Camera.Position, prevCameraPos);
            while (distanceSum >= 0)
            {
                distanceSum -= 10f;

                GenerateCloud();
            }

            prevCameraPos = ObjectsManager.Camera.Position;
        }

        public void BackDraw(Renderer renderer)
        {
            backCloud.ForEach(b => b.Draw(renderer));
        }

        public void FrontDraw(Renderer renderer)
        {
            frontCloud.ForEach(f => f.Draw(renderer));
        }

        void GenerateCloud()
        {
            while (true)
            {
                float generateOffset = 1000f;
                Vector2 generatePos = new Vector2(
                    rand.Next((int)(ObjectsManager.Camera.Position.X - Screen.WIDTH / 2f - generateOffset), (int)(ObjectsManager.Camera.Position.X + Screen.WIDTH / 2f + generateOffset)),
                    rand.Next((int)(ObjectsManager.Camera.Position.Y - Screen.HEIGHT / 2f - generateOffset), (int)(ObjectsManager.Camera.Position.Y + Screen.HEIGHT / 2f + generateOffset)));

                float removeOffset = 200;
                float left = ObjectsManager.Camera.Position.X - Screen.WIDTH / 2f - removeOffset;
                float right = ObjectsManager.Camera.Position.X + Screen.WIDTH / 2f + removeOffset;
                float up = ObjectsManager.Camera.Position.Y - Screen.HEIGHT / 2f - removeOffset;
                float down = ObjectsManager.Camera.Position.Y + Screen.HEIGHT / 2f + removeOffset;

                if ((generatePos.X > left) && (generatePos.X < right) && (generatePos.Y > up) &&(generatePos.Y < down))
                {
                }
                else
                {
                    AddCloud(new BackCloud(generatePos));
                    break;
                }
            }
        }
    }
}
