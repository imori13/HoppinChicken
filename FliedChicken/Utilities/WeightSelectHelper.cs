using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.Utilities
{
    class WeightSelectHelper<T>
    {
        public int Value { get; private set; }
        public T SelectObject { get; private set; }

        public WeightSelectHelper(int value, T selectObject)
        {
            Value = value;
            SelectObject = selectObject;
        }
    }
}
