using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnnemyState
{
    IDLE,
    PATROL,
    ALERT,
    ATTACK,
    HIT,
    STUN,
    DEAD
}

public class EnnemyBehaviour : MonoBehaviour
{

    #region Exposed

    [Header("Data")]
    [SerializeField]
    private CharacterData _playerData;
    [SerializeField]
    private EnnemyData _sentryData;
    public int m_sentryCurrentHp;
    [SerializeField]
    private MeleeAttackState _meleeAttackState;
    [SerializeField]
    private SoundDetection _soundDetection;

    [Header("Flags")]
    public bool m_isOnPatrol;
    public bool m_isAlerted;
    public bool m_isAttacking;
    public bool m_isHit;
    public bool m_isStun;
    public bool m_canCheck = true;
    public bool m_check = false;
    public bool m_hasWeapon = false;
    public bool m_playerDetectedBySentry;
    public bool _canTalk = true;

    [Header("Patrol")]
    [SerializeField]
    private Transform[] _wayPoints;     //Patrol Waypoints
    public int _waypointTocheck;        //Waypoint to check during Patrol
    public float m_timeTocheck = 3f;    //Time of the check
    [SerializeField]
    private float _checkTime;            //Debug var
    public float m_patrolSpeed;         //Movement speed of Patrol

    [Header("Alert")]
    public GameObject _weapon;
    [SerializeField]
    private Vector3 _lastKnownPos;
    [SerializeField]
    private float _alertSpeed = 3f;
    [SerializeField]
    private Vector3 _offsetCheck;

    [Header("Detection")]
    [SerializeField]
    private AiSensors _aiSensors;
    [SerializeField]
    private Transform _playerTransform;
    public float m_timeUntilAlert = 3f;
    [SerializeField]
    private float _attackStateTimer;
    [SerializeField]
    private CamDetect[] _linkCameras;
    [SerializeField]
    private CamDetectCone[] _linkConeCameras;

    [Header("On Hit Parameters")]
    public float m_timeToBeHitAgain = 0.5f;
    [SerializeField]
    private float _hitTime;
    private bool _isHitByFireBall;

    [Header("Stun Parameter")]
    public bool _canBeStun;
    public float _timeOfStun = 10f;
    [SerializeField]
    private float _stunTime;
    public float _timeToBeActive = 5f;
    [SerializeField]
    private float _activeTime;

    #endregion


    #region Unity API

    private void Awake()
    {
        _ennemyNav = GetComponent<NavMeshAgent>();
        _ennemyRigidbody = GetComponent<Rigidbody>();
        _ennemyAnim = GetComponent<Animator>();

    }

    private void Start()
    {
        TransitionToState(EnnemyState.IDLE);
        _ennemyNav.updatePosition = false;
        _checkTime = m_timeTocheck;
        m_sentryCurrentHp = _sentryData.m_totalSentryHp;
        _meleeAttackState = GetComponent<MeleeAttackState>();
        _soundDetection = GetComponentInChildren<SoundDetection>();
        _sentryAudio = GetComponent<SentryAudio>();


    }

    private void Update()
    {
        OnStateUpdate();

        HandleAnimations();

        m_playerDetectedBySentry = _aiSensors.m_isInsight(_playerTransform.gameObject) && _playerData.m_currentHp > 0f;

        if (m_playerDetectedBySentry || m_isHit)
        {
            m_isAttacking = true;
            _attackStateTimer = m_timeUntilAlert;
            m_isAlerted = false;
            _playerData.m_timeToRaiseAlert = _playerData.m_defaultAlertTime;
        }

        if (!m_playerDetectedBySentry)
        {
            if (m_isAttacking)
            {
                _lastKnownPos = _playerTransform.position;
                _playerData.m_alert = true;
                _attackStateTimer -= Time.deltaTime;
            }

            if (_attackStateTimer < 0f)
            {
                m_isAttacking = false;
            }
        }

        if (_soundDetection._hasHeardPlayer)
        {
            _lastKnownPos = _playerTransform.position;
            m_isAlerted = true;
            _playerData.m_alert = true;
            _playerData.m_timeToRaiseAlert = _playerData.m_defaultAlertTime;
        }

        foreach (CamDetect _sensor in _linkCameras)
        {
            if (_sensor._alerting)
            {
                m_isAlerted = true;
                _lastKnownPos = _sensor._lastKnownPos;
            }
        }

        foreach (CamDetectCone _cone in _linkConeCameras)
        {
            if (_cone._alerting)
            {
                m_isAlerted = true;
                _lastKnownPos = _cone._lastKnownPos;
            }
        }

    }

    private void OnGUI()
    {
        GUILayout.Button(_currentState.ToString());
    }

    private void OnAnimatorMove()
    {
        transform.position = _ennemyNav.nextPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EveFoot") && !_meleeAttackState.m_isInvincible)
        {
            m_isHit = true;
        }

        if (other.CompareTag("EveFireBall") && !_meleeAttackState.m_isInvincible)
        {
            m_isHit = true;
            _isHitByFireBall = true;
            Debug.Log("Hit");
        }

        if (other.CompareTag("EveFist") && _canBeStun)
        {
            m_isStun = true;
        }
    }

    #endregion


    #region STATE MACHINE

    private void OnstateEnter()
    {
        switch (_currentState)
        {
            case EnnemyState.IDLE:

                _weapon.SetActive(false);
                m_hasWeapon = false;
                _canBeStun = true;

                break;
            case EnnemyState.PATROL:

                _canBeStun = true;
                m_isOnPatrol = true;
                _weapon.SetActive(false);
                m_hasWeapon = false;
                _ennemyNav.speed = m_patrolSpeed;
                _ennemyNav.ResetPath();

                break;
            case EnnemyState.ALERT:

                _weapon.SetActive(true);
                m_hasWeapon = true;
                _ennemyNav.speed = _alertSpeed;
                _checkTime = 10f;
                _sentryAudio.SentrySuspiciousdex();
                _ennemyNav.SetDestination(_lastKnownPos);

                break;
            case EnnemyState.ATTACK:

                if (_canTalk)
                {
                    _sentryAudio.SentryAttackSounds(Random.Range(0, 3));
                }

                m_hasWeapon = true;
                _weapon.SetActive(true);
                _ennemyAnim.SetBool("Attack", true);
                _ennemyNav.ResetPath();

                break;
            case EnnemyState.HIT:

                _ennemyNav.speed = 0f;
                _ennemyAnim.SetTrigger("IsHit");
                m_sentryCurrentHp -= _playerData.m_attackPower[0];
                _hitTime = m_timeToBeHitAgain;

                if (_isHitByFireBall)
                {
                    _sentryAudio.SentryAttackSounds(13);
                }
                else
                {
                    _sentryAudio.OnHit();
                }

                break;
            case EnnemyState.STUN:

                _ennemyNav.speed = 0f;
                _ennemyAnim.SetTrigger("Stun");
                _stunTime = _timeOfStun;
                _activeTime = _timeToBeActive;
                _lastKnownPos = _playerTransform.position;

                break;
            case EnnemyState.DEAD:

                _ennemyNav.speed = 0f;
                _ennemyAnim.SetBool("IsDead", true);

                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch (_currentState)
        {
            case EnnemyState.IDLE:

                if (m_isOnPatrol)
                {
                    if (_playerData.m_currentHp <= 0)
                    {
                        return;
                    }
                    else
                    {
                        TransitionToState(EnnemyState.PATROL);
                    }
                }

                break;
            case EnnemyState.PATROL:

                Patrol();

                Check();

                if (!m_isOnPatrol)
                {
                    TransitionToState(EnnemyState.IDLE);
                }

                if (m_isAlerted)
                {
                    TransitionToState(EnnemyState.ALERT);
                }

                if (m_isAttacking)
                {
                    TransitionToState(EnnemyState.ATTACK);
                }

                if (m_isHit)
                {
                    TransitionToState(EnnemyState.HIT);
                }

                if (m_isStun)
                {
                    TransitionToState(EnnemyState.STUN);
                }

                break;
            case EnnemyState.ALERT:

                CheckPos(_lastKnownPos);

                Check();

                if (_playerData.m_timeToRaiseAlert < 0f)
                {
                    TransitionToState(EnnemyState.PATROL);
                }

                if (m_isHit)
                {
                    TransitionToState(EnnemyState.HIT);
                }

                if (m_isAttacking)
                {
                    TransitionToState(EnnemyState.ATTACK);
                }

                break;
            case EnnemyState.ATTACK:

                if (!m_isAttacking)
                {
                    m_isAlerted = true;

                    TransitionToState(EnnemyState.ALERT);
                }

                if (m_isHit)
                {
                    TransitionToState(EnnemyState.HIT);
                }

                if (_playerData.m_currentHp <= 0)
                {
                    TransitionToState(EnnemyState.PATROL);
                }
                break;
            case EnnemyState.HIT:

                _hitTime -= Time.deltaTime;
                m_isAttacking = true;

                if (_hitTime <= 0)
                {
                    TransitionToState(EnnemyState.ATTACK);
                }

                if (m_sentryCurrentHp <= 0)
                {
                    TransitionToState(EnnemyState.DEAD);
                }

                break;
            case EnnemyState.STUN:

                _ennemyNav.ResetPath();

                _stunTime -= Time.deltaTime;

                if (_stunTime < 0f)
                {
                    _ennemyAnim.SetTrigger("GetUp");
                    _activeTime -= Time.deltaTime;

                    if (_activeTime < 0f)
                    {
                        m_isAlerted = true;
                        _playerData.m_alert = true;
                        _playerData.m_timeToRaiseAlert = _playerData.m_defaultAlertTime;
                        TransitionToState(EnnemyState.ALERT);
                    }
                }

                break;
            case EnnemyState.DEAD:

                GetComponent<Collider>().enabled = false;
                GetComponent<MeleeAttackState>().enabled = false;
                this.enabled = false;

                break;
            default:
                break;
        }
    }

    private void OnStateExit()
    {
        switch (_currentState)
        {
            case EnnemyState.IDLE:
                break;
            case EnnemyState.PATROL:

                _canBeStun = false;
                m_isOnPatrol = false;
                _ennemyAnim.SetBool("Patrol", false);
                _ennemyNav.ResetPath();

                break;
            case EnnemyState.ALERT:

                m_isAlerted = false;
                _ennemyAnim.SetBool("Alert", false);
                _hasCheckPlayerPos = false;
                _canTalk = true;

                break;
            case EnnemyState.ATTACK:
                m_isAttacking = false;
                _ennemyAnim.SetBool("Attack", false);
                _canTalk = false;

                break;
            case EnnemyState.HIT:

                m_isHit = false;
                _isHitByFireBall = false;

                break;
            case EnnemyState.STUN:

                m_isStun = false;
                _ennemyAnim.ResetTrigger("Stun");
                _ennemyAnim.ResetTrigger("GetUp");

                break;
            case EnnemyState.DEAD:
                break;
            default:
                break;
        }
    }

    private void TransitionToState(EnnemyState _nextState)
    {
        OnStateExit();
        _currentState = _nextState;
        OnstateEnter();
    }

    #endregion


    #region Main Method

    private void Patrol()
    {
        if (m_isOnPatrol)
        {
            _ennemyAnim.SetBool("Patrol", true);

            if (!_ennemyNav.pathPending && _ennemyNav.remainingDistance <= 0f)
            {
                if (_wayPoints.Length == 0)
                {
                    return;
                }

                _ennemyNav.destination = _wayPoints[_destination].position;
                _destination = (_destination + 1) % _wayPoints.Length;

                if (m_canCheck && _destination == _waypointTocheck)
                {
                    m_check = true;
                }
                else
                {
                    return;
                }
            }
        }
        else
        {
            _ennemyAnim.SetBool("Patrol", false);
        }
    }

    private void Check()
    {
        if (m_check)
        {
            _ennemyNav.isStopped = true;
            _checkTime -= Time.deltaTime;
            _ennemyAnim.SetBool("Check", true);

            if (_checkTime < 0f)
            {
                m_check = false;
                _hasCheckPlayerPos = !_hasCheckPlayerPos;
            }
        }
        else
        {
            _ennemyNav.isStopped = false;
            _ennemyAnim.SetBool("Check", false);
            _checkTime = m_timeTocheck;
        }
    }

    private void CheckPos(Vector3 _position)
    {
        _ennemyAnim.SetBool("Alert", true);

        if (!_hasCheckPlayerPos)
        {
            _offsetCheck = new Vector3(Random.Range(-3, 3), _offsetCheck.y, Random.Range(-3, 3));
            _ennemyNav.SetDestination(_position);

            if (_ennemyNav.remainingDistance <= 1f)
            {
                m_check = true;
            }
        }
        else if (_hasCheckPlayerPos)
        {
            _ennemyNav.SetDestination(_position + _offsetCheck);

            if (_ennemyNav.remainingDistance <= 1f)
            {
                m_check = true;
            }
        }
    }

    private void HandleAnimations()
    {
        Vector3 _worldDeltaPosition = _ennemyNav.nextPosition - transform.position;

        //Map "WorldDeltaPosition" to Local Space
        float _directionX = Vector3.Dot(transform.right, _worldDeltaPosition);
        float _directionZ = Vector3.Dot(transform.forward, _worldDeltaPosition);
        Vector2 _deltaPosition = new Vector2(_directionX, _directionZ);

        //Low Pass Filter the deltaMove
        float _smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, _deltaPosition, _smooth);

        //Update Velocity if Time Advances
        if (Time.deltaTime > 1e-5f)
        {
            _ennemyVelocity = _smoothDeltaPosition / Time.deltaTime;
        }

        bool _shouldMove = _ennemyVelocity.magnitude > 0.5f && _ennemyNav.remainingDistance > _ennemyNav.radius;

        //Update Animation Parameters
        _ennemyAnim.SetFloat("VeloX", _ennemyVelocity.x);
        _ennemyAnim.SetFloat("VeloZ", _ennemyVelocity.y);
    }

    #endregion


    #region Privates

    private EnnemyState _currentState;

    private NavMeshAgent _ennemyNav;
    private Rigidbody _ennemyRigidbody;
    private Animator _ennemyAnim;
    private int _destination;
    private Vector2 _smoothDeltaPosition;
    private Vector2 _ennemyVelocity = Vector2.zero;

    //Alert
    public bool _hasCheckPlayerPos = false;

    //Audio
    private SentryAudio _sentryAudio;

    #endregion
}
