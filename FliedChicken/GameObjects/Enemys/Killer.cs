using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Enemys.AttackModules;
using FliedChicken.GameObjects.Enemys.MoveModules;

namespace FliedChicken.GameObjects.Enemys
{
    class Killer : Enemy
    {
        public Killer(Camera camera) : base(camera)
        {
            AttackModule = new NotAttack_AM(this);
            MoveModule = new Killer_MM(this);
        }

        public override void Initialize()
        {
            AttackModule.Initialize();
            MoveModule.Initialize();
        }

        public override void Update()
        {
            AttackModule.Attack();
            MoveModule.Move();
        }
        public override void Draw(Renderer renderer)
        {

        }

        public override void HitAction(GameObject gameObject)
        {

        }

        protected override bool IsDestroy()
        {
            return false;
        }

        protected override void OnDestroy()
        {

        }
    }
}
