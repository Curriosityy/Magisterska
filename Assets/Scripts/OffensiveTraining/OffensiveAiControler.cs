﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
public class OffensiveAiControler : MonoBehaviour
{
    private int correct = 0;
    private int neuralError = 0;
    [SerializeField] private NeuralNetwork _neuralNetwork;
    [SerializeField] private GameObject _minionPrefab;
    [SerializeField] private float _askTimer = 0.3f;
    [SerializeField] private float _teleTime = 0f;
    [SerializeField] private float _askTeleTimer = 1f;
    private float _timer = 0;
    private float _timer2 = 2;
    private string[] _tv;
    private BoardDictionary _bd;
    private Minion _controledMinion;
    public Transform spawnPoint;
    private Transform _board;
    private Minion _turret;

    private int move = 0;
    public int damageDealt = 0;
    private int fireballCasted = 0;
    public static List<int> roadplan = new List<int>();

    public void AssignToBoard(Transform board)
    {
        _board = board;


    }
    public NeuralNetwork NeuralNetwork { get => _neuralNetwork; set => _neuralNetwork = value; }
    public float Points
    {
        get
        {
            if (_controledMinion.SpellCasted == 0)
            {
                return 1;
            }
            if (_controledMinion != null && _controledMinion.SpellCasted > 0)
                return getPoints() + DefensivePoints()+correct+fireballCasted;
            /*  return (100-_turret.GetComponent<MinionHealth>().Statistics)
                  +(OffensiveGameManager.Instance.GameTimer-_turret.Timer)
                  +_controledMinion.GetComponent<MinionMana>().spellCasted*10+1
                  + _controledMinion.Timer-neuralError;*/
            return 1;
        }
    }

    private int DefensivePoints()
    {
        if(_controledMinion.steps==0)
        {
            return 0;
        }
        return  _controledMinion.GetComponent<MinionHealth>().Statistics;
    }

    public float getPoints()
    {
        move = 0;
        _teleTime = 0;

        //float res = damageDealt + _controledMinion.SpellCasted / 2 + 1;
        float res = damageDealt;

        damageDealt = 0;
        if (res > 100)
        {
            return 100;
        }
        return res;

    }
    public bool IsAlive
    {
        get
        {
            if (_controledMinion != null)
                return _controledMinion.IsAlive;
            return false;

        }
    }

    public Minion ControledMinion { get => _controledMinion; }

    public void Restart()
    {
        StopAllCoroutines();
        if (_controledMinion == null)
        {
            _controledMinion = Instantiate(_minionPrefab, Vector3.zero, Quaternion.identity, _board).GetComponent<Minion>();
            _controledMinion.Restart(1);
        }
        else
        {
            _controledMinion.Restart(1);
            _controledMinion.GetComponentInChildren<Renderer>().material.color = _neuralNetwork.Color;
        }
        _bd = _controledMinion.transform.parent.GetComponent<BoardDictionary>();
        if (_tv == null)
            _tv = _bd.Board.Select(b => b.Key).ToArray();
        _turret = _controledMinion.transform.parent.GetComponentsInChildren<Minion>().Where(m => m != _controledMinion).FirstOrDefault();
        _timer = 0;
        _controledMinion.GetComponent<MinionMana>().spellCasted = 0;
        neuralError = 0;
        _timer2 = 0;
        _controledMinion.SpellCasted = 0;
        correct = 0;
        fireballCasted = 0;
    }

    private void FixedUpdate()
    {
        _timer += Time.deltaTime;
        _timer2 += Time.deltaTime;
        if (IsAlive && !_controledMinion.IsDoingSomething && _timer >= _askTimer)
        {
            CalculateMove();
            _timer = 0;
        }


    }

    private void CalculateMove()
    {
        List<float> inputValue = new List<float>();
        var cmPos = _controledMinion.transform.position;
        var closest = Physics.OverlapBox(cmPos, new Vector3(7, 1, 7), Quaternion.identity).Where(c => c.tag == "Attack"
                                        && c.GetComponent<FireBall>().Caster != _controledMinion)
                                        .OrderBy(c => (c.transform.position - cmPos).sqrMagnitude).FirstOrDefault();
        if (closest != null)
        {
            var vec = closest.transform.position - cmPos;
            var vec2 = closest.GetComponent<FireBall>().FlyingVector;
            var vecmag = vec.magnitude;
            Debug.DrawLine(cmPos, closest.transform.position, Color.red);
            if (vecmag > 20)
                inputValue.Add(-1);
            else
                inputValue.Add(vecmag);

            float ivalue = Vector3.Dot(vec, vec2) / (vecmag * vec2.magnitude);
            if (Selection.activeGameObject == _controledMinion.gameObject)
            {
                //Debug.Log(ivalue);
            }
            inputValue.Add(ivalue * 10);
        }
        else
        {
            inputValue.Add(-1);
            inputValue.Add(0);
        }
        int pos2 = ((int)_turret.Position[0] - 65) * 7 + ((int)_turret.Position[1] - 49);
        int pos = ((int)_controledMinion.Position[0] - 65) * 7 + ((int)_controledMinion.Position[1] - 49);
        inputValue.Add(pos);
        inputValue.Add(_controledMinion.GetComponent<MinionMana>().Statistics / 100f);
        inputValue.Add(pos2);
        inputValue.Add(_turret.GetComponent<MinionMana>().Statistics / 100f);

        //inputValue.Add(_timer2);

        var value = _neuralNetwork.CalculateNeuralNetworkValue(inputValue.ToArray());
        int index = GetAwekedIndex(value);
        int index2 = GetAwekedFieldIndex(value);

        if (Selection.activeGameObject == _controledMinion.gameObject || Selection.activeGameObject == gameObject)
        {
            string a = "";
            foreach (var inp in inputValue)
            {
                a += inp.ToString() + " ";
            }
            Debug.Log(a + " equals = " + index + ", wait "+ value[0] + ", shoot "+ value[1] + ", jump "+ value[2] + ", walk " + value[3]+" Output " + value[4] , this);
        }
        if (value[4] >= _tv.Length)
        {
            index = 0;
        }

        if (index > 0)
        {
            if (_controledMinion.GetComponent<MinionMana>().Statistics >= 20)
            {

                if (index == 1)
                {
                    _controledMinion.SpellCasted += 1;
                    if (pos2 == (int)index2)
                    {
                        damageDealt += 20;
                        Debug.Log("HIT AT " + pos2, this);
                        //Debug.Log("Hitted", this);
                        _turret.GetComponent<MinionHealth>().DealDamage(20);
                        _controledMinion.Hitat.Add(pos2);
                    }
                    _controledMinion.GetComponent<MinionMana>().BurnMana(20);
                    _timer2 = 0;
                    //Debug.Log("Shoot AT " + pos2);
                    //_controledMinion.SpellCasted == 0
                    fireballCasted++;
                }
                else if (index == 2)
                {
                    _controledMinion.steps += 3;
                    if ((int)value[4] != pos)
                    {
                        Spell spell = SpellFactory.GetSpell("Teleport");
                        if(index2 < 0 || index2>= _tv.Length)
                        {
                            int a = 3;
                        }
                        spell?.Cast(_controledMinion, _tv[index2]);
                        _controledMinion.SpellCasted += 1;
                        _timer2 = 0;
                    }
                }
                
            }
            if (index == 3)
            {
                _controledMinion.SpellCasted += 1;
                _controledMinion.steps += 1;
                Spell spell = SpellFactory.GetSpell("walk");
                if (index2 < 0 || index2 >= _tv.Length)
                {
                    int a = 3;
                }
                spell?.Cast(_controledMinion, _tv[index2]);
            }
            if(correct<=10)
                correct++;
        }
        else
        {
            if(correct>=-10)
                correct--;
        }
    }


    private int GetAwekedIndex(float[] value)
    {
        var sum = 0f;
        int index=0;
        for(int i=0;i<4;i++)
        {
            sum += value[i];
        }
        for (int i = 0; i < 4; i++)
        {
            value[i] /= sum;
        }
        for (int i = 0; i < 4; i++)
        {
            if(value[i]>=value[index])
            {
                index = i;
            }
        }
        return index;
    }

    private int GetAwekedFieldIndex(float[] value)
    {
        var sum = 0f;
        int index = 4;
        for (int i = 4; i < 49+4; i++)
        {
            sum += value[i];
        }
        for (int i = 4; i < 49 + 4; i++)
        {
            value[i] /= sum;
        }
        for (int i = 4; i < 49 + 4; i++)
        {
            if (value[i] >= value[index])
            {
                index = i;
            }
        }
        return index-4;
    }



    private float[] GetTableValues()
    {
        float[] tableInfo = new float[_bd.Board.Count];
        //var vec = _controledMinion.transform.position
        int i = 0;
        foreach (var point in _bd.Board)
        {
            tableInfo[i++] = point.Value.GetComponent<PointInfo>().AiInfo;
        }
        return tableInfo;
    }
}
