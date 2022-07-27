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
    private void Start()
    {
        _fireballRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _fireBallSpeed * Time.fixedDeltaTime * transform.forward;
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

    public void SetFireballSpeed(float _speed)
    {
        _fireBallSpeed = _speed;
    }

    #endregion

    #region Privates

    private Rigidbody _fireballRb;
    private float _fireBallSpeed;

    #endregion
}
