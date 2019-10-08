using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.Devices
{
    public static class TimeSpeed
    {
        static float time;
        static float dest;
        public static float Time { get { return time * frame*60; } private set { time = value; } }
        static float frame;

        public static void Initialize()
        {
            time = 60;
        }

        public static void Update()
        {
            dest = (Input.GetKey(Keys.T)) ? (0.25f) : (1.00f);

            // 1フレームにかかった時間
            // ほとんどの移動、時間の変化に関係する処理にTimeSpeed.Timeがかけられているので、
            // TimeSpeed.Timeにフレーム秒が影響されるようにする。
            frame = (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

            time = MathHelper.Lerp(time, dest, 0.1f);
        }
    }
}
