using FliedChicken.Devices;
using FliedChicken.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.UI
{
    class OneChanItemUI
    {
        public ObjectsManager ObjectsManager { get; private set; }

        public OneChanItemUI(ObjectsManager ObjectsManager)
        {
            this.ObjectsManager = ObjectsManager;
        }

        public void Initialize()
        {

        }

        public void Update()
        {

        }

        public void Draw(Renderer renderer)
        {
            if (ObjectsManager.Player.OnechanBomManager.Count >= 1)
            {
                renderer.Draw2D("Star", new Vector2(Screen.WIDTH -100,100), Color.White, 0,new Vector2(48,48), Vector2.One);
        }
    }
    }
}
