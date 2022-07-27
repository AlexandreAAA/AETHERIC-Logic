using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CastingState
{
    NOCASTSTATE,
    CAST,
    ATTACK,
    TELEPORT
}
public class PlayerCasting : MonoBehaviour
{
    #region Exposed

    public PlayerController m_playerController;

    public float m_castingSpeed;

    [Header("Fireball")]
    [SerializeField]
    private GameObject _fireBallPrefab;
    [SerializeField]
    private Transform _launcher;
    public GameObject _fireBall;
    [SerializeField]
    private float _attackTime;
    public float m_timeBeforeNextAttack;
    [SerializeField]
    private float _projectileSpeed;
    [SerializeField]
    private float _fireballDeathTime = 5f;
    [SerializeField]
    private LayerMask _targetingLayer;

    [Header("Flags")]
    public bool m_isCasting = false;
    public bool m_isAttacking = false;
    public bool m_isTeleporting = false;

    [Header("Cursors")]
    [SerializeField]
    private Texture2D _castingCursor;

    #endregion

    #region Unity API

    private void Awake()
    {
        m_playerController = GetComponent<PlayerController>();
        _eveAnimator = GetComponentInChildren<Animator>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        m_isCasting = m_playerController.m_isCasting;

        if (m_isCasting)
        {
            //Change Mouse Cursor
            ManageCursor(_castingCursor);

            //Direction Input
            float _xHorizontal = Input.GetAxis("Horizontal");
            float _zVertical = Input.GetAxis("Vertical");

            //_castingDirectionInput = (_mainCamera.transform.right * _xHorizontal + _mainCamera.transform.forward * _zVertical).normalized;
            _castingDirectionInput = new Vector3(_xHorizontal, 0, _zVertical).normalized;
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit _hit, 100, _targetingLayer))
            {
                _lookedAtPoint = _hit.point;
                Vector3 _lookdirection = _hit.point - transform.position;
                _lookdirection.y = 0f;
                transform.forward = _lookdirection;
            }

            //Cast Skill Input
            if (Input.GetButtonDown("Fire1"))
            {
                _eveAnimator.SetTrigger("Fireball");
                //FireBall();
            }
        }

        OnStateUpdate();

        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        m_playerController.RigidBodyApply();
    }

    #endregion

    #region Main Method

    private void OnStateEnter()
    {
        switch (_currentState)
        {
            case CastingState.NOCASTSTATE:
                break;
            case CastingState.CAST:

                //m_castingSpeed = 4f;
                m_playerController._currentSpeed = m_playerController._castingSpeed;

                break;
            case CastingState.ATTACK:

                //m_castingSpeed = 0f;
                m_playerController._currentSpeed = 0f;
                m_isAttacking = true;
                _attackTime = m_timeBeforeNextAttack;

                break;
            case CastingState.TELEPORT:
                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch (_currentState)
        {
            case CastingState.NOCASTSTATE:

                if (Input.GetButton("Fire2"))
                {
                    TransitionToState(CastingState.CAST);
                }

                break;
            case CastingState.CAST:

                if (!Input.GetButton("Fire2"))
                {
                    TransitionToState(CastingState.NOCASTSTATE);
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    TransitionToState(CastingState.ATTACK);
                }

                break;
            case CastingState.ATTACK:

                _attackTime -= Time.deltaTime;

                if (Input.GetButtonDown("Fire1"))
                {
                    _attackTime = m_timeBeforeNextAttack;
                    _eveAnimator.SetTrigger("Fireball");
                }

                if (_attackTime < 0f)
                {
                    TransitionToState(CastingState.CAST);
                }
                break;
            case CastingState.TELEPORT:
                break;
            default:
                break;
        }
    }

    private void OnstateExit()
    {
        switch (_currentState)
        {
            case CastingState.NOCASTSTATE:
                break;
            case CastingState.CAST:

                m_isCasting = false;

                break;
            case CastingState.ATTACK:

                m_isAttacking = false;

                break;
            case CastingState.TELEPORT:
                break;
            default:
                break;
        }
    }

    private void TransitionToState( CastingState _nextState)
    {
        OnstateExit();
        _currentState = _nextState;
        OnStateEnter();
    }

    private void ManageCursor( Texture2D _iconToChange, float _anchorRation = 0.5f)
    {
        Vector2 _hotspot = new Vector2(_iconToChange.width, _iconToChange.height) * _anchorRation;
        Cursor.SetCursor(_iconToChange, _hotspot, CursorMode.Auto);
    }

    public void FireBall()
    {
        GameObject _clone = Instantiate(_fireBallPrefab, _launcher.position, transform.rotation);
        GoForward _goForward = _clone.GetComponent<GoForward>();
        _goForward.SetFireballSpeed(_projectileSpeed);
        Destroy(_clone, _fireballDeathTime);
    }

    // A ETUDIER Animator Parameters for twinstick
    private void UpdateAnimator()
    {
        float forwardBackwardsMagnitude = 0;
        float rightLeftMagnitude = 0;
        if (_castingDirectionInput.magnitude > 0)
        {
            Vector3 normalizedLookingAt = _lookedAtPoint - transform.position;
            normalizedLookingAt.Normalize();

            forwardBackwardsMagnitude = Mathf.Clamp( Vector3.Dot(_castingDirectionInput, normalizedLookingAt), -1, 1);

            Vector3 perpendicularLookingAt = new Vector3( normalizedLookingAt.z, 0, -normalizedLookingAt.x);
            rightLeftMagnitude = Mathf.Clamp( Vector3.Dot(_castingDirectionInput, perpendicularLookingAt), -1, 1);

            _eveAnimator.SetBool("IsMoving", true);
        }
        else
        {
            _eveAnimator.SetBool("IsMoving", false);
        }

        // update the animator parameters
        _eveAnimator.SetFloat("VeloZ", forwardBackwardsMagnitude);
        _eveAnimator.SetFloat("VeloX", rightLeftMagnitude);
    }
    #endregion

    #region Privates

    private CastingState _currentState;

    private Animator _eveAnimator;
    private Camera _mainCamera;

    private Vector3 _castingDirectionInput;
    public Vector3 _lookedAtPoint;

    #endregion
}
