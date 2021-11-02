using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotaMoveTest : MonoBehaviour
{
    public float _moveSpeed;
    public float _rotationSpeed;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");
        _movementDirection = new Vector3(_horizontal, 0f, _vertical);
        _movementDirection.Normalize();

        if (_movementDirection != Vector3.zero)
        {
            //transform.forward = _movementDirection;
            Quaternion _toRotation = Quaternion.LookRotation(_movementDirection, Vector3.up);
            _lookRotation = Quaternion.RotateTowards(_rb.rotation, _toRotation, _rotationSpeed * Time.deltaTime);
        }

        //transform.Translate(_movementDirection * _moveSpeed * Time.deltaTime, Space.World);
    }

    private void FixedUpdate()
    {
        float _velocitySpeed = _moveSpeed * Time.deltaTime;
        _rb.velocity = new Vector3(_movementDirection.x * _velocitySpeed, _rb.velocity.y, _movementDirection.z * _velocitySpeed);
        _rb.MoveRotation(_lookRotation.normalized);
    }

    private Vector3 _movementDirection;
    private Rigidbody _rb;
    private Quaternion _lookRotation;
}
