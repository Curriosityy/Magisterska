using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionHealth : BarStatistic, IDamageable
{
    public void DealDamage(int damagage)
    {
        var oldHp = Statistics;
        Statistics -= damagage;
        CalculateBar();
        SpawnText(damagage);
        if (Statistics <= 0)
        {
            Die();
        }
        Debug.Log(string.Format("{0} Took {1} DMG, HP Before {2} HP now {3}", gameObject.name, damagage, oldHp, Statistics));
    }

    private void Die()
    {
        //throw new NotImplementedException();
    }
}
