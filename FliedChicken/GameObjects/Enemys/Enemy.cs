using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FliedChicken.GameObjects.Particle;
using FliedChicken.GameObjects.Enemys.AttackModules;
using FliedChicken.GameObjects.Enemys.MoveModules;
using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Enemys
{

    abstract class Enemy : GameObject
    {
        /// <summary>
        /// テクスチャ名。Animationが設定されている場合は無視される。
        /// </summary>
        protected string TextureName { get; set; }

        /// <summary>
        /// nullの場合はTextureNameで指定された画像を描画する。
        /// </summary>
        public Animation Animation { get; protected set; }

        public Vector2 Size { get; protected set; }
        public Vector2 DrawOffset { get; protected set; }

        public Func<Vector2, Vector2> SpawnPosFunc { get; protected set; }

        protected AttackModule AttackModule { get; set; }
        protected MoveModule MoveModule { get; set; }

        protected SpriteEffects SpriteEffect { get; set; }
        
        protected Camera Camera { get; set; }

        private Vector2 previousPosition;

        public Enemy(Camera camera)
        {
            Camera = camera;
        }

        public override void Initialize()
        {
            if (Animation != null)
                Animation.Initialize();
        }

        public override void Update()
        {
            if (Animation != null)
                Animation.Update();

            if (IsDestroy())
            {
                IsDead = true;
                OnDestroy();
            }

            SetDrawDirection();
            previousPosition = Position;
        }

        public override void Draw(Renderer renderer)
        {
            if (Animation == null)
                renderer.Draw2D(TextureName, Position, Color.White, SpriteEffect);
            else
                Animation.Draw(renderer, DrawOffset, SpriteEffect);
        }

        protected void SetDrawDirection()
        {
            if (previousPosition.X < Position.X)
                SpriteEffect = SpriteEffects.FlipHorizontally;
            else
                SpriteEffect = SpriteEffects.None;
        }

        /// <summary>
        /// 死亡時エフェクト
        /// </summary>
        protected virtual void DestroyEffect(Vector2 scale)
        {
            var random = GameDevice.Instance().Random;
            int rotation = 360;
            while (rotation > 0)
            {
                Vector2 direction = MyMath.DegToVec2(rotation);
                direction = new Vector2(direction.X * scale.X, direction.Y * scale.Y);
                direction *= 0.3f;
                var newParicle = new RadiationParticle2D(Position, Color.Yellow, direction, random);
                ObjectsManager.AddBackParticle(newParicle);
                rotation -= random.Next(0, 30 + 1);
            }

            for (int i = 0; i < 100; i++)
            {
                ObjectsManager.AddBackParticle(new ExplosionParticle2D(ObjectsManager.Player.Position, MyMath.RandomCircleVec2(), Color.Red, random));
            }
        }

        /// <summary>
        /// 死亡条件メソッド
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsDestroy();

        /// <summary>
        /// 死亡時に呼び出されるコールバック
        /// </summary>
        protected abstract void OnDestroy();
    }
}
