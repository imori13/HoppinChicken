using FliedChicken.Devices;
using FliedChicken.GameObjects.Collision;
using FliedChicken.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects
{
    // 抽象クラス
    abstract class GameObject
    {
        // 変数
        public Vector2 Position { get; set; } // 位置
        public Vector2 Velocity { get; protected set; } // 移動量 処理は各自必要な場合
        public GameObjectTag GameObjectTag { get; set; }  // ゲームタグ
        public Collider Collider { get; protected set; }
        public bool IsDead { get; protected set; }  // 死亡してるか
        public ObjectsManager ObjectsManager { get; set; }

        // コンストラクタ
        public GameObject()
        {

        }

        // 抽象メソッド
        public abstract void Initialize();  // 初期化
        public abstract void Update();  // 更新
        public abstract void Draw(Renderer renderer);
        public abstract void HitAction(GameObject gameObject);  // 衝突を検知した時に呼ばれる処理
    }
}
