using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDetectCone : MonoBehaviour
{
    #region Exposed

    public float _resetTimer = 5f;
    public Light _coneLight;
    [SerializeField]
    private LayerMask _playerLayer;



    #endregion


    #region Unity API


    private void Start()
    {
        //_camTransform = GetComponent<Transform>();
        _aiSensor = GetComponent<AiSensors>();

    }

    void Update()
    {
        AlertTrigger();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RaycastHit _hit;
            if (Physics.Raycast(_camTransform.position, other.transform.position - _camTransform.position, out _hit, 100f, _playerLayer))
            {
                Debug.Log(_hit.collider.gameObject);

                if (_hit.collider == null)
                {
                    return;
                }
                else if (_hit.collider.gameObject.CompareTag("Player"))
                {
                    _alerting = true;
                    _alertTime = _resetTimer;
                }
            }
            else
            {
                return;
            }



            if (_alerting)
            {
                _lastKnownPos = other.transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _alerting = false;
        }
    }

    #endregion


    #region Main Method

    private void AlertTrigger()
    {
        if (_alerting)
        {
            _scanPlayer = true;
            _lastKnownPos = _playerTransform.position;
            _playerData.m_timeToRaiseAlert = _playerData.m_defaultAlertTime;

        }

        if (_scanPlayer)
        {
            _playerData.m_alert = true;
            _coneLight.color = Color.red;
            _alertTime -= Time.deltaTime;

            if (_alertTime < 0f)
            {
                _scanPlayer = false;
            }
        }
        else
        {
            _coneLight.color = Color.green;
        }
    }
    #endregion


    #region Privates

    public Transform _camTransform;

    public bool _alerting = false;

    private float _alertTime;

    public bool _scanPlayer = false;

    public Vector3 _lastKnownPos;

    private AiSensors _aiSensor;
    [SerializeField]
    private CharacterData _playerData;
    [SerializeField]
    private Transform _playerTransform;

    #endregion
}
