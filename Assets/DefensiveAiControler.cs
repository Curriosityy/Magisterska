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
            Debug.DrawLine(cmPos, closest.transform.position, Color.red);
            inputValue.Add(vec.sqrMagnitude);
            inputValue.Add(Mathf.Atan2(vec.z, vec.x));
        }
        else
        {
            inputValue.Add(float.PositiveInfinity);
            inputValue.Add(float.PositiveInfinity);
        }
        
        var value = _neuralNetwork.CalculateNeuralNetworkValue(inputValue.ToArray());

        value[0] = Mathf.RoundToInt(value[0]);
        value[1] = Mathf.RoundToInt(value[1]);

        //Debug.Log(value[0] + " " + value[1]);
        if(value[1]>= _tv.Length)
        {
            value[1] = _tv.Length - 1;
        }
        if(value[1]<0)
        {
            value[1] = 0;
        }
        if(value[0] >= 1)
        {
            Spell spell = SpellFactory.GetSpell("walk");
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
