using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.GameObjects.Collision;
using FliedChicken.Devices;

namespace FliedChicken.GameObjects.Objects
{
    class Enemy : GameObject, ICloneable
    {
        public float MoveSpeed { get; set; }
        public Vector2 Size { get; private set; }

        public int MinSpeed { get; set; }
        public int MaxSpeed { get; set; }

        public int MinInterval { get; set; }
        public int MaxInterval{ get; set; }

        private string textureName;

        public Enemy(string textureName, float width, float height)
        {
            this.textureName = textureName;
            Size = new Vector2(width, height);
            Collider = new BoxCollider(this, Size);
        }

        public override void Initialize()
        {
        }

        public override void Update()
        {
            Position += new Vector2(MoveSpeed, 0) * TimeSpeed.Time;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Draw2D(textureName, Position, Color.White, 0.0f, Size);
        }

        public override void HitAction(GameObject gameObject)
        {
        }

        public object Clone()
        {
            var newEnemy = new Enemy(textureName, Size.X, Size.Y);
            newEnemy.Position = Position;
            newEnemy.MoveSpeed = MoveSpeed;
            newEnemy.MinSpeed = MinSpeed;
            newEnemy.MaxSpeed = MaxSpeed;
            newEnemy.MinInterval = MinInterval;
            newEnemy.MaxInterval = MaxInterval;
            newEnemy.ObjectsManager = ObjectsManager;
            return newEnemy;
        }

        public void Destroy()
        {
            IsDead = true;
        }

        public float GetRandomizedSpeed()
        {
            Random rand = GameDevice.Instance().Random;
            return rand.Next(MinSpeed, MaxSpeed + 1) + (float)rand.NextDouble();
        }

        public float GetRandomizedInterval()
        {
            Random rand = GameDevice.Instance().Random;
            return rand.Next(MinInterval, MaxInterval + 1) + (float)rand.NextDouble();
        }
    }
}
