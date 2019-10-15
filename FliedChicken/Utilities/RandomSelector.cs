using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FliedChicken.Devices;

namespace FliedChicken.Utilities
{
    static class RandomSelector
    {
        public static T Select<T>(ICollection<T> collection)
        {
            return collection.ElementAt(GameDevice.Instance().Random.Next(0, collection.Count()));
        }
    }
}
