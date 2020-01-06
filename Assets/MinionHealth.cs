using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionHealth : BarStatistic, IDamageable
{
    public void DealDamage(int damagage)
    {
        var oldHp = _statistic;
        _statistic -= damagage;
        CalculateBar();
        SpawnText(damagage);
        if (_statistic <= 0)
        {
            Die();
        }
        Debug.Log(string.Format("{0} Took {1} DMG, HP Before {2} HP now {3}", gameObject.name, damagage, oldHp, _statistic));
    }

    private void Die()
    {
        //throw new NotImplementedException();
    }
}
