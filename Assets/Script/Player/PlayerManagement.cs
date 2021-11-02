using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class PlayerManagement : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private CharacterData _playerData;
    [SerializeField]
    private FloatReference _playerStartHp;
    [SerializeField]
    private FloatReference _playerCurrentHp;
    [SerializeField]
    private FloatReference _alertTime;
    [SerializeField]
    private BoolReference _isOnAlert;

    #endregion


    #region Unity API

    private void Start()
    {
        _playerData.m_currentHp = _playerData.m_totalHp;

        _playerData._blueKey = false;
        _playerData._yellowKey = false;
        _playerData._greenKey = false;
    }

    private void Update()
    {
        _playerStartHp.Value = _playerData.m_totalHp;
        _playerCurrentHp.Value = _playerData.m_currentHp;
        _alertTime.Value = _playerData.m_timeToRaiseAlert;
        _isOnAlert.Value = _playerData.m_alert;

        if (_playerData.m_detected)
        {
            _playerData.m_lastKnownPosition = transform.position;
            _playerData.m_timeToRaiseAlert = _playerData.m_defaultAlertTime;
            _playerData.m_alert = true;
        }

        if (_playerData.m_alert)
        {
            _playerData.m_timeToRaiseAlert -= Time.deltaTime;

            if (_playerData.m_timeToRaiseAlert < 0)
            {
                _playerData.m_alert = false;
            }
        }

        if (!_playerData.m_alert)
        {
            //_playerData.m_timeToRaiseAlert = _playerData.m_defaultAlertTime;
        }

    }

    

    #endregion
}
