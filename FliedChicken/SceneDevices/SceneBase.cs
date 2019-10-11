﻿using FliedChicken.Devices;
using FliedChicken.ScenesDevice;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.SceneDevices
{
    abstract class SceneBase
    {
        public bool IsEndFlag { get; private set; } // これsetをprivate以外にしないで
        public bool ShutDown { get; protected set; } // shundownをtrueにしたら終了するようになる

        public bool StartFlag { get; set; }

        Vector2 InSize;
        Vector2 OutSize;

        public SceneBase()
        {

        }

        public virtual void Initialize()
        {
            StartFlag = false;
            ShutDown = false;
            IsEndFlag = false;

            InSize = new Vector2(Screen.WIDTH, Screen.HEIGHT);
            OutSize = new Vector2(Screen.WIDTH, 0);
        }

        public virtual void Update()
        {
            if (!StartFlag)
            {
                InSize = Vector2.Lerp(InSize, new Vector2(Screen.WIDTH, 0), 0.1f);
            }

            if (ShutDown)
            {
                OutSize = Vector2.Lerp(OutSize, new Vector2(Screen.WIDTH, Screen.HEIGHT), 0.1f);

                if (Vector2.Distance(OutSize, new Vector2(Screen.WIDTH, Screen.HEIGHT)) <= 0.1f)
                {
                    IsEndFlag = true;
                }
            }
        }
        public virtual void Draw(Renderer renderer)
        {
            Color color = new Color(50, 50, 50);
            renderer.Begin();
            renderer.Draw2D("Pixel", new Vector2(Screen.WIDTH / 2f, Screen.HEIGHT), color, 0, new Vector2(0.5f, 1), InSize);
            renderer.Draw2D("Pixel", new Vector2(Screen.WIDTH / 2f, 0), color, 0, new Vector2(0.5f, 0), OutSize);
            renderer.End();
        }

        public abstract SceneEnum NextScene();
    }
}
