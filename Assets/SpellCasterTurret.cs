﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SpellCasterTurret : MonoBehaviour
{
    [SerializeField] float _timeBetweenCast = 2f;
    float _timer = 0;
    List<string> _attackSpells;

    [SerializeField] GameObject _minionPrefab;
    Minion _controledMinion;
    Minion _target;
    private Transform _board;
    public void AssignToBoard(Transform board)
    {
        _board = board;
    }
    void Start()
    {
        Spell spell;
        _attackSpells = new List<string>();
        foreach (var spellName in SpellFactory.GetSpellsName())
        {
            spell = SpellFactory.GetSpell(spellName);
            if (spell.Type == SpellType.Offensive)
            {
                _attackSpells.Add(spellName);
            }
        }
        FindTarget();
    }

    private void FindTarget()
    {
        _target = _controledMinion.transform.parent.GetComponentsInChildren<Minion>().Where(m => m != _controledMinion).First();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timeBetweenCast <= _timer && _target.IsAlive)
        {
            _timer -= _timeBetweenCast;
            CastSpell(_attackSpells[UnityEngine.Random.Range(0, _attackSpells.Count)]);
            _controledMinion.GetComponent<MinionMana>().Statistics = 100;
        }
    }

    private void CastSpell(string v)
    {
        var spell = SpellFactory.GetSpell(v);
        int rand1 = UnityEngine.Random.Range((int)-1, 1);
        int rand2 = UnityEngine.Random.Range((int)-1, 1);
        string pos = "";

        pos += (char)Mathf.Clamp(((int)_target.Position[0] + rand1), 65, 65 + 7);
        pos += (char)Mathf.Clamp(((int)_target.Position[1] + rand1), 49, 49 + 7);

        spell.Cast(_controledMinion, pos);
    }

    public void Restart()
    {
        StopAllCoroutines();
        if (_controledMinion == null)
        {
            _controledMinion = Instantiate(_minionPrefab, Vector3.zero, Quaternion.identity, _board).GetComponent<Minion>();
            _controledMinion.name = "Turret";
            _controledMinion.Restart(2);
        }
        else
        {
            _controledMinion.Restart(2);
        }
        _timer = 0;
    }
}
