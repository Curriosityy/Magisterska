using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    public GameObject obstacle;
    [SerializeField] private List< BoardDictionary> boardsList = new List<BoardDictionary>();
    private Dictionary<string, GameObject> _board;
    private string[] _tv;
    private BoardDictionary _bd;
    private List<GameObject> obstacles;
   
    // Start is called before the first frame update

    void Start()
    {

        obstacles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void getBoard()
    {
      
        if (_tv == null)
        {
            print("DUUUPAAA");
            _tv = boardsList[0].Board.Select(b => b.Key).ToArray();

            print(_tv.Length);
        }
           

    }
    public void spawnObstacles()
    {
        getBoard();
        foreach (BoardDictionary board in boardsList)
        {
            int spawned = 0;
            while (spawned < NeatValues.obstaclesAmout)
            {
                int rnd = NeatValues.rnd.Next(0, _tv.Length);
                PointInfo pI = board.Board[_tv[rnd]].GetComponent<PointInfo>();
                if (pI.Walkable)
                {
                   // temp.transform.position = _board[_spawningPoint1].transform.position;
                   // temp.transform.parent = transform;
                    Vector3 spawnPosition = new Vector3(2,board.transform.position.y,-2);
                    GameObject gobs= Instantiate(obstacle, spawnPosition,Quaternion.Euler(0,0,0), board.transform);

                    gobs.transform.position = board.Board[_tv[rnd]].transform.position+new Vector3(0,1f,0);
                    spawned += 1;
                    //obstacles.Add(gobs);

                }
            }
            spawned = 0;
        }

        /*if (_tv != null)
        {
            for (int i = 0; i < NeatValues.obstaclesAmout; i += 1)
            {
                int s=_bd.Board["a1"].GetComponent<PointInfo>().G;
                
            }
        }*/
    }
    public void deleteObstacles()
    {
        foreach(GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
    }

    public void addDictionary(BoardDictionary bd)
    {
        boardsList.Add(bd);
        print(boardsList.Count);
    }
}
