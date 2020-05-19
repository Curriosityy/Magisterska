using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
public class OffensiveAiControler : MonoBehaviour
{
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
    public int generalPoins = 1;
    public int oC = 1;
    public int dC = 1;
    public int rC = 1;

    private int move = 0;
    public int damageDealt = 0;
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
            /*if(_controledMinion.SpellCasted==0)
            {
                return 1;
            }*/
            if (_controledMinion != null)
                return getPoints();
            /*  return (100-_turret.GetComponent<MinionHealth>().Statistics)
                  +(OffensiveGameManager.Instance.GameTimer-_turret.Timer)
                  +_controledMinion.GetComponent<MinionMana>().spellCasted*10+1
                  + _controledMinion.Timer-neuralError;*/
            return 1;
        }
    }
    public float getPoints()
    {
        //float pp= damageDealt + generalPoins + _controledMinion.SpellCasted;
        /*if (pp > NeatValues.BestFitness)
        {
            Debug.Log(_controledMinion.Hitat.Count + " " + _controledMinion.GetComponent<MinionHealth>().Statistics);
            foreach(var hit in _controledMinion.Hitat)
            {
                Debug.Log(hit);
            }
        }
        if (generalPoins < 1)
        {
            generalPoins = 1;
        }
        */
        return ((damageDealt/oC)*10+ (_controledMinion.GetComponent<MinionHealth>().Statistics / dC)*10)/rC + _controledMinion.SpellCasted;
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
        damageDealt = 1;
        generalPoins = 1;
        dC = 1;
        oC = 1;
        rC = 1;
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
    //punkty za unikniecie i fuck mana

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
            if(vecmag>10 || vecmag < -10)
            {
                vecmag = -1;
            }
            Debug.DrawLine(cmPos, closest.transform.position, Color.red);
            inputValue.Add(vecmag);

            float ivalue = Vector3.Dot(vec, vec2) / (vecmag * vec2.magnitude);
            if (Selection.activeGameObject == _controledMinion.gameObject)
            {
                //Debug.Log(ivalue);
            }
            inputValue.Add(ivalue*10);
        }
        else
        {
            inputValue.Add(-1);
            inputValue.Add(0);
        }
        float pos2 = ((int)_turret.Position[0] - 65) * 7 + ((int)_turret.Position[1] - 49);
        int pos = ((int)_controledMinion.Position[0] - 65) * 7 + ((int)_controledMinion.Position[1] - 49);
        //inputValue.Add(pos);
        //inputValue.Add(_controledMinion.GetComponent<MinionMana>().Statistics / 100f);
        inputValue.Add(pos2);
        //inputValue.Add(_turret.GetComponent<MinionMana>().Statistics / 100f);

        //inputValue.Add(_timer2);

        var value = _neuralNetwork.CalculateNeuralNetworkValue(inputValue.ToArray());

       
        value[2] = Mathf.RoundToInt(value[2]);
        value[3] = Mathf.RoundToInt(value[3]);


    
        if (Selection.activeGameObject == _controledMinion.gameObject || Selection.activeGameObject == gameObject)
        {
            string a = "";
            foreach (var inp in inputValue)
            {
                a += inp.ToString() + " ";
            }
            Debug.Log(a + " results: shooting " + value[0] + ", teleporting " + value[1] + ", waiting"+ value[2] + ", field " + value[3], this);
        }


        if (value[3] > 0 && value[3]<49 && value[2] > 0 && value[2] < 49)
        {
            
            if (_controledMinion.GetComponent<MinionMana>().Statistics == 100)
            {
                _controledMinion.SpellCasted += 1;
                if (value[1]>= value[0])
                {
                    oC += 1;
                    //Debug.Log("shooting at " + value[3]);
                    if ((int)pos2 == value[2])
                    {
                        

                        damageDealt += 10;
                        
                        Debug.Log("HIT AT " + pos2, this);
                        //Debug.Log("Hitted", this);
                        _turret.GetComponent<MinionHealth>().DealDamage(10);
                        _controledMinion.Hitat.Add((int)pos2);
                        _controledMinion.GetComponent<MinionMana>().BurnMana(30);
                    }
                    else
                    {
                        //generalPoins -= 1;
                        //generalPoins -= 10;
                    }
                   
                }
                else if (value[0] > value[1])
                {
                    dC += 1;
                    if(inputValue[0]>0 && inputValue[1] < -8)
                    {
                       // generalPoins += 10;
                    }
                    else
                    {
                        //generalPoins -= 10;
                    }
                    Spell spell = SpellFactory.GetSpell("Teleport");
                    spell?.Cast(_controledMinion, _tv[(int)value[3]]);
                    //Debug.Log("teleporting at " + value[3]);
                    _controledMinion.GetComponent<MinionMana>().BurnMana(30);
                }
                else
                {
                    _controledMinion.SpellCasted += 1;
                    //generalPoins -= 1;
                }
               
                /*
                if (value[0] <= 1)
                {
                    if((int)value[1]<50&& (int)value[1] > 0)
                    {
                        if(_controledMinion.SpellCasted < 30)
                        {
                            //_controledMinion.SpellCasted += 3;
                        }
                        
                    }
                    
                    if (pos2 == (int)value[1] && damageDealt<100)
                    {
                        damageDealt += 10;
                        Debug.Log("HIT AT " + pos2,this);
                        //Debug.Log("Hitted", this);
                        _turret.GetComponent<MinionHealth>().DealDamage(10);
                        _controledMinion.Hitat.Add(pos2);
                    }
                    _controledMinion.GetComponent<MinionMana>().BurnMana(40);
                    _timer2 = 0;
                }
                else if (value[0] <= 2)
                {
                    if (value[1] != pos)
                    {
                        
                        if ((int)value[1] < 50 && (int)value[1] > 0)
                        {
                            if (_controledMinion.SpellCasted < 30)
                            {
                                //_controledMinion.SpellCasted += 3;
                            }
                            Spell spell = SpellFactory.GetSpell("Teleport");
                            spell?.Cast(_controledMinion, _tv[(int)value[1]]);
                        }
                        
                        
                        _timer2 = 0;
                    }
                }*/
            }
        }
        else
        {
            rC += 1;
        }
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
