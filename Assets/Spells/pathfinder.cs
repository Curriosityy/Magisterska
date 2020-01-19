using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathfinder : MonoBehaviour
{




    void Start()
    {

    }
    public static void findpath(GameObject targetLocation, GameObject startingPosition)
    {
        List<GameObject> openlist = new List<GameObject>();
        List<GameObject> closedlist = new List<GameObject>();
        List<GameObject> path = new List<GameObject>();
        startingPosition.GetComponent<ss>().g = 0;
        openlist.Add(startingPosition);
        GameObject[,] array;
        int size = GameObject.FindObjectOfType<BoardDictionary>().size;
        //Debug.Log(size);
        array = new GameObject[size, size];
        int unicode;

        for (int i = 0; i < size; i += 1)
        {
            unicode = 65 + i;
            for (int j = 1; j < size + 1; j += 1)
            {
                char character = (char)unicode;
                string text = character.ToString();
                var temp = Object.FindObjectOfType<BoardDictionary>().Board[text + System.Convert.ToString(j)];
                array[i, j - 1] = temp;
                array[i, j - 1].GetComponent<ss>().setDist(targetLocation);
                array[i, j - 1].GetComponent<ss>().x = i;
                array[i, j - 1].GetComponent<ss>().y = j - 1;
                //bug.Log(array[i, j - 1].name);
            }
        }
        
        GameObject currentbest = startingPosition;
        while (currentbest.name != targetLocation.name)
        {
            List<GameObject> neighbours = new List<GameObject>();
            neighbours = getNeighbours(array, currentbest.GetComponent<ss>().x, currentbest.GetComponent<ss>().y, size);
            for (int i = 0; i < neighbours.Count; i += 1)
            {
                string neighbourname = neighbours[i].name;
                if (!ischecked(openlist, closedlist, neighbourname))
                {
                    openlist.Add(neighbours[i]);
                }
            }
            GameObject tempc = currentbest;
            openlist.Remove(currentbest);
            closedlist.Add(tempc);
            evaluateroute(openlist, tempc);
            currentbest= openlist[selectBest(openlist)];
            //Debug.Log("Current best:"+ currentbest.name);
        }
        path=recreatepath(targetLocation.name,startingPosition.name);
       
        //geting neighbours
        
    }
    static List<GameObject> recreatepath(string last,string first)
    {
       
        List<GameObject> path = new List<GameObject>();
        string curtile = last;
        while (curtile!="A3")
        {
            GameObject ctile = Object.FindObjectOfType<BoardDictionary>().Board[curtile];
            path.Add(ctile);
            Debug.Log("Current best:" + curtile);

            curtile = ctile.GetComponent<ss>().camefrom.name;
        }
        path.Add(Object.FindObjectOfType<BoardDictionary>().Board[first]);
        return path;

    }
    static void evaluateroute(List<GameObject> olist,GameObject curb)
    {
        int g =curb.GetComponent<ss>().g;
        for (int i=0;i< olist.Count;i+=1)
        {
            if (olist[i].GetComponent<ss>().g > g+10)
            {
                olist[i].GetComponent<ss>().camefrom = curb;
                olist[i].GetComponent<ss>().g = g + 10;
                olist[i].GetComponent<ss>().f = olist[i].GetComponent<ss>().g + olist[i].GetComponent<ss>().h;
                Debug.Log(olist[i].GetComponent<ss>().g);
            }
        }
    }
    
    static int selectBest(List<GameObject> olist)
    {
        int curbest=int.MaxValue;
        float fscore=int.MaxValue;
        for(int i = 0; i < olist.Count; i += 1)
        {
            if (olist[i].GetComponent<ss>().f<fscore)
            {
                curbest = i;
                fscore = olist[i].GetComponent<ss>().f;
            }
        }
        return curbest;
    }
    

    // Update is called once per frame
    public static List<GameObject> getNeighbours(GameObject[,] array, int x, int y, int asize)
    {
        List<GameObject> neighbours = new List<GameObject>();
        if (x > 0 && array[x - 1, y].GetComponent<ss>().walkable)
        {
            neighbours.Add(array[x - 1, y]);
        }
        if (x < asize- 1 && array[x + 1, y].GetComponent<ss>().walkable)
        {
            neighbours.Add(array[x + 1, y]);
        }
        if (y > 0 && array[x, y - 1].GetComponent<ss>().walkable)
        {
            neighbours.Add(array[x, y - 1]);
        }
        if (y < asize- 1 && array[x, y + 1].GetComponent<ss>().walkable)
        {
            neighbours.Add(array[x, y + 1]);
        }
        return neighbours;
    }
    static bool ischecked(List<GameObject> olist, List<GameObject> clist,string name)
    {
        for (int i=0;i<olist.Count;i+=1)
        {
            if (olist[i].name == name)
            {
                return true;
            }
            
        }
        for (int i = 0; i < clist.Count; i += 1)
        {

            if (clist[i].name == name)
            {
                return true;
            }
        }
        return false;
    }
}
