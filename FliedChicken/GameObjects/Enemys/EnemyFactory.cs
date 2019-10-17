using FliedChicken.Devices.AnimationDevice;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.Enemys
{
    static class EnemyFactory
    {
        private static Dictionary<string, Enemy> enemyDictionary = new Dictionary<string, Enemy>()
        {
            { "normal_enemy", new NormalEnemy() },
            { "slowenemy", new SlowEnemy() },
            { "highspeed_enemy", new HighSpeedEnemy() }
        };

        public static void Initialize()
        {
        }

        public static Enemy Create(string enemyName)
        {
            return enemyDictionary[enemyName].Clone();
        }

        public static ICollection<string> GetEnemyNameList()
        {
            return enemyDictionary.Keys;
        }
    }
}
