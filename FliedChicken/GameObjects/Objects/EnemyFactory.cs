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
            { "Kamome", new Enemy("Pixel", 64 * 3, 64 * 3, 64 * 3, 64 * 3) { MinSpeed = 4, MaxSpeed = 8, MinInterval = 2, MaxInterval = 4 } },
            { "Zeppelin", new Enemy("Pixel", 64 * 10, 64 * 8, 64 * 10, 64 * 8) { MinSpeed = 1, MaxSpeed = 3, MinInterval = 12, MaxInterval = 15 } },
            { "Airplane", new Enemy("Pixel", 64 * 6, 64 * 4,  64 * 6, 64 * 4) { MinSpeed = 16, MaxSpeed = 18, MinInterval = 7, MaxInterval = 10} }
        };

        public static Enemy Create(string enemyName)
        {
            return (Enemy)enemyDictionary[enemyName].Clone();
        }

        public static ICollection<string> GetEnemyNameList()
        {
            return enemyDictionary.Keys;
        }
    }
}
