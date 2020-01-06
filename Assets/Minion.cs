using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Minion : MonoBehaviour,IDamageable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private Image _healthBar;
    private int _health;
    public void DealDamage(int damagage)
    {
        var oldHp = _health;
        _health -= damagage;
        CalculateHealthBar();
        if(_health<=0)
        {
            Die();
        }
        Debug.Log(string.Format("{0} Took {1} DMG, HP Before {2} HP now {3}", gameObject.name, damagage, oldHp, _health));
    }

    private void Die()
    {
        throw new NotImplementedException();
    }

    private void CalculateHealthBar()
    {
        _healthBar.fillAmount = (_health * 1.0f)/ _maxHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;
        CalculateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
