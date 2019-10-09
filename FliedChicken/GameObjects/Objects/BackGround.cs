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
        private float scrollSpeed;

        private Vector2 scrollStart;
        private Vector2 scrollEnd;

        private List<BackGroundObject> scrollObjects;

        public BackGround(Vector2 scrollStart, Vector2 scrollEnd)
        {
            this.scrollStart = scrollStart;
            this.scrollEnd = scrollEnd;
            scrollSpeed = 64.0f;
        }

        public override void Draw(Renderer renderer)
        {
#if DEBUG
            //デバッグ用範囲表示
#endif

            scrollObjects.ForEach(bgObj => bgObj.Draw(renderer));
        }

        public override void HitAction(GameObject gameObject)
        {
        }

        public override void Initialize()
        {
            scrollObjects = new List<BackGroundObject>();

            BackGroundObject lastBGObject = new BackGroundObject(new Vector2(704, 832));
            lastBGObject.Position = scrollStart - new Vector2(0, 832 / 2);
            scrollObjects.Add(lastBGObject);

            while (lastBGObject.Top().Y > scrollEnd.Y)
            {
                BackGroundObject newBGObject = new BackGroundObject(new Vector2(704, 832));
                newBGObject.Position = lastBGObject.Top() - newBGObject.Down();
                lastBGObject = newBGObject;
                scrollObjects.Add(newBGObject);
            }
        }

        public override void Update()
        {
            foreach (var bgObject in scrollObjects)
            {
                bgObject.Update();
                ScrollBGObject(bgObject);
                RepeatBGObject(bgObject);
            }
        }

        private void ScrollBGObject(BackGroundObject bgObject)
        {
            bgObject.Position -= new Vector2(0, 1) * scrollSpeed * TimeSpeed.Time; 
        }

        private void RepeatBGObject(BackGroundObject bgObject)
        {
            if (!(bgObject.Top().Y < scrollEnd.Y)) return;

            Vector2 halfHeight = new Vector2(0, bgObject.Size.Y / 2);
            bgObject.Position = scrollStart + halfHeight;
        }
    }
}
