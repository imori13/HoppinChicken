using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FliedChicken.Devices;
using FliedChicken.Objects;

namespace FliedChicken.GameObjects.Objects
{
    class CoinManager
    {
        public int CoinCount { get; private set; }

        private float coinRatio;
        private List<Coin> coinList;
        private ObjectsManager objectsManager;

        public CoinManager(ObjectsManager objectsManager, float coinRatio)
        {
            this.objectsManager = objectsManager;
            this.coinRatio = coinRatio;
            coinList = new List<Coin>();
        }

        public void Initialize()
        {
            foreach(var coin in coinList)
            {
                coin.Destroy();
            }

            coinList.Clear();
        }

        public void OnGetCoin()
        {
            CoinCount++;
        }

        public void GenerateCoin(EnemyLane lane)
        {
            var random = GameDevice.Instance().Random;
            float randomRatio = (float)random.NextDouble();
            if (!(randomRatio < coinRatio)) return;

            Vector2 basePosition = lane.Position - new Vector2(lane.LaneInfo.width / 2, 0);
            var newCoin = new Coin(this);
            newCoin.Position = basePosition + new Vector2((float)random.NextDouble() * lane.LaneInfo.width, 0);

            objectsManager.AddGameObject(newCoin);
        }

    }
}
