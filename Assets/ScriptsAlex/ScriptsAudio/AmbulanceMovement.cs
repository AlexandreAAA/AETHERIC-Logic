using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceMovement : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _maxDistance;

    [SerializeField]
    private float _minDistance;

    #endregion


    void Start()
    {
        _ambulanceRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PushForward();
        
            if (transform.position.z <= _minDistance && ! _isFacingNorth|| transform.position.z >= _maxDistance && _isFacingNorth)
            {
                UTurn();
                _isFacingNorth = !_isFacingNorth;
            }
        
    }

    #region Main Method

    private void PushForward()
    {
        _ambulanceRb.velocity = transform.forward * _speed;
    }

    private void UTurn()
    {
        transform.Rotate(0, 180, 0);
        PushForward();
    }

    #endregion


    #region Privates

    private bool _isFacingNorth = true;

    private Rigidbody _ambulanceRb;

    #endregion
}
