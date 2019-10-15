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

        public void OnGetCoin()
        {
            CoinCount++;
        }

        public void GenerateCoin(EnemyLane lane)
        {
            var random = GameDevice.Instance().Random;
            float randomRatio = (float)random.NextDouble();
            if (!(randomRatio <= coinRatio)) return;

            float laneWidth = lane.LaneInfo.width * 0.75f;

            Vector2 basePosition = lane.Position - new Vector2(laneWidth / 2, 0);
            var newCoin = new Coin(this);
            newCoin.Position = basePosition + new Vector2((float)random.NextDouble() * laneWidth, 0);

            objectsManager.AddGameObject(newCoin);
        }

        public void ClearCoin()
        {
            foreach (var coin in coinList)
            {
                coin.Destroy();
            }

            coinList.Clear();
        }

    }
}
