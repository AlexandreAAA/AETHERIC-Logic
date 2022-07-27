using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionSoldier : MonoBehaviour
{

    #region Exposed

    [SerializeField]
    private float _rayCastLength;

    [SerializeField]
    private Transform _headTransform;

    public bool _detect = false;


    #endregion

    void Start()
    {
        
    }

    
    void Update()
    {
        Ray _ray = new Ray(_headTransform.position + _offset, _headTransform.forward);
        Debug.DrawRay(_ray.origin, _ray.direction * _rayCastLength, Color.magenta);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Ray _ray = new Ray(_headTransform.position + _offset, _headTransform.forward);
            Debug.DrawRay(_ray.origin, _ray.direction * _rayCastLength, Color.magenta);
            RaycastHit _hit;

            if (Physics.Raycast(_ray, out _hit))
            {
                Debug.Log("I'm looking at " + _hit.transform.name);
            }

            //if (Physics.Raycast(_ray, out _hit, _rayCastLength))
            //{
            //    if (_hit.collider.CompareTag("Player"))
            //    {
            //        SendMessageUpwards("PlayerDetect");
            //        _detect = true;
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
        }
    }


    #region Privates

    private Vector3 _offset = new Vector3(0, 0, 1f);

    #endregion
}
