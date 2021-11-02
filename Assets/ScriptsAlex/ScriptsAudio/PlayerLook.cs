using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private Vector2 _mouseSensitivity;

    [SerializeField]
    private Vector2 _padSensitivity;

    [SerializeField]
    private Vector2 _mouseYLimit;

    
    public Transform _playerBody;


    #endregion
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float _mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity.x * Time.deltaTime;
        float _mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity.y * Time.deltaTime;

        float _gamePadX = Input.GetAxis("RightHorizontal") * _padSensitivity.x * Time.deltaTime;
        float _gamePadY = Input.GetAxis("RightVertcial") * _padSensitivity.y * Time.deltaTime;

        _horizontal = _mouseX;
        _vertical = _mouseY;
        _vertical = Mathf.Clamp(_vertical, -90, 90);

        transform.Rotate(0, _horizontal, 0);
        Camera.main.transform.Rotate( _vertical, 0, 0);
    }

    #region Privates

    private float _horizontal;

    private float _vertical;

    #endregion
}
