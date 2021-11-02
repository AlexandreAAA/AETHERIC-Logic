using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetection : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private CharacterData _playerData;
    public bool _hasHeardPlayer;
    public Vector3 _lastHeardPos;
    

    #endregion


    #region Unity API

    private void Start()
    {
        _playerController = GameObject.Find("PlayerOKV1").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_playerController.m_isCrouch)
        {
            _hasHeardPlayer = true;
            _lastHeardPos = other.transform.position;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _hasHeardPlayer = false;
            
        }
    }

    #endregion


    private PlayerController _playerController;
}
