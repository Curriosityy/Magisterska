using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
public class DefensiveAiControler : MonoBehaviour
{
    [SerializeField] private NeuralNetwork _neuralNetwork;
    [SerializeField] private GameObject _minionPrefab;
    private string[] _tv;
    private BoardDictionary _bd;
    private Minion _controledMinion;
    public Transform spawnPoint;
    private Transform _board;
    private Minion _turret;

    public void AssignToBoard(Transform board)
    {
        _board = board;
    }
    public NeuralNetwork NeuralNetwork { get => _neuralNetwork; set => _neuralNetwork = value; }
    public float Points
    {
        get
        {
            if (_controledMinion != null)
                return _controledMinion.Points;
            return 0;
        }
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
    }

    private void Update()
    {
        if (IsAlive && !_controledMinion.IsDoingSomething)
        {
            CalculateMove();
        }

    }

    private void CalculateMove()
    {
        List<float> inputValue = new List<float>();
        inputValue.AddRange(GetTableValues());

        var cmPos = _controledMinion.transform.position;
        
        var closest = Physics.OverlapBox(cmPos, new Vector3(7, 1, 7), Quaternion.identity).Where(c=>c.tag=="Attack").OrderBy(c => (c.transform.position - cmPos).sqrMagnitude).FirstOrDefault();
        if(closest!=null)
        {
            var vec = closest.transform.position - cmPos;
            var vec2 = closest.GetComponent<FireBall>().FlyingVector;
            var vecmag = vec.magnitude;
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
            inputValue.Add(float.PositiveInfinity);
            inputValue.Add(float.PositiveInfinity);
        }
        int pos2 = ((int)_turret.Position[0] - 65) + (int)_turret.Position[1] - 49;
        int pos = ((int)_controledMinion.Position[0] - 65) + (int)_controledMinion.Position[1] - 49;
        //inputValue.Add(_turret.Position);
        
        inputValue.Add(pos);
        inputValue.Add(_controledMinion.GetComponent<MinionMana>().Statistics);

        inputValue.Add(pos2);

        var value = _neuralNetwork.CalculateNeuralNetworkValue(inputValue.ToArray());


        value[0] = Mathf.RoundToInt(value[0]);
        value[1] = Mathf.RoundToInt(value[1]);
        //Debug.Log(value[0] + " " + value[1]);
        
        if(value[1]>= _tv.Length)
        {
            value[1] = _tv.Length - 1;
        }
        if(value[1]<=0)
        {
            value[1] = 0;
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



        if (value[0] == 1)
        {
            _controledMinion.steps += 1;
            Spell spell = SpellFactory.GetSpell("walk");
            spell?.Cast(_controledMinion, _tv[(int)value[1]]);
        }else
        if(value[0] == 2)
        {
            _controledMinion.steps += 3;
            Spell spell = SpellFactory.GetSpell("Teleport");
            spell?.Cast(_controledMinion, _tv[(int)value[1]]);
        }
        

        //_controledMinion.GetDistanceToNextObstacle(out inputValue);
        //var value = _neuralNetwork.CalculateNeuralNetworkValue(inputValue);

        //if (Selection.activeGameObject == _controledMinion.gameObject)
        //{
        //    Debug.Log(inputValue[0] + ", " + inputValue[1] + " equals = " + value);
        //}
        //value = Mathf.RoundToInt(value);
        //if (Mathf.RoundToInt(value) >= 1)
        //   _controledMinion.Jump();
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
