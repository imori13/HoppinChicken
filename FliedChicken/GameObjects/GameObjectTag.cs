﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FliedChicken.GameObjects
{
    enum GameObjectTag
    {
        None,   // タグなし
        Player, // プレイヤー
        OrangeEnemy, // 当たっても死なない敵
        RedEnemy,   // 当たったら死ぬ敵
        DiveEnemy,
        OneChanceItem,  // ワンチャンスボムの発動条件のアイテム
        OneChanBom, // ワンちゃんボム
    }
}
