using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SpellCasterTurret : MonoBehaviour
{
    public bool t = false;
    [SerializeField] float _timeBetweenCast = 1f;
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
            StartCoroutine(DoSequence());
            _controledMinion.GetComponent<MinionMana>().Statistics = 100;
        }
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
        if (t==true)
        {
            pos = _target.Position;
            t = false;
        }
        else
        {
            pos += (char)(65 + UnityEngine.Random.Range(0, 7));
            pos += (char)(49 + UnityEngine.Random.Range(0, 7));
            t = true;
        }
        spell.Cast(_controledMinion, pos);
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
    }
}
