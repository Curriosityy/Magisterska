using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControler : MonoBehaviour
{
    [SerializeField]private NeuralNetwork _neuralNetwork;

    [SerializeField] bool _isAlive=true;
    [SerializeField] float speed = 1;
    [SerializeField] float distToObstacle=5;
    [SerializeField] float jumpExh = 1;
    [SerializeField] int _points = 0;
    bool canJump = true;
    public NeuralNetwork NeuralNetwork { get => _neuralNetwork; set => _neuralNetwork = value; }
    public int Points { get => _points; }
    public bool IsAlive { get => _isAlive; }

    public void Restart()
    {
        Debug.Log("Restart");
        StopAllCoroutines();
        _isAlive = true;
        distToObstacle = 5;
        _points = 0;
        canJump = true;
    }

    private void Update()
    {
        if(IsAlive)
        {
            distToObstacle -= speed * Time.deltaTime;
            CalculateMove();
            if (distToObstacle <= 0)
            {
                _isAlive = false;
                _points -= 10;
            }

        }

    }


    

    void Jump()
    {
        if (canJump)
        {
            Debug.Log("Jump");
            if (distToObstacle <= 1f && distToObstacle >= 0.5f)
            {
                distToObstacle += 5;
                _points += 10;
            }
            else
            {
                _points -= 1;
            }
            StartCoroutine(JumpExhaust());
        }  
    }

    private void CalculateMove()
    {
        var value = _neuralNetwork.CalculateNeuralNetworkValue(distToObstacle);
        if (value >= 1)
            Jump();
    }


    IEnumerator JumpExhaust()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpExh);
        canJump = true;
    }


}
