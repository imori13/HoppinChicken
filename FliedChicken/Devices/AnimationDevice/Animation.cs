using FliedChicken.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.Devices.AnimationDevice
{
    class Animation
    {
        public bool RepeatFlag { get; set; }
        string assetName;
        Vector2 size;
        int maxCount;
        float updateTime;
        public GameObject GameObject { get; set; }

        float time;
        int count;

        public Animation(string assetName, Vector2 size, int maxCount, float updateTime)
        {
            this.assetName = assetName;
            this.size = size;
            this.maxCount = maxCount;
            this.updateTime = updateTime;
        }

        public Animation Clone()
        {
            Animation animation = new Animation(assetName, size, maxCount, updateTime);
            animation.RepeatFlag = RepeatFlag;

            return animation;
        }

        public void Initialize()
        {
            count = 0;

            RepeatFlag = true;
        }

        public void Update()
        {
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

            // 時間がたったら
            if (time >= updateTime)
            {
                // カウント++
                count++;

                // もしカウントがMaxCountよりも小さいなら
                if (count < maxCount)
                {
                    // もう一度時間を数えなおす
                    time = 0;
                }
                // もしカウントがMaxCountよりも超えたら
                else
                {
                    // リピートフラグがtrueなら最初から
                    if (RepeatFlag)
                    {
                        time = 0;
                        count = 0;
                    }
                    // リピートしないなら
                    else
                    {
                        // 何もしない
                    }
                }
            }
        }

        public void Draw(Renderer renderer, Vector2 offset)
        {
            Rectangle rectangle = new Rectangle(
                (int)size.X * count, 0,
                (int)size.X, (int)size.Y);

            renderer.Draw2D(assetName, GameObject.Position + offset, rectangle, Color.White, 0, size / 2f, Vector2.One * Screen.ScreenSize);
        }
    }
}
