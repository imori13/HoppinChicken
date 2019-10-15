﻿using FliedChicken.Devices;
using FliedChicken.SceneDevices;
using FliedChicken.ScenesDevice;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FliedChicken
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Renderer renderer;
        SceneManager sceneManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // フルスクリーンのオンオフを設定
            graphics.IsFullScreen = false;
            // フルスクリーンモードから画面を切り替えると実行中のまま消えるアレをなくす処理
            graphics.HardwareModeSwitch = false;
            // DrawメソッドをモニタのVerticalRetraceと同期しない
            graphics.SynchronizeWithVerticalRetrace = false;
            // Updateメソッドをデフォルトのレート(1/60秒)で呼び出し
            IsFixedTimeStep = true;

            Screen.UpdateScreenSize(graphics, Window);

            sceneManager = new SceneManager();
        }

        protected override void Initialize()
        {
            GameDevice.Instance(GraphicsDevice, Content);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Fonts.LoadFonts(Content);

            renderer = GameDevice.Instance().Renderer;

            Texture2D Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Color[] color = new Color[1];
            color[0] = Color.White;
            Pixel.SetData(color);
            renderer.LoadTexture("Pixel", Pixel);
            renderer.LoadTexture("packman", "Texture/");
            renderer.LoadTexture("stage", "Texture/");
            renderer.LoadTexture("slowenemy", "Texture/");
            renderer.LoadTexture("highspeed_enemy", "Texture/");
            
            sceneManager.AddScene(SceneEnum.GameScene, new GameScene());

            sceneManager.ChangeScene(SceneEnum.GameScene);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameDevice.Instance().Update(gameTime);
            TimeSpeed.Update();
            Screen.Update(graphics, Window);

            sceneManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            sceneManager.Draw(renderer);

            base.Draw(gameTime);
        }
    }
}
