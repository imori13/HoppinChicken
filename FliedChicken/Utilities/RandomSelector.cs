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

        public static T WeightSelect<T>(ICollection<WeightSelectHelper<T>> collection)
        {
            var random = GameDevice.Instance().Random;
            int weightSum = collection.Sum(select => select.Value);
            int randomValue = random.Next(1, weightSum + 2);

            int index = 0;

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection.ElementAt(i).Value >= randomValue)
                {
                    index = i;
                    break;
                }
                randomValue -= collection.ElementAt(i).Value;
            }

            return collection.ElementAt(index).SelectObject;
    
        }
    }
}
