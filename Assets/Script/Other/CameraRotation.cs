using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    #region Unity API

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        _orientationInput = new Vector3(Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), _orientationInput.z);
        transform.Rotate(_orientationInput);
    }

    #endregion

    #region Privates

    private Vector3 _orientationInput;
    private Quaternion _lookRotation;

    #endregion
}
