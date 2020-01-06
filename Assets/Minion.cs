using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour,IDamageable
{
    [SerializeField] private int _maxHealth;
    private int _health;
    public void DealDamage(int damagage)
    {
        var oldHp = _health;
        _health -= damagage;
        Debug.Log(string.Format("{0} Took {1} DMG, HP Before {2} HP now {3}", gameObject.name, damagage, oldHp, _health));
    }

    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
