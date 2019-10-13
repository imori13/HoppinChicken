using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace FliedChicken.GameObjects.Objects
{
    class ObjectLooper
    {
        public List<GameObject> LoopObjects { get; private set; }

        private float loopStart;
        private float loopEnd;

        public ObjectLooper(float loopStart, float loopEnd)
        {
            this.loopStart = loopStart;
            this.loopEnd = loopEnd;

            LoopObjects = new List<GameObject>();
        }

        public void Update()
        {
            LoopHorizontal();
        }

        private void LoopHorizontal()
        {
            foreach (var obj in LoopObjects)
            {
                if (!IsOverLoopEnd(obj)) continue;

                float diff = loopEnd - obj.Position.X;
                obj.Position = new Vector2(loopStart + diff, obj.Position.Y);
            }
        }

        private bool IsOverLoopEnd(GameObject obj)
        {
            if (loopEnd < 0)
                return obj.Position.X < loopEnd;
            else
                return obj.Position.X > loopEnd;
        }
    }
}
