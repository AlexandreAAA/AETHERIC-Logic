using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    #region Exposed

    public float m_mouseSensitivity = 100f;
    public Transform m_playerBody;

    #endregion


    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float _mouseX = Input.GetAxis("Mouse X") * m_mouseSensitivity * Time.deltaTime;
        float _mouseY = Input.GetAxis("Mouse Y") * m_mouseSensitivity * Time.deltaTime;

        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        m_playerBody.Rotate(Vector3.up * _mouseX);
    }

    #region Privates

    private float _xRotation = 0f;

    #endregion
}
