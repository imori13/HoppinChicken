using FliedChicken.Devices;
using FliedChicken.SceneDevices.Title;
using FliedChicken.ScenesDevice;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.SceneDevices
{
    class TitleScene : SceneBase
    {
        T_KeyWindow keyWindow;
        T_Player player;
        T_ParticleManager particleManager;

        float time;

        public TitleScene()
        {
            keyWindow = new T_KeyWindow();
            particleManager = new T_ParticleManager();
            player = new T_Player(particleManager);
        }

        public override void Initialize()
        {
            time = 0;
            keyWindow.Initialize();
            player.Initialize();
            particleManager.Initialize();

            base.Initialize();
        }

        public override void Update()
        {
            keyWindow.Update();
            player.Update();
            particleManager.Update();

            if (player.IsClear())
            {
                time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                float limitTime = 1;
                if (time > limitTime)
                {
                    ShutDown = true;
                }
            }

            base.Update();
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Begin();
            renderer.DrawString(Fonts.Font10_128, "TitleName", new Vector2(Screen.WIDTH / 2f, 250 * Screen.ScreenSize), Color.White, 0, Fonts.Font10_128.MeasureString("TitleName") / 2f, Vector2.One * Screen.ScreenSize);
            keyWindow.Draw(renderer);
            player.Draw(renderer);
            particleManager.Draw(renderer);
            renderer.End();

            base.Draw(renderer);
        }

        public override SceneEnum NextScene()
        {
            return SceneEnum.GameScene;
        }
    }
}
