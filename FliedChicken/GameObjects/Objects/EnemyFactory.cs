using FliedChicken.Devices.AnimationDevice;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Objects
{
    static class EnemyFactory
    {
        private static Dictionary<string, Enemy> enemyDictionary = new Dictionary<string, Enemy>()
        {
            { "Kamome", new Enemy(64 * 3, 64 * 3) { MinSpeed = 4, MaxSpeed = 8, MinInterval = 2, MaxInterval = 4 } },
            { "slowenemy", new Enemy(64 * 10, 64 * 8) { MinSpeed = 1, MaxSpeed = 3, MinInterval = 12, MaxInterval = 15 } },
            { "highspeed_enemy", new Enemy(64 * 6, 64 * 4) { MinSpeed = 16, MaxSpeed = 18, MinInterval = 7, MaxInterval = 10} }
        };

        public static void Initialize()
        {
            enemyDictionary["Kamome"].Animation = new Animation("highspeed_enemy", new Vector2(400, 140), 6, 0.25f);
            enemyDictionary["highspeed_enemy"].Animation = new Animation("highspeed_enemy", new Vector2(400, 140), 6, 0.25f);
            enemyDictionary["slowenemy"].Animation = new Animation("slowenemy", new Vector2(445, 165), 8, 0.25f);
        }

        public static Enemy Create(string enemyName)
        {
            Enemy enemy = (Enemy)enemyDictionary[enemyName].Clone();

            return enemy;
        }

        public static ICollection<string> GetEnemyNameList()
        {
            return enemyDictionary.Keys;
        }
    }
}
