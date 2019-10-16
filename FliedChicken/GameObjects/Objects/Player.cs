using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FliedChicken.GameObjects.Objects
{
    enum PlayerState
    {
        FLY,
        CLEAR,
    }
    class Player : GameObject
    {
        private static readonly float FALLMAXSPEED = 10;
        private static readonly int MAXGRID = 11;


        Camera camera;

        private int currentGrid;
        private Vector2 destPosition;

        private float time;
        private bool inputflag;

        public PlayerState state;

        private const float xExpandSpeed = 1.0f;
        private const float xShrinkSpeed = 0.05f;

        private const float yExpandSpeed = 0.05f;
        private const float yShrinkSpeed = 1.0f;

        private const float xMinScale = 0.75f;
        private const float xMaxScale = 1.25f;

        private const float yMinScale = 0.5f;
        private const float yMaxScale = 1.5f;

        private Vector2 preVelocity;
        private Vector2 drawScale;

        public Player(Camera camera)
        {
            this.camera = camera;
            GameObjectTag = GameObjectTag.Player;
            Collider = new CircleCollider(this, 30);
            //Collider = new BoxCollider(this, new Vector2(127, 132) / 2f);
        }

        public override void Initialize()
        {
            currentGrid = 6;
            inputflag = false;
            state = PlayerState.FLY;
            drawScale = new Vector2(1, 1);
        }

        public override void Update()
        {
            switch (state)
            {
                case PlayerState.FLY:
                    FlyUpdate();
                    break;
                case PlayerState.CLEAR:
                    ClearUpdate();
                    break;
            }

            ChangeDrawScale();
            preVelocity = Velocity;
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

            if (Input.GetKeyDown(Keys.Space) || inputflag)
            {
                if (time >= 0.15f)
                {
                    time = 0;
                    Velocity = new Vector2(Velocity.X, -10);
                    inputflag = false;
                }
                else
                {
                    inputflag = true;
                }
            }

            camera.Position = Vector2.Lerp(camera.Position, Position + Vector2.UnitY * Screen.HEIGHT / 5f, 0.1f);
        }

        public void ClearUpdate()
        {

        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D("packman", Position, Color.White, 0, new Vector2(127, 132) / 2f, drawScale * 0.5f);
        }
        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.Enemy)
            {
                IsDead = true;
            }
        }

        private void ChangeDrawScale()
        {
            float newX = drawScale.X;
            float newY = drawScale.Y;

            //前フレームの速度より早い場合
            if (Velocity.Y >= preVelocity.Y)
            {
                //縦に伸びて横に縮む
                newX = MathHelper.Lerp(newX, xMinScale, xShrinkSpeed);
                newY = MathHelper.Lerp(newY, yMaxScale, yExpandSpeed);
            }
            else
            {
                //横に伸びて縦に縮む
                newX = MathHelper.Lerp(newX, xMaxScale, xExpandSpeed);
                newY = MathHelper.Lerp(newY, yMinScale, yShrinkSpeed);
            }

            drawScale = new Vector2(newX, newY);
        }
    }
}
