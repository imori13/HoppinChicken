using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Objects;
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
        public bool PlayerGameStartFlag { get; set; }
        public float StartPositionY { get; set; }
        public float SumDistance { get; private set; }
        PlayerDeath playerDeath;
        public PlayerState state;

        public bool MutekiFlag { get; private set; }
        float mutekiTime;
        float mutekiLimit = 1;

        bool boundSoundFlag;
        float boundSoundTime;

        Random rand = GameDevice.Instance().Random;

        // ヒットしたかどうか
        public bool HitFlag { get; private set; }

        float normalParticleTime = 0;

        public Player(Camera camera)
        {
            this.camera = camera;
            GameObjectTag = GameObjectTag.Player;
            Collider = new CircleCollider(this, 20);
            playerScale = new PlayerScale(this);
            PlayerMove = new PlayerMove(this);
            OnechanBomManager = new OnechanBomManager(this);
            playerDeath = new PlayerDeath(this);
        }

        public override void Initialize()
        {
            playerScale.Initialize();
            PlayerMove.Initialize();
            playerDeath.Initialize();

            state = PlayerState.FLY;

            HitFlag = false;

            mutekiTime = 0;
            MutekiFlag = false;

            normalParticleTime = 0;

            StartPositionY = 0;
            SumDistance = 0;
            PlayerGameStartFlag = false;

            boundSoundFlag = false;
            boundSoundTime = 0;
        }

        public override void Update()
        {
            if (PlayerGameStartFlag)
            {
                SumDistance = Math.Abs(StartPositionY - Position.Y) / 100f;
            }

            switch (state)
            {
                case PlayerState.BEFOREFLY:
                    BeforeFly();
                    break;
                case PlayerState.FLY:
                    FlyUpdate();
                    break;
            }

            // 連続してバウンド音を鳴らさないようにしたい
            if (!boundSoundFlag)
            {
                boundSoundTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
                float limit = 0.025f;
                if (boundSoundTime >= limit)
                {
                    boundSoundTime = 0;
                    boundSoundFlag = true;
                }
            }
        }

        void BeforeFly()
        {

        }

        public void FlyUpdate()
        {
            // カメラの移動処理
            camera.Position = Vector2.Lerp(camera.Position, Position + Vector2.UnitY * 50f, 0.1f * TimeSpeed.Time);

            if (MutekiFlag)
            {
                mutekiTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * TimeSpeed.Time;

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

#if DEBUG
            // TODO:あとでけす
            if (Input.GetKeyDown(Keys.H))
            {
                var random = GameDevice.Instance().Random;
                int rotation = 360;
                while (rotation > 0)
                {
                    Vector2 direction = MyMath.DegToVec2(rotation);
                    direction = new Vector2(direction.X, direction.Y);
                    direction *= 0.3f;
                    var newParicle = new RadiationParticle2D(Position, Color.Yellow, direction, random);
                    ObjectsManager.AddBackParticle(newParicle);
                    rotation -= random.Next(0, 30 + 1);
                }

                for (int i = 0; i < 100; i++)
                {
                    ObjectsManager.AddBackParticle(new ExplosionParticle2D(ObjectsManager.Player.Position, MyMath.RandomCircleVec2(), Color.Black, random));
                }
            }

            if (Input.GetKeyDown(Keys.D))
            {
                HitFlag = true;

                GameDevice.Instance().Sound.PlaySE("Death");

                TimeSpeed.HitStop();

                // スコアをテキストファイルに記録
                ScoreStream.Instance().AddScore(ObjectsManager.GameScene.TitleDisplayMode.keyInput.Text, SumDistance);
            }
#endif
        }

        void ClearUpdate()
        {

        }

        public override void Draw(Renderer renderer)
        {
            if (!HitFlag)
                renderer.Draw2D("Chicken", Position, Color.White, 0, playerScale.DrawScale * 1.2f);
            else
                playerDeath.Draw(renderer);
        }

        public override void HitAction(GameObject gameObject)
        {
            if (HitFlag) { return; }

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

                if (boundSoundFlag)
                {
                    boundSoundFlag = false;
                    GameDevice.Instance().Sound.PlaySE("Bound0" + rand.Next(1, 4).ToString());
                }
            }

            if (gameObject.GameObjectTag == GameObjectTag.RedEnemy)
            {
                if (!MutekiFlag)
                {
                    if (OnechanBomManager.OneChanceFlag)
                    {
                        // ワンちゃんボム発動！！！
                        OnechanBomManager.Bom();
                        MutekiFlag = true;
                        GameDevice.Instance().Sound.PlaySE("Bom01");
                        GameDevice.Instance().Sound.PlaySE("Bom02");
                        TimeSpeed.HitStop();
                    }
                    else
                    {
                        // スコアをテキストファイルに記録
                        ScoreStream.Instance().AddScore(ObjectsManager.GameScene.TitleDisplayMode.keyInput.Text, SumDistance);

                        // しぬ
                        HitFlag = true;
                        GameDevice.Instance().Sound.PlaySE("Death");
                        TimeSpeed.HitStop();
                    }
                }
            }

            if (gameObject.GameObjectTag == GameObjectTag.OneChanceItem)
            {
                OnechanBomManager.AddCount();

                GameDevice.Instance().Sound.PlaySE("HitItem");
            }
        }

        void Default()
        {
            // プレイヤーのぼよんぼよんする挙動
            playerScale.Update();
            // プレイヤーの移動処理
            Velocity = PlayerMove.Velocity();
            Position = PlayerMove.Move();

            float limit = 0.1f;
            normalParticleTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * TimeSpeed.Time;
            while (normalParticleTime >= limit)
            {
                normalParticleTime -= limit;
                ObjectsManager.AddBackParticle(new PlayerTrajectory_Particle(Position + MyMath.RandomCircleVec2() * 10f, -Velocity / 100f, rand));
            }
        }
    }
}
