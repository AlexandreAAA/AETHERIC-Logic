using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AttackState
{
    NOTATTACKING,
    CHASE,
    WALK,
    DODGE,
    BLOCK,
    ATTACK,
    STRONGATTACK,
    CHARGE,
    WAIT
}
public class MeleeAttackState : MonoBehaviour
{

    #region Exposed

    [Header("Player Parameter")]
    [SerializeField]
    private CharacterData _playerData;
    [SerializeField]
    private Transform _playerTransform;

    [Header("Flags")]
    public bool m_isAttacking;
    public bool m_isChasing;
    public bool m_isWalkingTo;
    public bool m_dodge;
    public bool m_attack;
    public bool m_strongAttack;
    public bool m_Charge;
    public bool m_isInvincible;

    [Header("Flags Patterns")]
    [SerializeField]
    private bool _canAttack;
    [SerializeField]
    private bool _canStrongAttack;
    [SerializeField]
    private bool _canCharge;
    [SerializeField]
    private bool _canDodge;

    [Header("Speed and Distance")]
    [SerializeField]
    private float _chasingDistance = 10f;
    [SerializeField]
    private float _distanceToAttack = 0.3f;
    [SerializeField]
    private float _maxChargeDistance = 8f;
    [SerializeField]
    private float _minChargeDistance = 5f;
    [SerializeField]
    private float _chasingSpeed = 5f;
    [SerializeField]
    private float _walkingSpeed = 2.5f;
    [SerializeField]
    private float _attackSpeed = 0.1f;
    //[SerializeField]
    //private float _chargeSpeed = 8f;
    

    [Header("Timers")]
    //Attack State Timer
    [SerializeField]
    private float _attackTime;
    public float _timeToAttack;
    [SerializeField]
    private float _timeOfCharge;

    //WeaponCollider
    [SerializeField]
    private Collider _weaponCollider;

    //Strong Attack Timer
    [SerializeField]
    private float _strongAttackTimer;
    public float _timeUntilNextStrongAttack = 10f;

    //Normal Attack Timer
    [SerializeField]
    private float _normalAttackTimer;
    public float _timeUntilNextSAttack = 5;

    //Charge Timer
    [SerializeField]
    private float _chargeTimer;
    public float _timeUntilNextCharge;

    //Dodge Timer
    [SerializeField]
    private float _dodgeTimer;
    public float _timeUntilNextDodge;




    #endregion


    #region Init

    private void Awake()
    {
        _enemyRb = GetComponent<Rigidbody>();
        _ennemyAnim = GetComponent<Animator>();
        _attackNav = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        _playerTransform = GameObject.Find("PlayerOKV1").GetComponent<Transform>();
        _ennemyBehaviour = GetComponent<EnnemyBehaviour>();

        TransitionToState(AttackState.NOTATTACKING);

        
    }

    #endregion


    #region Unity API

    void Update()
    {
        m_isAttacking = _ennemyBehaviour.m_isAttacking;

        if (m_Charge)
        {
            _attackNav.acceleration = 100f;
        }
        else
        {
            _attackNav.acceleration = 8f;
        }

        //State Machine Update
        OnStateUpdate();
        

        //Update of Patterns's Timers
        AttackTimers();

        Invincible();
    }

    private void OnGUI()
    {
        //GUILayout.Button(_currentState.ToString());
    }

    #endregion


    #region STATE MACHINE

    private void OnStateEnter()
    {
        switch (_currentState)
        {
            case AttackState.NOTATTACKING:
                break;
            case AttackState.CHASE:

                m_isChasing = true;
                _ennemyAnim.SetBool("Chasing", m_isChasing);
                _attackNav.speed = _chasingSpeed;

                break;
            case AttackState.WALK:

                m_isWalkingTo = true;
                _ennemyAnim.SetBool("WalkTo", m_isWalkingTo);
                _attackNav.speed = _walkingSpeed;

                break;
            case AttackState.DODGE:
                break;
            case AttackState.BLOCK:
                break;
            case AttackState.ATTACK:

                _attackNav.speed = _attackSpeed;
                _attackTime = _timeToAttack;
                

                break;
            case AttackState.STRONGATTACK:

                m_strongAttack = true;
                _attackNav.speed = _attackSpeed;
                _attackTime = _timeToAttack;
                

                break;
            case AttackState.CHARGE:

                m_Charge = true;
                _attackTime = _timeOfCharge;
                

                break;
            case AttackState.WAIT:
                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch (_currentState)
        {
            case AttackState.NOTATTACKING:

                if (m_isAttacking)
                {
                    _attackNav.SetDestination(_playerTransform.position);

                    if (_attackNav.remainingDistance > _chasingDistance)
                    {
                        TransitionToState(AttackState.CHASE);
                    }
                    else
                    {
                        TransitionToState(AttackState.WALK);
                    }
                }

                break;
            case AttackState.CHASE:

                _attackNav.SetDestination(_playerTransform.position);

                if (_attackNav.remainingDistance < _chasingDistance)
                {
                    TransitionToState(AttackState.WALK);
                }

                if (!m_isAttacking)
                {
                    TransitionToState(AttackState.NOTATTACKING);
                }

                break;
            case AttackState.WALK:

                _attackNav.SetDestination(_playerTransform.position);

                if (_attackNav.remainingDistance > _chasingDistance)
                {
                    TransitionToState(AttackState.CHASE);
                }

                if (_attackNav.remainingDistance < _maxChargeDistance 
                    && _attackNav.remainingDistance > _minChargeDistance && _canCharge)
                {
                    TransitionToState(AttackState.CHARGE);
                }

                if (_attackNav.remainingDistance < _distanceToAttack)
                {
                    if (_canStrongAttack)
                    {
                        TransitionToState(AttackState.STRONGATTACK);
                    }
                    else if (_canAttack)
                    {
                        TransitionToState(AttackState.ATTACK);
                    }
                }

                if (!m_isAttacking)
                {
                    TransitionToState(AttackState.NOTATTACKING);
                }

                break;
            case AttackState.DODGE:
                break;
            case AttackState.BLOCK:
                break;
            case AttackState.ATTACK:

                _attackNav.SetDestination(_playerTransform.position);
                _attackTime -= Time.deltaTime;

                _ennemyAnim.SetBool("NormalAttack", true);

                if (_attackTime < 0f)
                {
                    TransitionToState(AttackState.WALK);
                }

                break;
            case AttackState.STRONGATTACK:

                _attackNav.SetDestination(_playerTransform.position);
                _attackTime -= Time.deltaTime;

                _ennemyAnim.SetBool("StrongAttack", true);

                if (_attackTime < 0f)
                {
                    TransitionToState(AttackState.WALK);
                }

                break;
            case AttackState.CHARGE:

                _attackTime -= Time.deltaTime;

                _ennemyAnim.SetBool("Charge", true);

                if (_attackTime < 0f)
                {
                    TransitionToState(AttackState.WALK);
                }


                break;
            case AttackState.WAIT:
                break;
            default:
                break;
        }
    }

    private void OnStateExit()
    {
        switch (_currentState)
        {
            case AttackState.NOTATTACKING:
                break;
            case AttackState.CHASE:

                m_isChasing = false;
                _ennemyAnim.SetBool("Chasing", m_isChasing);

                break;
            case AttackState.WALK:

                m_isWalkingTo = false;
                _ennemyAnim.SetBool("WalkTo", m_isWalkingTo);

                break;
            case AttackState.DODGE:
                break;
            case AttackState.BLOCK:
                break;
            case AttackState.ATTACK:

                
                _ennemyAnim.SetBool("NormalAttack", false);
                _canAttack = false;
                _normalAttackTimer = _timeUntilNextSAttack;

                break;
            case AttackState.STRONGATTACK:

                
                m_strongAttack = false;
                _ennemyAnim.SetBool("StrongAttack", false);
                _canStrongAttack = false;
                _strongAttackTimer = _timeUntilNextStrongAttack;

                break;
            case AttackState.CHARGE:

                
                _ennemyAnim.SetBool("Charge", false);
                _canCharge = false;
                m_Charge = false;
                _chargeTimer = _timeUntilNextCharge;
                _attackNav.SetDestination(_playerTransform.position);
                break;
            case AttackState.WAIT:

                _ennemyAnim.SetBool("Shout", false);

                break;
            default:
                break;
        }
    }

    private void TransitionToState(AttackState _nextState)
    {
        OnStateExit();
        _currentState = _nextState;
        OnStateEnter();
    }

    #endregion


    #region MAin Method

    private void AttackTimers()
    {
        // STRONG ATTACK TIMER
        if (!_canStrongAttack)
        {
            _strongAttackTimer -= Time.deltaTime;

            if (_strongAttackTimer < 0f)
            {
                _canStrongAttack = true;
            }
        }

        // NORMAL ATTACK TIMER
        if (!_canAttack)
        {
            _normalAttackTimer -= Time.deltaTime;

            if (_normalAttackTimer < 0f)
            {
                _canAttack = true;
            }
        }

        // CHARGE TIMER
        if (!_canCharge)
        {
            _chargeTimer -= Time.deltaTime;

            if (_chargeTimer < 0f)
            {
                _canCharge = true;
            }
        }
    }

    private void Invincible()
    {

        if (m_Charge || m_strongAttack)
        {
            m_isInvincible = true;
        }
        else
        {
            m_isInvincible = false;
        }
    }

    public void ChargeOn(float _speed)
    {
        _attackNav.speed = _speed;
    }

    #endregion


    #region Privates

    private AttackState _currentState;

    private EnnemyBehaviour _ennemyBehaviour;

    private Rigidbody _enemyRb;
    private Animator _ennemyAnim;
    public NavMeshAgent _attackNav;

    

    #endregion
}
