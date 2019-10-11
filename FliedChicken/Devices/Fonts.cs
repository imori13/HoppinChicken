using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.Devices
{
    static class Fonts
    {
        public static SpriteFont Font32 { get; private set; }

        public static void LoadFonts(ContentManager content)
        {
            Font32 = content.Load<SpriteFont>("Font/pixelMplus12_size32");
        }
    }
}
