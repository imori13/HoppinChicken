using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FliedChicken.Devices;
using FliedChicken.GameObjects.Collision;
using Microsoft.Xna.Framework;
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

        public Player(Camera camera)
        {
            this.camera = camera;
            GameObjectTag = GameObjectTag.Player;
            Collider = new CircleCollider(this, 30);
            //Collider = new BoxCollider(this, new Vector2(127, 132) / 2f);
            playerScale = new PlayerScale(this);
            playerMove = new PlayerMove(this);
        }

        public override void Initialize()
        {
            state = PlayerState.FLY;

            playerScale.Initialize();
            playerMove.Initialize();
        }

        public override void Update()
        {
            switch (state)
            {
                case PlayerState.FLY:
                    // プレイヤーのぼよんぼよんする挙動
                    playerScale.Update();
                    // プレイヤーの移動処理
                    Position = playerMove.Update();
                    Velocity = playerMove.Velocity;
                    // カメラの移動処理
                    camera.Position = Vector2.Lerp(camera.Position, Position + Vector2.UnitY * Screen.HEIGHT / 5f, 0.1f);
                    break;
                case PlayerState.CLEAR:
                    ClearUpdate();
                    break;
            }
        }

        public void FlyUpdate()
        {


        }

        public void ClearUpdate()
        {

        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D("packman", Position, Color.White, 0, new Vector2(127, 132) / 2f, playerScale.DrawScale * 0.5f);
        }

        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.Enemy)
            {
                IsDead = true;
            }
        }
    }
}
