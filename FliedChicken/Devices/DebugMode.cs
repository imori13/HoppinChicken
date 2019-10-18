using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.Devices
{
    static class DebugMode
    {
        public static bool DebugFlag { get; private set; }

        public static void Update()
        {
            DebugFlag = (Input.GetKeyDown(Keys.F12)) ? (!DebugFlag) : (DebugFlag);
        }
    }
}
