using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Enemys.AttackModules
{
    abstract class AttackModules
    {
        public GameObject GameObject { get; private set; }

        public AttackModules(GameObject GameObject)
        {
            this.GameObject = GameObject;
        }

        public abstract void Initialize();

        public abstract void Attack();
    }
}
