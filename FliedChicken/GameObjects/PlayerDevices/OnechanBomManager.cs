using FliedChicken.Devices;
using FliedChicken.GameObjects.PlayerDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects.PlayerDevices
{
    class OnechanBomManager
    {
        Player player;
        public int Count { get; private set; }

        public bool OneChanceFlag { get; private set; }

        public OnechanBomManager(Player player)
        {
            this.player = player;
            Count = 0;
        }

        public void Bom()
        {
            Count = 0;
            OneChanceFlag = false;

            // ボムを生成
            player.ObjectsManager.AddGameObject(new OnechanBom(player, 500f));
        }

        // プレイヤーヒット時の呼び出し用
        public void AddCount()
        {
            Count++;

            if (Count >= 1) { OneChanceFlag = true; }
        }
    }
}
