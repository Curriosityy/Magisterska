using System;
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
    private int _dodged = 0;
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
                return getPoints() + DefensivePoints()+correct+fireballCasted+_controledMinion.SpellCasted*10;
            /*  return (100-_turret.GetComponent<MinionHealth>().Statistics)
                  +(OffensiveGameManager.Instance.GameTimer-_turret.Timer)
                  +_controledMinion.GetComponent<MinionMana>().spellCasted*10+1
                  + _controledMinion.Timer-neuralError;*/
            return 1;
        }
    }

    private int DefensivePoints()
    {
        if (_controledMinion.steps == 0)
        {
            return 0;
        }
        return _controledMinion.GetComponent<MinionHealth>().Statistics;
    }

    public float getPoints()
    {
        if(fireballCasted>0)
        {
            return 100-_turret.GetComponent<MinionHealth>().Statistics;
        }
        return 0;

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
    int restartPoint=0;
    public void Restart(int v=-1)
    {
        if(v!=-1)
            restartPoint = v;
        StopAllCoroutines();
        if (_controledMinion == null)
        {
            _controledMinion = Instantiate(_minionPrefab, Vector3.zero, Quaternion.identity, _board).GetComponent<Minion>();
            _controledMinion.Restart(restartPoint);
        }
        else
        {
            _controledMinion.Restart(restartPoint);
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
        if (IsAlive && !_controledMinion.IsDoingSomething && _timer >= _askTimer && _turret.IsAlive)
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
            if(ivalue==-1)
            {
                _dodged++;
            }
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

        inputValue.Add(_timer2);

        var value = _neuralNetwork.CalculateNeuralNetworkValue(inputValue.ToArray());
        int index = GetAwekedIndex(value);

        if (Selection.activeGameObject == _controledMinion.gameObject || Selection.activeGameObject == gameObject)
        {
            string a = "";
            foreach (var inp in inputValue)
            {
                a += inp.ToString() + " ";
            }
            Debug.Log(a + " equals = " + index + ", wait " + value[0] + ", shoot " + value[1] + ", jump " + value[2] + ", walk " + value[3] + " Output " + (int)value[4], this);
        }
        if (value[4] >= _tv.Length)
        {
            index = -1;
        } 
        if(float.IsNaN(value[4]))
        {
            index = -1;
        }

        if (index >= 0)
        {
            if (index == 1)
            {
                if ((int)value[4] != pos)
                {
                    Spell spell = SpellFactory.GetSpell("fireball");
                    //Debug.Log(_tv[pos2] + " " + _turret.Position);
                    spell?.Cast(_controledMinion, _tv[pos2]);
                    _timer2 = 0;
                }
                if (fireballCasted <= 10)
                    fireballCasted++;


            }
            else if (index == 2 && value[4] != pos2 && value[4] != pos)
            {
                _controledMinion.steps += 3;
                if ((int)value[4] != pos)
                {
                    Spell spell = SpellFactory.GetSpell("Teleport");
                    if ((int)(int)value[4] < 0 || (int)(int)value[4] >= _tv.Length)
                    {
                        Debug.Break();
                    }
                    spell?.Cast(_controledMinion, _tv[(int)value[4]]);
                    _timer2 = 0;
                }
            }
            else if (index == 3 && value[4] != pos2 && value[4] != pos)
            {
                _controledMinion.steps += 1;
                Spell spell = SpellFactory.GetSpell("walk");
                if ((int)(int)value[4] < 0 || (int)(int)value[4] >= _tv.Length)
                {
                    Debug.Break();
                }
                spell?.Cast(_controledMinion, _tv[(int)value[4]]);
            }
            if(correct<=10)
            correct++;
        }
        else
        {
            if (correct >= -10)
                correct--;
        }


    }


    private int GetAwekedIndex(float[] value)
    {
        var sum = 0f;
        int index = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += value[i];
        }
        for (int i = 0; i < 4; i++)
        {
            value[i] /= sum;
        }
        for (int i = 0; i < 4; i++)
        {
            if (value[i] >= value[index])
            {
                index = i;
            }
        }
        return index;
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
