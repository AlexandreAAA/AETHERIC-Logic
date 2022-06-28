using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    IDLE,
    RUN,
    ROLL,
    CROUCH,
    ATTACK,
    CASTING,
    HIT,
    DEAD,
    CINEMATIC
}
public class PlayerController : MonoBehaviour
{
    #region Exposed

    [Header("Data")]
    [SerializeField]
    private CharacterData _playerData;
    public int _playerCurrentHp;
    [SerializeField]
    private EnnemyData _ennemyData;

    [Header("Speed & Move Parameters")]
    [SerializeField]
    private float _moveSpeed;
    public float _crouchSpeed;
    public float _castingSpeed;
    [SerializeField]
    private float _rollSpeed = 10f;
    [SerializeField]
    private float _rollDuration = 0.5f;
    [SerializeField]
    private AudioClip _rollSound;
    public float _maxDelatPos;

    //Hit And Death
    [SerializeField]
    private float _hitTime;
    public float _timeToBeHitAgain = 0.2f;

    [Header("Flags")]
    public bool m_isCrouch = false;
    public bool m_isComboing = false;
    public bool m_chargeAttack = false;
    public bool m_isCasting = false;
    public bool m_isGrounded;
    public bool m_isHit;
    public bool m_isDead;

    [Header("Attack Parameters")]
    [SerializeField]
    private float _timeToDropCombo = 0.9f;
    [SerializeField]
    private float _chargeDrop = 0.7f;
    [SerializeField]
    private float _detectRange = 2f;

    [Header("RayCasting")]
    [SerializeField]
    private float _rayOffset;
    [SerializeField]
    private float _raycastLength;
    [SerializeField]
    private float _heightOffset;
    [SerializeField]
    private LayerMask _groundLayer;

    [Header("Level Manager Ref")]
    [SerializeField]
    private LevelManager _levelManager;

    #endregion

    #region Init

    private void Awake()
    {
        _eveAnimator = GetComponentInChildren<Animator>();
        _eveRigidbody = GetComponent<Rigidbody>();
        _cameraTransform = Camera.main.transform;
        _playerTransform = GetComponent<Transform>();

    }

    void Start()
    {
        TransitionToState(State.IDLE);
        _playerCrouch = GetComponent<PlayerCrouch>();
        _playerCasting = GetComponent<PlayerCasting>();
        _playerRot = transform.rotation;
        _eveAudioSource = GetComponentInChildren<AudioSource>();
        _playerCurrentHp = _playerData.m_totalHp;
        _playerAudio = GetComponentInChildren<PlayerAudioManager>();

    }
    #endregion


    #region Unity API

    void Update()
    {
        _eveAnimator.SetFloat("DirMag", _movement.magnitude);
        _playerRot.x = 0f;

        RayCasting();


        // PLAYER INPUTS
        _movement = new Vector3(Input.GetAxis("Horizontal"), _directionInput.y, Input.GetAxis("Vertical"));

        float _xHorizontal = Input.GetAxis("Horizontal");
        float _zVertical = Input.GetAxis("Vertical");


        if (!_isRolling)
        {
            if (_playerCrouch._inCover)
            {
                _directionInput = transform.right * _xHorizontal;
            }
            else
            {
                _directionInput = new Vector3(Input.GetAxis("Horizontal"), _orientationInput.y, Input.GetAxis("Vertical")).normalized;
                _directionInput.y = 0f;
            }

            if (_directionInput != Vector3.zero)
            {
                if (!m_isCasting)
                {
                    if (!_playerCrouch._inCover)
                    {
                        if (!m_isComboing)
                        {
                            _orientationInput = new Vector3(Input.GetAxis("Horizontal"), _orientationInput.y, Input.GetAxis("Vertical"));
                            _lookRotation = Quaternion.LookRotation(_orientationInput.normalized);
                        }
                    }
                }
            }
        }


        OnStateUpdate();

        //Melee Attack Input
        if (!m_isCrouch && !m_isCasting && Input.GetButtonDown("StrongAttack"))
        {
            m_chargeAttack = true;
            _eveAnimator.SetTrigger("BigAttack");
        }

        if (!_playerData.m_alert)
        {
            _playerCurrentHp = _playerData.m_totalHp;
        }

        _playerData.m_currentHp = _playerCurrentHp;

    }

    private void FixedUpdate()
    {
        RbPositionGrounded();

        //Rigidbody Velocity
        if (_isRolling)
        {
            _eveRigidbody.velocity = _orientation.normalized * _rollSpeed * Time.fixedDeltaTime;
        }
        else if (!m_isCrouch && !m_isCasting)
        {
            RigidBodyApply();
        }

        //RigidBody Rotation
        if (_playerCrouch._inCover)
        {
            _eveRigidbody.rotation = _playerCrouch._coverRotation;
        }
        else
        {
            _eveRigidbody.MoveRotation(_lookRotation.normalized);
        }

        //SOFTLOCK
        if (m_isComboing && m_ennemyTransform != null || _playerCrouch._stealthAttack && m_ennemyTransform != null)
        {
            _targetRot.x = 0f;
            _eveRigidbody.MoveRotation(_targetRot.normalized);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnnemyWeapon"))
        {
            m_isHit = true;
        }
    }

    private void OnGUI()
    {
        //GUILayout.Button(_currentState.ToString() + _orientation.ToString() + _rollSpeed.ToString());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up, _detectRange);
    }

    #endregion


    #region STATE MACHINE

    private void OnStateEnter()
    {
        switch (_currentState)
        {
            case State.IDLE:

                _currentSpeed = 0f;

                break;
            case State.RUN:

                _currentSpeed = _moveSpeed;
                _eveAnimator.SetBool("IsRunning", true);

                break;
            case State.ROLL:
                _currentSpeed = _rollSpeed;
                _timeRollFinish = Time.timeSinceLevelLoad + _rollDuration;
                _eveAnimator.SetBool("IsRolling", true);
                _eveAudioSource.PlayOneShot(_rollSound, 0.5f);

                break;
            case State.CROUCH:

                m_isCrouch = true;
                _currentSpeed = _crouchSpeed;
                _eveAnimator.SetBool("IsCrouching", true);

                break;
            case State.ATTACK:

                _currentSpeed = 0f;
                m_isComboing = true;

                if (m_chargeAttack)
                {
                    _comboTime = _chargeDrop;
                }
                else
                {
                    _comboTime = _timeToDropCombo;
                }

                _eveAnimator.SetBool("IsCombo", m_isComboing);
                _eveAnimator.SetFloat("DirMag", _currentSpeed);

                break;
            case State.CASTING:

                m_isCasting = true;
                _playerCasting._fireBall.SetActive(true);
                _currentSpeed = _castingSpeed;
                _eveAnimator.SetBool("IsCasting", m_isCasting);

                break;
            case State.HIT:

                _playerCurrentHp -= 20;
                _playerAudio.HitSounds();
                _currentSpeed = 0f;
                _eveAnimator.SetTrigger("IsHit");
                _hitTime = _timeToBeHitAgain;

                break;
            case State.DEAD:
                m_isDead = true;
                _playerAudio.DeathSound();
                _currentSpeed = 0f;
                _eveAnimator.SetBool("IsDead", true);
                break;
            case State.CINEMATIC:
                _eveAnimator.SetBool("Cinematic", true);
                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch (_currentState)
        {
            case State.IDLE:

                if (_movement.magnitude > 0f)
                {
                    TransitionToState(State.RUN);
                }

                if (Input.GetButtonDown("Fire3"))
                {
                    TransitionToState(State.ROLL);
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    TransitionToState(State.CROUCH);
                }

                if (Input.GetButtonDown("Fire1") || m_chargeAttack)
                {
                    TransitionToState(State.ATTACK);
                }

                if (Input.GetButton("Fire2"))
                {
                    TransitionToState(State.CASTING);
                }

                if (m_isHit)
                {
                    TransitionToState(State.HIT);
                }

                if (_levelManager != null)
                {

                    if (_levelManager.m_cinematic)
                    {
                        TransitionToState(State.CINEMATIC);
                    }
                }

                break;
            case State.RUN:

                SetOrientation();

                if (Input.GetAxis("Horizontal") == 0f && Input.GetAxis("Vertical") == 0f)
                {
                    TransitionToState(State.IDLE);
                }

                if (Input.GetButtonDown("Fire3"))
                {
                    TransitionToState(State.ROLL);
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    TransitionToState(State.CROUCH);
                }

                if (Input.GetButtonDown("Fire1") || m_chargeAttack)
                {
                    TransitionToState(State.ATTACK);
                }

                if (Input.GetButton("Fire2"))
                {
                    TransitionToState(State.CASTING);
                }

                if (m_isHit)
                {
                    TransitionToState(State.HIT);
                }

                break;
            case State.ROLL:

                _isRolling = Time.timeSinceLevelLoad < _timeRollFinish;

                if (!_isRolling)
                {
                    if (_movement.magnitude > 0f)
                    {
                        TransitionToState(State.RUN);
                    }
                    else
                    {
                        TransitionToState(State.IDLE);
                    }
                }

                break;
            case State.CROUCH:

                SetOrientation();

                if (!_playerCrouch._inCover)
                {
                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        if (_movement.magnitude > 0f)
                        {
                            TransitionToState(State.RUN);
                        }
                        else
                        {
                            TransitionToState(State.IDLE);
                        }
                    }
                }

                if (m_isHit)
                {
                    TransitionToState(State.HIT);
                }

                break;
            case State.ATTACK:

                ComboDir();

                _comboTime -= Time.deltaTime;

                if (Input.GetButtonDown("Fire1"))
                {
                    _comboTime = _timeToDropCombo;
                    _eveAnimator.SetTrigger("Attack");
                }

                if (_comboTime < 0f)
                {
                    m_isComboing = false;
                    m_chargeAttack = false;

                    if (_movement.magnitude > 0f)
                    {
                        TransitionToState(State.RUN);
                    }
                    else
                    {
                        TransitionToState(State.IDLE);
                    }
                }

                break;
            case State.CASTING:

                SetOrientation();

                if (!Input.GetButton("Fire2") && !_playerCasting.m_isAttacking)
                {
                    if (_movement.magnitude > 0f)
                    {
                        TransitionToState(State.RUN);
                    }
                    else
                    {
                        TransitionToState(State.IDLE);
                    }
                }

                if (m_isHit)
                {
                    TransitionToState(State.HIT);
                }
                break;
            case State.HIT:

                _hitTime -= Time.deltaTime;

                if (_playerCurrentHp <= 0)
                {
                    TransitionToState(State.DEAD);
                }

                if (_hitTime < 0f)
                {
                    if (m_isCrouch)
                    {
                        TransitionToState(State.CROUCH);
                    }
                    else if (m_isCasting)
                    {
                        TransitionToState(State.CASTING);
                    }
                    else
                    {
                        TransitionToState(State.IDLE);
                    }
                }

                break;
            case State.DEAD:

                //_levelManager.IsDead();

                if (!m_isDead)
                {
                    _playerData.m_alert = false;
                    TransitionToState(State.IDLE);
                }

                break;
            case State.CINEMATIC:

                if (!_levelManager.m_cinematic)
                {
                    TransitionToState(State.IDLE);
                }

                break;
            default:
                break;
        }
    }

    private void OnStateExit()
    {
        switch (_currentState)
        {
            case State.IDLE:
                break;
            case State.RUN:

                _eveAnimator.SetBool("IsRunning", false);

                break;
            case State.ROLL:

                _eveAnimator.SetBool("IsRolling", false);

                break;
            case State.CROUCH:

                m_isCrouch = false;
                _eveAnimator.SetBool("IsCrouching", false);

                break;
            case State.ATTACK:

                _eveAnimator.SetBool("IsCombo", m_isComboing);
                _lookRotation = Quaternion.RotateTowards(_eveRigidbody.rotation, _lookRotation, 7f);

                break;
            case State.CASTING:

                m_isCasting = false;
                _playerCasting._fireBall.SetActive(false);
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                _eveAnimator.SetBool("IsCasting", m_isCasting);

                break;
            case State.HIT:

                m_isHit = false;
                _eveAnimator.ResetTrigger("IsHit");

                break;
            case State.DEAD:

                _playerTransform.position = _orginalPos.position;
                _eveAnimator.SetBool("IsDead", false);

                break;
            case State.CINEMATIC:

                _eveAnimator.SetBool("Cinematic", false);
                break;
            default:
                break;
        }
    }

    private void TransitionToState(State _nextState)
    {
        OnStateExit();
        _currentState = _nextState;
        OnStateEnter();
    }

    #endregion


    #region Main Method

    private void SetOrientation()
    {
        if (_directionInput.magnitude > 0)
        {
            _orientation = _directionInput;

            _eveAnimator.SetFloat("MoveX", _orientation.x);
            _eveAnimator.SetFloat("MoveZ", _orientation.z);
            _eveAnimator.SetFloat("DirMag", _movement.magnitude);
        }

        if (_playerCrouch._inCover)
        {
            _eveAnimator.SetFloat("MoveX", Input.GetAxis("Horizontal"));
        }
    }

    private void MeleeCombo()
    {
        if (!m_isCrouch && Input.GetButtonDown("Fire1"))
        {
            _comboTime = _timeToDropCombo;
            _comboTime -= Time.deltaTime;
            m_isComboing = true;

            _eveAnimator.SetBool("IsCombo", m_isComboing);
            _eveAnimator.SetTrigger("Attack");
        }

        if (_comboTime < 0f)
        {
            m_isComboing = false;
            _eveAnimator.SetBool("isCombo", m_isComboing);
        }
    }

    public void ComboDir()
    {
        if (m_ennemyTransform != null)
        {
            Vector3 _targetDir = m_ennemyTransform.position - transform.position;
            _targetDir.y = 0f;
            _targetRot = Quaternion.LookRotation(_targetDir, Vector3.up);
        }
    }

    private void RayCasting()
    {
        _groundDetectRaycasts[0] = new Ray(transform.position + Vector3.up, Vector3.down);
        _groundDetectRaycasts[1] = new Ray(transform.position + Vector3.up + Vector3.forward * _rayOffset, Vector3.down);
        _groundDetectRaycasts[2] = new Ray(transform.position + Vector3.up - Vector3.forward * _rayOffset, Vector3.down);
        _groundDetectRaycasts[3] = new Ray(transform.position + Vector3.up + Vector3.right * _rayOffset, Vector3.down);
        _groundDetectRaycasts[4] = new Ray(transform.position + Vector3.up - Vector3.right * _rayOffset, Vector3.down);

        int _hitCount = 0;
        _averageGroundHeight = Vector3.zero;

        for (int i = 0; i < _groundDetectRaycasts.Length; i++)
        {
            Ray _currentRay = _groundDetectRaycasts[i];
            Debug.DrawRay(_currentRay.origin, _currentRay.direction * _raycastLength, Color.blue);

            if (Physics.Raycast(_currentRay, out RaycastHit _hit, _raycastLength, _groundLayer))
            {
                _averageGroundHeight += _hit.point;
                _hitCount++;
                m_isGrounded = true;
            }
        }

        if (_hitCount > 0)
        {
            _averageGroundHeight = _averageGroundHeight / _hitCount;
            _averageGroundHeight.y += _heightOffset;
        }

        if (_hitCount == 0)
        {
            m_isGrounded = false;
        }
    }

    private void RbPositionGrounded()
    {
        if (m_isGrounded)
        {
            _eveRigidbody.useGravity = false;
            Vector3 _position = transform.position;
            _position.y = _averageGroundHeight.y;
            _eveRigidbody.MovePosition(_position);
        }
        else
        {
            _eveRigidbody.useGravity = true;
        }
    }

    public void RigidBodyApply()
    {
        _eveRigidbody.velocity = new Vector3(_directionInput.x * _currentSpeed * Time.fixedDeltaTime, _eveRigidbody.velocity.y, _directionInput.z * _currentSpeed * Time.fixedDeltaTime);

        if (_isRolling)
        {
            _eveRigidbody.velocity = _orientation.normalized * _rollSpeed * Time.fixedDeltaTime;
        }
    }

    #endregion


    #region Privates

    private State _currentState;

    private Transform _cameraTransform;

    private Rigidbody _eveRigidbody;

    private Animator _eveAnimator;

    private Ray[] _groundDetectRaycasts = new Ray[5];

    private Vector3 _averageGroundHeight;

    private Vector3 _directionInput;

    private Vector3 _movement;

    public float _currentSpeed;

    private Vector3 _orientationInput;

    private Vector3 _orientation;

    private Quaternion _lookRotation;

    private float _timeRollFinish = 0f;

    private bool _isRolling = false;

    private float _comboTime;

    public Transform m_ennemyTransform;

    private Quaternion _targetRot;

    private Quaternion _playerRot;

    private Vector3 _targetPos;

    private PlayerCrouch _playerCrouch;

    private PlayerCasting _playerCasting;

    private AudioSource _eveAudioSource;

    private PlayerAudioManager _playerAudio;

    private Transform _playerTransform;

    public Transform _orginalPos;

    #endregion
}
