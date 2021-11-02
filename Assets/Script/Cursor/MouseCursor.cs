using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public PlayerCasting _playerCasting;

    void Start()
    {
        Cursor.visible = true;
        _mainCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 _mousePos = _playerCasting._lookedAtPoint;
        transform.position = _mousePos;
    }

    private Camera _mainCamera;
}
