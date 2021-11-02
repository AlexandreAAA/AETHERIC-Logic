using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour, IUsable
{

    public CharacterData _playerData;
    public bool _isGreen;
    public bool _isYellow;
    public bool _isBlue;
    public bool _isKey = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        if (_isBlue)
        {
            _playerData._blueKey = true;
            Destroy(gameObject);
        }
        else if (_isGreen)
        {
            _playerData._greenKey = true;
            Destroy(gameObject);
        }
        else if (_isYellow)
        {
            _playerData._yellowKey = true;
            Destroy(gameObject);
        }
        else
        {
            return;
        }
    }
}
