using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMana : BarStatistic
{
    public void BurnMana(int manaToBurn)
    {
        var oldMana = Statistics;
        Statistics -= manaToBurn;
        CalculateBar();
        SpawnText(manaToBurn);
        Debug.Log(string.Format("{0} Burned {1} mana, mana Before {2} mana now {3}", gameObject.name, manaToBurn, oldMana, Statistics));
    }
}
