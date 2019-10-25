using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Collision;
using FliedChicken.GameObjects.Particle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FliedChicken.GameObjects.PlayerDevices
{
    class Player : GameObject
    {
        public enum PlayerState
        {
            BEFOREFLY,
            FLY,
        }

        Camera camera;

        // モジュール
        PlayerScale playerScale;
        public PlayerMove PlayerMove { get; private set; }
        public OnechanBomManager OnechanBomManager { get; private set; }
        public Animation animation;
        PlayerDeath playerDeath;
        public PlayerState state;

        public bool MutekiFlag { get; private set; }
        float mutekiTime;
        float mutekiLimit = 1;

        Random rand = GameDevice.Instance().Random;

        // ヒットしたかどうか
        public bool HitFlag { get; private set; }

        float time = 0;

        public Player(Camera camera)
        {
            this.camera = camera;
            GameObjectTag = GameObjectTag.Player;
            Collider = new CircleCollider(this, 20);
            playerScale = new PlayerScale(this);
            PlayerMove = new PlayerMove(this);
            OnechanBomManager = new OnechanBomManager(this);
            animation = new Animation(this, "PlayerIdol", Vector2.One * 114, 3, 0.1f);
            animation.drawSize = Vector2.One * 0.5f;
            playerDeath = new PlayerDeath(this);
        }

        public override void Initialize()
        {
            playerScale.Initialize();
            PlayerMove.Initialize();
            animation.Initialize();
            playerDeath.Initialize();

            state = PlayerState.FLY;

            HitFlag = false;

            mutekiTime = 0;
            MutekiFlag = false;

            time = 0;

        }

        public override void Update()
        {

            switch (state)
            {
                case PlayerState.BEFOREFLY:
                    BeforeFly();
                    break;
                case PlayerState.FLY:
                    FlyUpdate();
                    break;
            }
        }

        void BeforeFly()
        {

        }

        public void FlyUpdate()
        {
            // カメラの移動処理
            camera.Position = Vector2.Lerp(camera.Position, Position + Vector2.UnitY * 50f, 0.1f);

            if (MutekiFlag)
            {
                mutekiTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                if (mutekiTime >= mutekiLimit)
                {
                    mutekiTime = 0;
                    MutekiFlag = false;
                }
            }


            if (!HitFlag)
                Default();
            else
                playerDeath.Update();
        }

        void ClearUpdate()
        {

        }

        public override void Draw(Renderer renderer)
        {

            if (!HitFlag)
                animation.Draw(renderer, Vector2.Zero);
            else
                playerDeath.Draw(renderer);
        }

        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.OrangeEnemy)
            {
                if (gameObject.Collider is CircleCollider)
                {
                    BoundCircleCollision(gameObject);
                }
                else
                {
                    BoundBoxCollision(gameObject);
                }
            }

            if (gameObject.GameObjectTag == GameObjectTag.RedEnemy)
            {
                if (!MutekiFlag)
                {
                    if (OnechanBomManager.OneChanceFlag)
                    {
                        // ワンちゃんボム発動！！！
                        ObjectsManager.Camera.Shake(40, 5, 0.95f);

                        OnechanBomManager.Bom();
                        MutekiFlag = true;
                    }
                    else
                    {
                        HitFlag = true;
                    }
                }
            }

            if (gameObject.GameObjectTag == GameObjectTag.OneChanceItem)
            {
                OnechanBomManager.AddCount();
            }
        }

        void Default()
        {
            animation.Update();

            if (animation.FinishFlag)
            {
                animation = new Animation(this, "PlayerIdol", Vector2.One * 114, 3, 0.1f);
                animation.RepeatFlag = true;
                animation.drawSize = Vector2.One * 0.5f;

                animation.Color = (MutekiFlag) ? (Color.Yellow) : (Color.White);
            }

            // プレイヤーのぼよんぼよんする挙動
            playerScale.Update();
            // プレイヤーの移動処理
            Velocity = PlayerMove.Velocity();
            Position = PlayerMove.Move();

            if (PlayerMove.PlayerMoveState == PlayerMoveState.Fall)
            {
                ObjectsManager.AddBackParticle(new FallParticle2D(Position + MyMath.RandomCircleVec2() * 20f - Vector2.UnitY * 100f, Color.Blue, Vector2.UnitY, rand));
            }
            else
            {
                float limit = 0.1f;
                time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
                while (time >= limit)
                {
                    time -= limit;
                    ObjectsManager.AddBackParticle(new PlayerTrajectory_Particle(Position + MyMath.RandomCircleVec2() * 10f, -Velocity / 100f, rand));
                }
            }
        }
    }
}
