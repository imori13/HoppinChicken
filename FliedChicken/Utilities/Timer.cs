using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices;

namespace FliedChicken.Utilities
{
    class Timer
    {
        public float MaxTime { get; set; }
        private float TotalGameTime
        {
            get
            {
                if (gameTime == null)
                {
                    gameTime = GameDevice.Instance().GameTime;
                    return 0;
                }
                return gameTime.TotalGameTime.Seconds;
            }
        }

        private GameTime gameTime;
        private float compareTime;

        public Timer(float maxTime)
        {
            MaxTime = maxTime;
            gameTime = GameDevice.Instance().GameTime;
            compareTime = TotalGameTime;
        }

        public void Reset()
        {
            compareTime = TotalGameTime;
        }

        public bool IsTime()
        {
            return GetCurrentTime() > MaxTime;
        }

        public float GetCurrentTime()
        {
            return TotalGameTime - compareTime;
        }

        public float GetRatio()
        {
            return GetCurrentTime() / MaxTime;
        }

        public float GetClampedRatio()
        {
            return Math.Min(1, GetRatio());
        }

    }
}
