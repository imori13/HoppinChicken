using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using FliedChicken.Devices.AnimationDevice;
using FliedChicken.GameObjects.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FliedChicken.GameObjects.PlayerDevices
{
    enum PlayerState
    {
        FLY,
        CLEAR,
    }
    class Player : GameObject
    {
        Camera camera;
        public PlayerState state;

        // モジュール
        PlayerScale playerScale;
        PlayerMove playerMove;
        OnechanBomManager onechanBomManager;
        public Animation animation;

        public Player(Camera camera)
        {
            this.camera = camera;
            GameObjectTag = GameObjectTag.Player;
            Collider = new CircleCollider(this, 30);
            playerScale = new PlayerScale(this);
            playerMove = new PlayerMove(this);
            onechanBomManager = new OnechanBomManager(this);
            animation = new Animation(this, "PlayerIdol", Vector2.One * 32, 3, 0.1f);
            animation.drawSize = Vector2.One * 2.5f;
        }

        public override void Initialize()
        {
            state = PlayerState.FLY;

            playerScale.Initialize();
            playerMove.Initialize();
            animation.Initialize();
        }

        public override void Update()
        {
            animation.Update();

            if (animation.FinishFlag)
            {
                animation = new Animation(this, "PlayerIdol", Vector2.One * 32, 3, 0.1f);
                animation.RepeatFlag = true;
                animation.drawSize = Vector2.One * 2.5f;
            }

            switch (state)
            {
                case PlayerState.FLY:
                    // プレイヤーのぼよんぼよんする挙動
                    playerScale.Update();
                    // プレイヤーの移動処理
                    Velocity = playerMove.Velocity();
                    Position = playerMove.Move();
                    // カメラの移動処理
                    camera.Position = Vector2.Lerp(camera.Position, Position + Vector2.UnitY * Screen.HEIGHT / 5f, 0.1f);
                    break;
                case PlayerState.CLEAR:
                    ClearUpdate();
                    break;
            }

#if DEBUG
            Dead();
#endif
        }

        public void FlyUpdate()
        {


        }

        public void ClearUpdate()
        {

        }

        public override void Draw(Renderer renderer)
        {
            //renderer.Draw2D("packman", Position, Color.White, 0, new Vector2(127, 132) / 2f, playerScale.DrawScale * 0.5f);
            animation.Draw(renderer, Vector2.Zero);
        }

        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.OrangeEnemy)
            {
                BoundBoxCollision(gameObject);
            }

            if (gameObject.GameObjectTag == GameObjectTag.RedEnemy)
            {
                //IsDead = true;
                if (onechanBomManager.OneChanceFlag)
                {
                    // ワンちゃんボム発動！！！
                    onechanBomManager.Bom();
                }
            }

            if (gameObject.GameObjectTag == GameObjectTag.OneChanceItem)
            {
                onechanBomManager.AddCount();
            }
        }

        private void Dead()
        {
            if (Input.GetKeyDown(Keys.D))
            {
                IsDead = true;
            }
        }
    }
}
