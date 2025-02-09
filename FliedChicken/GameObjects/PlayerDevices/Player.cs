﻿using System;
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
    class Player : GameObject, IOneChanItemCarrier
    {
        public enum PlayerState
        {
            BEFOREFLY,
            FLY,
        }

        Camera camera;

        // モジュール
        public PlayerScale PlayerScale { get; private set; }
        public PlayerMove PlayerMove { get; private set; }
        public OnechanBomManager OnechanBomManager { get; private set; }
        public bool PlayerGameStartFlag { get; set; }
        public float StartPositionY { get; set; }
        public float SumDistance { get; private set; }
        PlayerDeath playerDeath;
        public PlayerState state;
        public float Vivration { get; set; }

        public bool MutekiFlag { get; private set; }
        float mutekiTime;
        float mutekiLimit = 1;

        bool boundSoundFlag;
        float boundSoundTime;

        Random rand = GameDevice.Instance().Random;

        // ヒットしたかどうか
        public bool HitFlag { get; private set; }
        public OneChanItem OneChanItem { get; set; }

        float normalParticleTime = 0;
        float oneChanRotation;

        public Player(Camera camera)
        {
            this.camera = camera;
            GameObjectTag = GameObjectTag.Player;
            Collider = new CircleCollider(this, 20);
            PlayerScale = new PlayerScale(this);
            PlayerMove = new PlayerMove(this);
            OnechanBomManager = new OnechanBomManager(this);
            playerDeath = new PlayerDeath(this);

            //見た目だけ欲しいのでクラスだけ追加(クソ手抜き)
            OneChanItem = new OneChanItem(this);
        }

        public override void Initialize()
        {
            PlayerScale.Initialize();
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
            Vivration = 0;
        }

        public override void Update()
        {
            if (PlayerGameStartFlag && !HitFlag)
            {
                SumDistance = Math.Max(SumDistance, Math.Abs(StartPositionY - Position.Y) / 100f);
            }

            // プレイヤーとDiveEnemyが近ければカメラが上に行く処理
            // ゴリラプログラミング。読むと毒
            if (PlayerGameStartFlag)
            {
                float distance = Vector2.Distance(Position, ObjectsManager.DiveEnemy.Position);

                float limitMax = 600;
                distance = MathHelper.Clamp(distance, 0, limitMax);

                float offsetY = 0;

                float min = -300;
                float max = 250;
                
                offsetY = Easing2D.SineIn(distance, limitMax, min, max);

                offsetY = MathHelper.Clamp(offsetY, 0, max);

                camera.Position = Vector2.Lerp(camera.Position, Position + Vector2.UnitY * offsetY, 0.1f * TimeSpeed.Time);
            }
            else
            {
                camera.Position = Vector2.Lerp(camera.Position, Position + Vector2.UnitY * 50, 0.1f * TimeSpeed.Time);
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

            Vivration = (TimeSpeed.IsHitStop) ? (1) : (Vivration);

            Vivration = MathHelper.Lerp(Vivration, 0, 0.2f);

            Input.SetVibration(0, Vivration);

            //見た目だけ欲しいので手動更新
            OneChanItem.Update();
        }

        void BeforeFly()
        {
            Default();
        }

        public void FlyUpdate()
        {
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
        }

        void ClearUpdate()
        {
            Default();
        }

        public override void Draw(Renderer renderer)
        {
            if (OnechanBomManager.OneChanceFlag && !HitFlag)
                OneChanItem.Draw(renderer);

            if (!HitFlag)
                renderer.Draw2D("Chicken", Position, Color.White, 0, PlayerScale.DrawScale * 1.2f);
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

                Vivration = 2f;

                if (boundSoundFlag)
                {
                    boundSoundFlag = false;
                    GameDevice.Instance().Sound.PlaySE("Bound0" + rand.Next(1, 4).ToString());
                }
            }

            if (gameObject.GameObjectTag == GameObjectTag.RedEnemy || gameObject.GameObjectTag == GameObjectTag.DiveEnemy)
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
                        if (!DebugMode.DebugFlag)
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
            PlayerScale.Update();
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

        public Vector2 GetItemPosition()
        {
            return Position;
        }
    }
}
