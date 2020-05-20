using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMana : BarStatistic
{
    [SerializeField]int _manaPerSec=40;
    public int spellCasted=0;
    public float lastCastTimer = 0f;
    float _timer=0;
    
    public void BurnMana(int manaToBurn)
    {
        var oldMana = Statistics;
        Statistics -= manaToBurn;
        CalculateBar();
        SpawnText(manaToBurn);
        GetComponent<Minion>().SpellCasted++;
        lastCastTimer = 0;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        lastCastTimer += Time.deltaTime;
        if(_timer>=1f)
        {
            _timer -= 1;
            if(Statistics<maxStat)
            {
                Statistics += _manaPerSec;
            }
        }
    }
}
