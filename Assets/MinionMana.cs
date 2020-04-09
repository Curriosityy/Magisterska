using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMana : BarStatistic
{
    [SerializeField]int _manaPerSec=10;
    float _timer=0;
    public void BurnMana(int manaToBurn)
    {
        var oldMana = Statistics;
        Statistics -= manaToBurn;
        CalculateBar();
        SpawnText(manaToBurn);
        //Debug.Log(string.Format("{0} Burned {1} mana, mana Before {2} mana now {3}", gameObject.name, manaToBurn, oldMana, Statistics));
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer>=1f)
        {
            _timer -= 1;
            Statistics += _manaPerSec;
        }
    }
}
