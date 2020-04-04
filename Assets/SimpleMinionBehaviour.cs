using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMinionBehaviour : MonoBehaviour
{

    [SerializeField] float _speed = 3;
    [SerializeField] float _jumpHeight = 4;
    [SerializeField] bool _isJumping = false;
    float _points;
    bool _isAlive = true;
    public float Points { get => _points;}
    public bool IsAlive { get => _isAlive; set => _isAlive = value; }
    public bool IsJumping { get => _isJumping; set => _isJumping = value; }

    // Start is called before the first frame update
    void Start()
    {
        _jumpHeight = 4;
        _speed = 5;
        _points = 0;
        _isAlive = true;
        _isJumping = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag== "Obstacle")
            Die();
        _isJumping = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="PointAdder")
        {
            _points += 10;
        }
    }
    public float GetDistanceToNextObstacle()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,Vector3.right,out hit,Mathf.Infinity,LayerMask.GetMask("Default")))
        {
            if(hit.transform.tag=="Obstacle")
            {
                return Vector3.SqrMagnitude(transform.position - hit.transform.position);
            }
        }
        return 100;
        
    }

    public void Die()
    {
        _points -= 20;
        _isAlive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isAlive)
        {
            _points += Time.deltaTime;
            transform.Translate(_speed * Time.deltaTime, 0, 0);
        }
    }

    public void Jump()
    {
        if(!_isJumping)
        {
            Debug.Log("Jump");
            GetComponent<Rigidbody>().AddForce(new Vector3(0, _jumpHeight*100, 0));
            _isJumping = true;
        }
    }
}
