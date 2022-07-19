using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Droid : MonoBehaviour
{
    public enum DroiState
    {
        PATROL,
        ATTACK,
        SEARCH,
        HURT,
        DEATH
    }

    #region Exposed

    public Transform[] m_wayPoints;

    [Header("Flags")]
    public bool _isAuto = true;

    [SerializeField]
    private Transform _playerTransform;

    #endregion


    #region Unity API

    void Start()
    {
        _droidRigidbody = GetComponent<Rigidbody>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _navTest = GetComponent<NavMeshAgent>();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        Patrolling();
    }

    #endregion


    #region Main Method;

    private void Patrolling()
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

    private Rigidbody _droidRigidbody;
    private NavMeshAgent _navTest;
    private int _destPoint = 0;






    #endregion
}
