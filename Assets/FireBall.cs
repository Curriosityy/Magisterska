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
    public void Initialize(Vector3 position)
    {
        _positionToFly = position;
        StartCoroutine("FlyToPosition");
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(string.Format("{0} Collided",gameObject.name));
        Explode();
    }
    private void Explode()
    {
        //TODO Cast some nice explosion effect.
        var entityToDamage = Physics.SphereCastAll(transform.position, _expRange, Vector3.zero)
            .Where(entity => entity.collider.GetComponent<IDamageable>() != null)
            .Select(entity => entity.collider.GetComponent<IDamageable>());
        foreach (var entity in entityToDamage)
        {
            entity.DealDamage(_damage);
        }
        Destroy(gameObject);
    }
    IEnumerator FlyToPosition()
    {
        Debug.Log(string.Format("{0} FlyToPosition", gameObject.name));
        Vector3 oldPos = transform.position;
        _timer = 0;
        while(_positionToFly!=transform.position)
        {
            _timer += _speed * Time.deltaTime;
            transform.position = Vector3.Lerp(oldPos, _positionToFly, _timer);
            yield return null;
            
        }
        Explode();
        yield return null;
    }
}
