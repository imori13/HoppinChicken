using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Objects
{
    class BackGround : GameObject
    {
        private Camera camera;
        private List<BackGroundObject> backGroundObjects;

        public BackGround(Camera camera)
        {
            this.camera = camera;
            Position = camera.Position;
            backGroundObjects = new List<BackGroundObject>();
        }

        public override void Initialize()
        {
            for (int i = -1; i < 2; i++)
            {
                var bgObject = new BackGroundObject("stage", new Vector2(704, 832));
                bgObject.Position = Position + new Vector2(0, Screen.HEIGHT * i);

                ObjectsManager.AddGameObject(bgObject);
                backGroundObjects.Add(bgObject);
            }
        }

        public override void Update()
        {
            Position = camera.Position;

            foreach (var bgObject in backGroundObjects)
            {
                RepeatBGObject(bgObject);
            }
        }

        public override void Draw(Renderer renderer)
        {
        }

        public override void HitAction(GameObject gameObject)
        {
        }

        private void RepeatBGObject(BackGroundObject bgObject)
        {
            if (!bgObject.IsOutOfScreenUp(camera)) return;

            bgObject.Position = new Vector2(Position.X, GetLowestYPosition() + Screen.HEIGHT / 2);
        }

        private float GetLowestYPosition()
        {
            return backGroundObjects.Max<BackGroundObject, float>(bgObj => bgObj.Down().Y);
        }
    }
}
