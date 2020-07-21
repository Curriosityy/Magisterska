using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JumpingTurret : MonoBehaviour
{
    [SerializeField] float _timeBetweenCast = 3f;
    float _timer = 0;
    private bool shootTarget = true;
    private bool shootTarget2 = true;
    int _a = 0;
    int _tempHp = 0;
    List<string> _attackSpells;
    [SerializeField] GameObject _minionPrefab;
    Minion _controledMinion;
    Minion _target;
    private Transform _board;
    int _ticker = 0;
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
            if (spell.Type == SpellType.Defensive)
            {
                _attackSpells.Add(spellName);
            }
        }
        FindTarget();
    }

    private void FindTarget()
    {
       // _target = _controledMinion.transform.parent.GetComponentsInChildren<Minion>().Where(m => m != _controledMinion).First();
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            _target = FindObjectOfType<Player>().Minion;
            return;
        }
        if(_controledMinion == null)
        {
            Restart();
        }
        if (_target.GetComponent<MinionHealth>().Statistics <= 0)
            return;
        _timer += Time.deltaTime;
        if (_controledMinion.GetComponent<MinionHealth>().Statistics > 0)
        {
            if (_timer >= 1f)
            {
                if (_a % 2 == 0)
                {
                    CastOffenciveSpell("fireball");
                }
                else
                {
                    if (shootTarget)
                    {
                        CastOffenciveSpell("fireball");
                        shootTarget = false;
                    }
                    else
                    {
                        Teleport();
                        shootTarget = true;
                    }
                    _timer = 0;
                }
                _timer = 0;
                _a++;
                _a %= 2;
            }
        }

        if (_tempHp != _controledMinion.GetComponent<MinionHealth>().Statistics)
        {
            Teleport();
            _tempHp = _controledMinion.GetComponent<MinionHealth>().Statistics;
        }
        _controledMinion.GetComponent<MinionMana>().Statistics=100;
    }

    IEnumerator DoSequence()
    {
        Teleport();
        yield return new WaitForSeconds(0.5f);
        CastOffenciveSpell(_attackSpells[UnityEngine.Random.Range(0, _attackSpells.Count)]);
    }

    private void CastOffenciveSpell(string v)
    {
        var spell = SpellFactory.GetSpell(v);
        string pos = "";
        if (shootTarget2)
        {
            if (_ticker < 3)
            {
                pos = _target.Position;
                _ticker++;
            }
            else
            {
                pos += (char)(65 + UnityEngine.Random.Range(0, 7));
                pos += (char)(49 + UnityEngine.Random.Range(0, 7));
                shootTarget2 = true;
                _ticker = 0;
            }
            spell.Cast(_controledMinion, pos);
        }
    }
    private void Teleport()
    {
        var spell = SpellFactory.GetSpell("Teleport");
        string pos = "";
        pos += (char)(65 + UnityEngine.Random.Range(0, 7));
        pos += (char)(49 + UnityEngine.Random.Range(0, 7));
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
        _tempHp = _controledMinion.GetComponent<MinionHealth>().Statistics;
        _ticker = 0;
    }
}

