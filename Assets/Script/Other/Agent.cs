using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    #region Exposed

    public Transform[] m_wayPoints;
    public bool _isAuto = false;

    [SerializeField]
    private Vector3 velocity;

    [SerializeField]
    private float _normalSpeed;

    [SerializeField]
    private float _attackStateSpeed;

    public float velo;

    public AiSensors m_sensor;

    [SerializeField]
    private float _resumePatrol = 3f;

    public GameObject m_camDetects;

    [SerializeField]
    private bool _playerDetection = false;

    public float _alertTime;

    public bool _attack = false;

    public bool _alert = false;

    public bool _searchAndDestroy = false;

    #endregion


    #region Unity API

    void Start()
    {
        _navTest = GetComponent<NavMeshAgent>();
        _soldierRigidbody = GetComponent<Rigidbody>();
        _soldierAnim = GetComponent<Animator>();
        _coneLight = GetComponentInChildren<Light>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_sensor = GetComponent<AiSensors>();

    }

    void Update()
    {
        Patrolling();

        velocity = _navTest.velocity;

        _soldierAnim.SetFloat("Velocity", velocity.magnitude);

        if (m_sensor.m_isInsight(_playerTransform.gameObject))
        {
            _playerDetection = true;
            _attack = true;
            _alertTime = _resumePatrol;

        }
        else
        {
            _playerDetection = false;
        }

        if (m_camDetects.GetComponent<CamDetect>()._alerting)
        {
            _alert = true;
            _searchAndDestroy = true;
            _alertTime = 5f;
        }
        else
        {
            _alert = false;
        }


        if (_attack)
        {
            _isAuto = false;
            _navTest.speed = _attackStateSpeed;
            _navTest.SetDestination(_playerTransform.position);
            _coneLight.color = Color.red;

            if (!_playerDetection)
            {
                _alertTime -= Time.deltaTime;

                if (_alertTime < 0f)
                {
                    _attack = false;
                }
            }
        }
        else if (_searchAndDestroy)
        {
            _isAuto = false;
            Vector3 _lastKnowPos = m_camDetects.GetComponent<CamDetect>()._lastKnownPos;
            _coneLight.color = Color.blue;
            _navTest.SetDestination(_lastKnowPos);

            if (!_alert)
            {
                _alertTime -= Time.deltaTime;

                if (_alertTime < 0f)
                {
                    _searchAndDestroy = false;
                }
            }
        }
        else
        {
            _isAuto = true;
            _navTest.speed = _normalSpeed;
            _coneLight.color = Color.green;
        }

        //
    }

    #endregion;


    #region Main Method

    void Patrolling()
    {
        if (_isAuto)
        {
            if (!_navTest.pathPending && _navTest.remainingDistance < 0.5f) 
            { 
                if (m_wayPoints.Length == 0)
                {
                    return;
                }

            _navTest.destination = m_wayPoints[_destPoint].position;

            _destPoint = (_destPoint + 1) % m_wayPoints.Length;
            }
        }
    }

    #endregion


    #region Privates

    private NavMeshAgent _navTest;

    private Rigidbody _soldierRigidbody;

    private Animator _soldierAnim;

    private Light _coneLight;

    private Transform _playerTransform;

    private int _destPoint = 0;

    #endregion
}
