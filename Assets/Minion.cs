using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour,IDamageable
{
    [SerializeField] private int _maxHealth;
    private int _health;
    public void DealDamage(int damagage)
    {
        _health -= damagage;
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
