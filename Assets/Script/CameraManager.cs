using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Exposed

    private Camera _camera;

    #endregion

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    void Start()
    {
       // _camera.cullingMask()
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
