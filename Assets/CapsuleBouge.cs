using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleBouge : MonoBehaviour
{
    
    public float _moveSpeed;

    void Start()
    {
        _capsuleRb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {

        float _xHor = Input.GetAxis("Horizontal");
        float _zVer = Input.GetAxis("Vertical");

        _dirInput = new Vector3(_xHor, 0, _zVer);
        _dirInput.Normalize();
    }

    private void FixedUpdate()
    {
        float _step = _moveSpeed * Time.deltaTime;
        _capsuleRb.velocity = new Vector3(_dirInput.x * _step, _capsuleRb.velocity.y, _dirInput.z * _step);
    }

    private Vector3 _dirInput;
    private Rigidbody _capsuleRb;
}
