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
    private float _timer=0;
    private string[] _tv;
    private BoardDictionary _bd;
    private Minion _controledMinion;
    public Transform spawnPoint;
    private Transform _board;
    private Minion _turret;
    
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
            if (_controledMinion != null && _controledMinion.SpellCasted > 0)
                return getPoints();
              /*  return (100-_turret.GetComponent<MinionHealth>().Statistics)
                    +(OffensiveGameManager.Instance.GameTimer-_turret.Timer)
                    +_controledMinion.GetComponent<MinionMana>().spellCasted*10+1
                    + _controledMinion.Timer-neuralError;*/
            return 0;
        }
    }
    public float getPoints()
    {
        move = 0;
        _teleTime = 0;

        float res = damageDealt + _controledMinion.SpellCasted/2 + 1;
        damageDealt = 0;
        if (res <= 0)
        {
            return _controledMinion.SpellCasted/20+1;
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

    public void Restart()
    {
        StopAllCoroutines();
        if (_controledMinion == null)
        {
            _controledMinion = Instantiate(_minionPrefab, Vector3.zero, Quaternion.identity, _board).GetComponent<Minion>();
            //_controledMinion.aiControling = this;
            _controledMinion.Restart(1);
        }
        else
        {
            //_controledMinion.aiControling = this;
            _controledMinion.Restart(1);
            _controledMinion.GetComponentInChildren<Renderer>().material.color = _neuralNetwork.Color;
        }
        _bd=_controledMinion.transform.parent.GetComponent<BoardDictionary>();
        if(_tv==null)
            _tv = _bd.Board.Select(b => b.Key).ToArray();
        _turret = _controledMinion.transform.parent.GetComponentsInChildren<Minion>().Where(m => m != _controledMinion).FirstOrDefault();
        _timer = 0;
        _controledMinion.GetComponent<MinionMana>().spellCasted = 0;
        neuralError = 0;
    }

    private void FixedUpdate()
    {
        _timer += Time.deltaTime;
        _teleTime+= Time.deltaTime; 
        if (IsAlive && _teleTime >= _askTeleTimer)
        {
            //Debug.Log("CASTING");

            move = NeatValues.rnd.Next(0, roadplan.Count-1);
            Spell spell = SpellFactory.GetSpell("Teleport");
            spell?.Cast(_turret, _tv[roadplan[move]]);
            _controledMinion.GetComponent<MinionMana>().Statistics = 100;
            move += 1;
            _teleTime = 0;
            //CalculateMove();

            /* if(_controledMinion.GetComponent<MinionMana>().spellCasted >= OffensiveGameManager.Instance.maxShoots)
             {
                 _controledMinion.GetComponent<MinionHealth>().DealDamage(100);
             }*/
        }
        
        
        if (IsAlive && !_controledMinion.IsDoingSomething && _timer>=_askTimer)
        {

            
            CalculateMove();
            _timer = 0;
           /* if(_controledMinion.GetComponent<MinionMana>().spellCasted >= OffensiveGameManager.Instance.maxShoots)
            {
                _controledMinion.GetComponent<MinionHealth>().DealDamage(100);
            }*/
        }

    }

    private void CalculateMove()
    {
        List<float> inputValue = new List<float>();
        //inputValue.AddRange(GetTableValues());

        var cmPos = _controledMinion.transform.position;
        
        var closest = Physics.OverlapBox(cmPos, new Vector3(7, 1, 7), Quaternion.identity).Where(c=>c.tag=="Attack").OrderBy(c => (c.transform.position - cmPos).sqrMagnitude).FirstOrDefault();
        if(closest!=null)
        {
            var vec = closest.transform.position - cmPos;
            var vec2 = closest.GetComponent<FireBall>().FlyingVector;
            var vecmag = vec.magnitude;
            Debug.DrawLine(cmPos, closest.transform.position, Color.red);
            //inputValue.Add(vecmag);

            float ivalue = Vector3.Dot(vec, vec2) / (vecmag * vec2.magnitude);
            if (Selection.activeGameObject == _controledMinion.gameObject)
            {
                //Debug.Log(ivalue);
            }
            //inputValue.Add(ivalue*10);
        }
        else
        {
            //inputValue.Add(-1);
            //inputValue.Add(0);
        }
        int pos2 = ((int)_turret.Position[0] - 65) * 7 + ((int)_turret.Position[1] - 49);
        int pos = ((int)_controledMinion.Position[0] - 65) * 7 + ((int)_controledMinion.Position[1] - 49);
        //inputValue.Add(_turret.Position);
        
        //inputValue.Add(pos);
        //inputValue.Add(_controledMinion.GetComponent<MinionMana>().Statistics);

        inputValue.Add(pos2);
        //inputValue.Add(_controledMinion.GetComponent<MinionMana>().lastCastTimer);
        var value = _neuralNetwork.CalculateNeuralNetworkValue(inputValue.ToArray());


        value[0] = Mathf.RoundToInt(value[0]);
        value[1] = Mathf.RoundToInt(value[1]);
        //Debug.Log(value[0] + " " + value[1]);
        
        if(value[1]>= _tv.Length)
        {
            value[0] = 0;
        }
        if(value[1]<=0)
        {
            value[0] = 0;
        }
        if(value[0]>2)
        {
            value[0] = 2;
        }
        if (value[0] <=0)
        {
            value[0] = 0;
        }
        if (Selection.activeGameObject == _controledMinion.gameObject)
        {
            string a = "";
            foreach (var inp in inputValue)
            {
                a += inp.ToString() + " ";
            }
            Debug.Log(a + " equals = " + value[0] + ", " + value[1]);
        }



        if (value[0] >= 1)
        {
            
            if (_controledMinion.GetComponent<MinionMana>().Statistics == 100)
            {
                _controledMinion.SpellCasted += 1;
                if (pos2 == (int)value[1])
                {
                    damageDealt += 20;
                    Debug.Log("HIT AT "+pos2);
                    _turret.GetComponent<MinionHealth>().DealDamage(20);
                    _controledMinion.Hitat.Add(pos2);
                }
                else
                {
                    if ((int)value[1] > 15)
                    {
                        //Debug.Log("shooting at AT " + (int)value[1]);
                    }
                   
                }
                _controledMinion.GetComponent<MinionMana>().BurnMana(100);
                //_controledMinion.SpellCasted += 1;
            }
            
           
        
            
            //Spell spell = SpellFactory.GetSpell("fireball");
            //spell?.Cast(_controledMinion, _tv[(int)value[1]]);
            
            if(_controledMinion.GetComponent<MinionMana>().lastCastTimer>=0.1f)
            {
                neuralError++;
            }
        }
        //else if(value[0] == 2)
        //{
        //    Spell spell = SpellFactory.GetSpell("Teleport");
        //    spell?.Cast(_controledMinion, _tv[(int)value[1]]);
        //}
    }

    private float[] GetTableValues()
    {
        float[] tableInfo = new float[_bd.Board.Count];
        //var vec = _controledMinion.transform.position
        int i = 0;
        foreach(var point in _bd.Board)
        {
            tableInfo[i++] = point.Value.GetComponent<PointInfo>().AiInfo;
        }
        return tableInfo;
    }
}
