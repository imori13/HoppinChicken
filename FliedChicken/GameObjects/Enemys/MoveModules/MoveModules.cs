﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Enemys.MoveModules
{
    abstract class MoveModules
    {
        public GameObject GameObject { get; private set; }

        public MoveModules(GameObject GameObject)
        {
            this.GameObject = GameObject;
        }

        public abstract void Initialize();

        public abstract void Move();
    }
}
