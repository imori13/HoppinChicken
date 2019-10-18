using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Enemys.AttackModules
{
    class NotAttack_AM : AttackModule
    {
        public NotAttack_AM(GameObject GameObject) : base(GameObject)
        {

        }
        public override void Initialize()
        {

        }

        public override void Attack()
        {
            // 何もしない
        }
    }
}
