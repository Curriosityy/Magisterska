using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FireBall : MonoBehaviour
{
    [SerializeField] private int _damage=1;
    [SerializeField] private float _expRange=1f;
    [SerializeField] private float _speed=1f;
    private float _timer = 0;
    Vector3 _positionToFly;
    private Vector3 _flyingVector;
    private Minion _caster;

    public Vector3 FlyingVector { get => _flyingVector; }
    public Minion Caster { get => _caster;}

    public void Initialize(Vector3 position,Minion caster)
    {
        _positionToFly = position;
        _flyingVector = (position - transform.position).normalized;
        StartCoroutine("FlyToPosition");
        _caster = caster;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(string.Format("{0} Collided with {1}",gameObject.name,collision.collider.name));
        Explode();
    }
    private void Explode()
    {
        //TODO Cast some nice explosion effect.
        var entityToDamage = Physics.OverlapSphere(transform.position, _expRange)
            .Where(entity => entity.gameObject.GetComponent<IDamageable>() != null)
            .Select(entity => entity.gameObject.GetComponent<IDamageable>());
        foreach (var entity in entityToDamage)
        {
            entity.DealDamage(_damage);
        }
        transform.position = new Vector3(0, 100000, 0);
       
        Destroy(gameObject,0.3f);
    }
    IEnumerator FlyToPosition()
    {
        //Debug.Log(string.Format("{0} FlyToPosition {1}", gameObject.name,_positionToFly));
        Vector3 oldPos = transform.position;
        _timer = 0;
        while(_positionToFly!=transform.position)
        {
            //_timer += ;
            transform.position = Vector3.MoveTowards(transform.position, _positionToFly, _speed*Time.deltaTime);
            yield return null;
            
        }
        Explode();
        yield return null;
    }
}
