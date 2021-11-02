using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public enum CrouchState
    {
        NOSTATE,
        CROUCH,
        CRCOVER,
        CRSTEALTHATTACK
    }

    #region exposed

    [SerializeField]
    private float _crouchSpeed;

    public CapsuleCollider m_stealthCollider;

    public Quaternion _coverRotation;

    [Header("Cover Parameters")]
    [SerializeField]
    private Vector3 _colliderOfset;
    [SerializeField]
    private LayerMask _coverLayer;
    [SerializeField]
    private float _rayCastLength;
    [SerializeField]
    private RigidbodyConstraints _coverEdgeConstraint;

    [Header("Flags")]
    public bool _canCover = false;
    public bool _inCover = false;
    public bool _stealthAttack = false;
    public bool m_coverRightEdge = false;
    public bool m_coverLeftEdge = false;

    [Header("Stealth Attack Parameters")]
    [SerializeField]
    private Collider _fistCollider;
    [SerializeField]
    private float _stealthAttackTimer;
    [SerializeField]
    private float _timeTodrop = 0.7f;

    #endregion


    #region Init

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _eveRigidbody = GetComponent<Rigidbody>();
        _originalConstraints = _eveRigidbody.constraints;
        _eveAnimator = GetComponentInChildren<Animator>();


    }

    void Start()
    {
        _eveCollider = GetComponent<CapsuleCollider>();
        _cameraTransform = Camera.main.transform;


        TransitionToState(CrouchState.NOSTATE);

    }

    #endregion


    #region Unity API

    void Update()
    {
        _isCrouch = _playerController.m_isCrouch;

        if (_isCrouch)
        {
            // Direction Input
            _mouvement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            float _xHorizontal = Input.GetAxisRaw("Horizontal");
            float _zVertical = Input.GetAxisRaw("Vertical");

            // Cover Input With CoverEdge Raycast
            if (_inCover)
            {
                _crouchInput = transform.right * _xHorizontal;

                // Raycast For Right edge
                Ray _rayRight = new Ray(transform.position + transform.right * 0.6f + transform.up * 0.6f, transform.forward);
                Debug.DrawRay(_rayRight.origin, _rayRight.direction * _rayCastLength, Color.magenta);
                RaycastHit _hitRight;

                m_coverRightEdge = !Physics.Raycast(_rayRight, out _hitRight, _rayCastLength, _coverLayer);
                _eveAnimator.SetBool("CoverStopRight", m_coverRightEdge);


                //Raycast for Left edge
                Ray _rayLeft = new Ray(transform.position + transform.right * -0.6f + transform.up * 0.6f, transform.forward);
                Debug.DrawRay(_rayLeft.origin, _rayLeft.direction * _rayCastLength, Color.magenta);
                RaycastHit _hitLeft;

                m_coverLeftEdge = !Physics.Raycast(_rayLeft, out _hitLeft, _rayCastLength, _coverLayer);
                _eveAnimator.SetBool("CoverStopLeft", m_coverLeftEdge);

            }
            else // Normal Input
            {
                //_crouchInput = (_cameraTransform.right * _xHorizontal + _cameraTransform.forward * _zVertical).normalized;
                _crouchInput = new Vector3(Input.GetAxis("Horizontal"), _orientationInput.y, Input.GetAxis("Vertical")).normalized;
                _crouchInput.y = 0f;
            }


            //Orientation Input
            if (_crouchInput != Vector3.zero)
            {
                if (!_inCover)
                {
                    if (!_stealthAttack)
                    {
                        _orientationInput = new Vector3(Input.GetAxis("Horizontal"), _orientationInput.y, Input.GetAxis("Vertical"));
                        _lookRotation = Quaternion.LookRotation(_orientationInput.normalized);
                    }
                }
            }

            //Stealth Attack Input
            if (Input.GetButtonDown("Fire1"))
            {
                _eveAnimator.SetTrigger("StealthAttack");
            }

            //Cover Management
            if (Input.GetButtonDown("Fire3"))
            {
                TakeCover();
            }
        }

        OnStateUpdate();
    }

    private void FixedUpdate()
    {

        CrouchWalk();
        Stealth();

        if (_orientationInput != Vector3.zero)
        {
            _eveRigidbody.MoveRotation(_lookRotation);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cover"))
        {
            _canCover = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Cover"))
        {
            _canCover = false;
        }
    }

    private void OnGUI()
    {
        //GUILayout.Button(_currentState.ToString());
    }

    #endregion


    #region Main Method

    //FSM
    private void OnStateEnter()
    {
        switch (_currentState)
        {
            case CrouchState.NOSTATE:
                break;
            case CrouchState.CROUCH:
                //_currentCrouchSpeed = _crouchSpeed;
                _playerController._currentSpeed = _playerController._crouchSpeed;
                break;
            case CrouchState.CRCOVER:
                break;
            case CrouchState.CRSTEALTHATTACK:

                _fistCollider.enabled = true;
                //_currentCrouchSpeed = 0f;
                _playerController._currentSpeed = 50f;
                _stealthAttack = true;
                _stealthAttackTimer = _timeTodrop;

                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch (_currentState)
        {
            case CrouchState.NOSTATE:

                if (_isCrouch)
                {
                    TransitionToState(CrouchState.CROUCH);
                }

                break;
            case CrouchState.CROUCH:

                if (Input.GetKeyDown(KeyCode.C))
                {
                    TransitionToState(CrouchState.NOSTATE);
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    TransitionToState(CrouchState.CRSTEALTHATTACK);
                }

                if (_inCover)
                {
                    TransitionToState(CrouchState.CRCOVER);
                }

                break;
            case CrouchState.CRCOVER:

                if (Input.GetButtonDown("Fire3"))
                {
                    TransitionToState(CrouchState.CROUCH);
                }

                break;
            case CrouchState.CRSTEALTHATTACK:

                _playerController.ComboDir();

                _stealthAttackTimer -= Time.deltaTime;


                if (_stealthAttackTimer < 0f)
                {
                    _stealthAttack = false;
                    TransitionToState(CrouchState.CROUCH);
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
            case CrouchState.NOSTATE:
                break;
            case CrouchState.CROUCH:
                break;
            case CrouchState.CRCOVER:
                _eveRigidbody.constraints = _originalConstraints;
                break;
            case CrouchState.CRSTEALTHATTACK:

                _fistCollider.enabled = false;
                _eveAnimator.ResetTrigger("StealthAttack");
                break;
            default:
                break;
        }
    }

    private void TransitionToState(CrouchState _nextState)
    {
        OnStateExit();
        _currentState = _nextState;
        OnStateEnter();
    }

    private void CrouchWalk()
    {
        if (_isCrouch)
        {
            //_eveRigidbody.velocity = new Vector3(_crouchInput.x * _currentCrouchSpeed, _eveRigidbody.velocity.y, _crouchInput.z * _currentCrouchSpeed);
            //_eveRigidbody.velocity = new Vector3(_mouvement.x * _currentCrouchSpeed, _eveRigidbody.velocity.y, _mouvement.z *_currentCrouchSpeed );
            _playerController.RigidBodyApply();

            if (_inCover)
            {
                if (m_coverLeftEdge && Input.GetAxis("Horizontal") < 0)
                {
                    //_eveRigidbody.velocity = new Vector3(0, _eveRigidbody.velocity.y, _eveRigidbody.velocity.z);
                    _eveRigidbody.constraints = _coverEdgeConstraint;
                }
                else if (m_coverRightEdge && Input.GetAxis("Horizontal") > 0)
                {
                    //_eveRigidbody.velocity = new Vector3(0, _eveRigidbody.velocity.y, _eveRigidbody.velocity.z);
                    _eveRigidbody.constraints = _coverEdgeConstraint;
                }
                else
                {
                    _eveRigidbody.constraints = _originalConstraints;
                }
            }
        }
    }

    //Collider smaller during Crouch State
    private void Stealth()
    {
        if (_isCrouch)
        {
            _eveCollider.enabled = false;
            m_stealthCollider.enabled = true;
        }
        else
        {
            _eveCollider.enabled = true;
            m_stealthCollider.enabled = false;
        }
    }

    private void TakeCover()
    {
        if (_inCover)
        {
            _inCover = false;
            _eveAnimator.SetBool("TakeCover", _inCover);
        }
        else
        {
            Ray _ray = new Ray(transform.position + Vector3.up, transform.forward);
            Debug.DrawRay(_ray.origin, _ray.direction * _rayCastLength, Color.green);
            RaycastHit _hit;

            if (Physics.Raycast(_ray, out _hit, _rayCastLength, _coverLayer) && _canCover)
            {
                //Debug.Log("Ray Hit: " + _hit.transform.name);
                //Debug.Log(_hit.normal);

                Vector3 _coverPos = _hit.point + _hit.normal * 0.3f;
                _eveRigidbody.MovePosition(new Vector3(_coverPos.x, transform.position.y, _coverPos.z));
                Quaternion _toRotation = Quaternion.LookRotation(-_hit.normal);
                _coverRotation = _toRotation;

                //transform.forward = -_hit.normal;

                _inCover = true;
                _eveAnimator.SetBool("TakeCover", _inCover);
            }
        }

        if (_canCover)
        {
            m_stealthCollider.center = _colliderOfset;
        }
    }
    #endregion


    #region Privates

    private CrouchState _currentState;

    private PlayerController _playerController;
    private bool _isCrouch;
    private Vector3 _crouchInput;
    private Vector3 _mouvement;

    private Vector3 _orientationInput;
    private Quaternion _lookRotation;
    private Transform _cameraTransform;

    private Rigidbody _eveRigidbody;
    private RigidbodyConstraints _originalConstraints;
    private Animator _eveAnimator;


    private CapsuleCollider _eveCollider;
    private float _currentCrouchSpeed;


    #endregion
}
