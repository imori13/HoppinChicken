using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Enemys.AttackModules
{
    abstract class AttackModule
    {
        public GameObject GameObject { get; private set; }

        public AttackModule(GameObject GameObject)
        {
            this.GameObject = GameObject;
        }

        public abstract void Initialize();

        public abstract void Attack();
    }
}
