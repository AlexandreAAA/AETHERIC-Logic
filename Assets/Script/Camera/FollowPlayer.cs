using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    #region exposed

    public Transform m_playerTransform;
    public float m_smoothSpeed;

    #endregion
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, m_playerTransform.position + _offset, ref _velocity, m_smoothSpeed);
        //transform.position = m_playerTransform.position + _offset;
        
    }

    private void LateUpdate()
    {
    }

    #region Privates

    public Vector3 _offset = new Vector3(0, 13, -5);
    private Vector3 _velocity = Vector3.zero;
    

    #endregion
}
