using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace FliedChicken.GameObjects.Objects
{
    interface IOneChanItemCarrier
    {
        OneChanItem OneChanItem { get; set; }

        Vector2 GetItemPosition();
    }
}
