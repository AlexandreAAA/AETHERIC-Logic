using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoForward : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private GameObject _bigExplosion;

    #endregion


    #region Unity API
    void Start()
    {
        _fireballRb = GetComponent<Rigidbody>();
    }


    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 velocity = transform.forward * _fireBallSpeed * Time.fixedDeltaTime;
        Vector3 _newPos = transform.position + velocity;
        _fireballRb.MovePosition(_newPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Instantiate(_bigExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }


    }
    #endregion


    #region MainMethod

    public void _setFireballSpeed(float _speed)
    {
        _fireBallSpeed = _speed;
    }

    #endregion


    #region Privates

    private Rigidbody _fireballRb;
    private float _fireBallSpeed;

    #endregion
}
