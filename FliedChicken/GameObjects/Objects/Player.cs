using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FliedChicken.GameObjects.Objects
{
    enum PlayerState
    {
        STOP,
        FLY,
        CLEAR,
    }
    class Player : GameObject
    {
        Camera camera;

        private int currentGrid;
        private Vector2 destPosition;

        private static readonly float FALLMAXSPEED = 10;
        private static readonly int MAXGRID = 11;

        private float time;

        public PlayerState state;

        public Player(Camera camera)
        {
            this.camera = camera;
        }

        public override void Initialize()
        {
            currentGrid = 6;
            state = PlayerState.STOP;
        }

        public override void Update()
        {
            switch (state)
            {
                case PlayerState.STOP:
                    StopUpdate();
                    break;
                case PlayerState.FLY:
                    FlyUpdate();
                    break;
                case PlayerState.CLEAR:
                    ClearUpdate();
                    break;
            }
        }

        public void StopUpdate()
        {
            if (Input.GetKeyDown(Keys.A))
            {
                state = PlayerState.FLY;
            }
        }

        public void FlyUpdate()
        {
            if (Input.GetKeyDown(Keys.Right) && currentGrid <= MAXGRID)
            {
                currentGrid++;
            }

            if (Input.GetKeyDown(Keys.Left) && currentGrid >= 1)
            {
                currentGrid--;
            }

            destPosition = new Vector2(currentGrid * 100, destPosition.Y) + Velocity;

            Velocity = Vector2.Lerp(Velocity, Vector2.UnitY * FALLMAXSPEED, 0.25f);

            Position = Vector2.Lerp(Position, destPosition, 0.2f);

            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

            // TODO : 上に上がらない処理を書く
            if (time >= 0.1f)
            {
                if (Input.GetKeyDown(Keys.Space))
                {
                    time = 0;
                    Velocity = new Vector2(Velocity.X, -15);
                }
            }

            camera.Position = Vector2.Lerp(camera.Position, Position + Vector2.UnitY * Screen.HEIGHT / 5f, 0.1f);
        }

        public void ClearUpdate()
        {

        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D("packman", Position, Color.White, 0, new Vector2(124 / 2f, 132 / 2f), Vector2.One * 0.5f);
        }
        public override void HitAction(GameObject gameObject)
        {

        }
    }
}
