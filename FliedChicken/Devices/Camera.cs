using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.Devices
{
    class Camera
    {
        public Vector2 Position { get; set; }
        public Vector2 Zoom { get; set; }
        public float Rotation { get; set; }

        public Matrix Matrix
        {
            get
            {
                Vector3 pos = new Vector3(Position, 0);
                Vector3 screenPos = new Vector3(Screen.WIDTH / 2f, Screen.HEIGHT / 2f, 0);

                return Matrix.CreateTranslation(-pos) *
                       Matrix.CreateScale(new Vector3(Zoom, 0)) *
                       Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation)) *
                       Matrix.CreateTranslation(screenPos);

            }
        }

        public Camera()
        {
            Zoom = Vector2.One;
        }

        public void Initialize()
        {

        }

        public void Update()
        {

        }
    }
}
