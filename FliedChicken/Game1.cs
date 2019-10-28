using FliedChicken.Devices;
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
            graphics.IsFullScreen = true;
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
            TimeSpeed.Initialize();

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
            renderer.LoadTexture("slowenemy", "Texture/");
            renderer.LoadTexture("highspeed_enemy", "Texture/");
            renderer.LoadTexture("normal_enemy", "Texture/");
            renderer.LoadTexture("DiveEnemy", "Texture/");
            renderer.LoadTexture("PlayerIdol", "Texture/");
            renderer.LoadTexture("PlayerFly", "Texture/");
            renderer.LoadTexture("PlayerDead", "Texture/");
            renderer.LoadTexture("FlyedChickenTitle", "Texture/");
            renderer.LoadTexture("Mine", "Texture/");
            renderer.LoadTexture("Star", "Texture/");
            renderer.LoadTexture("slowMode", "Texture/");

            renderer.LoadTexture("Title", "Texture/");
            renderer.LoadTexture("Chicken", "Texture/");
            renderer.LoadTexture("NormalEnemy", "Texture/");
            renderer.LoadTexture("HighSpeed", "Texture/");
            renderer.LoadTexture("SlowEnemy", "Texture/");
            renderer.LoadTexture("Killer_Active", "Texture/");
            renderer.LoadTexture("Killer_Passive", "Texture/");
            renderer.LoadTexture("NameWIndow", "Texture/");
            renderer.LoadTexture("DiveEnemyUI", "Texture/");
            renderer.LoadTexture("RankingLR", "Texture/");
            renderer.LoadTexture("ESCAPE!!", "Texture/");

            Sound sound = GameDevice.Instance().Sound;

            sound.LoadSE("Bom01", "SE/");
            sound.LoadSE("Bom02", "SE/");
            sound.LoadSE("Jump01", "SE/");
            sound.LoadSE("Jump02", "SE/");
            sound.LoadSE("Jump03", "SE/");
            sound.LoadSE("Jump04", "SE/");
            sound.LoadSE("Jump05", "SE/");
            sound.LoadSE("HitItem", "SE/");
            sound.LoadSE("Bound01", "SE/");
            sound.LoadSE("Bound02", "SE/");
            sound.LoadSE("Bound03", "SE/");
            sound.LoadSE("Death", "SE/");

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
            DebugMode.Update();

            sceneManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(155, 190, 225));

            sceneManager.Draw(renderer);

            renderer.Begin();
            TimeSpeed.Draw(renderer);
            renderer.End();

            base.Draw(gameTime);
        }
    }
}
