using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNav : MonoBehaviour
{
    public Transform[] m_wayPoints;

    void Start()
    {
        _soldierAgent = GetComponent<NavMeshAgent>();
        _soldierAgent.autoBraking = false;

        GoToNextPoint();
    }

    private void GoToNextPoint()
    {
        if (m_wayPoints.Length ==0)
        {
            return;
        }

        _soldierAgent.destination = m_wayPoints[_destPoint].position;

        _destPoint = (_destPoint + 1) % m_wayPoints.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_soldierAgent.pathPending && _soldierAgent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }


    private int _destPoint = 0;
    private NavMeshAgent _soldierAgent;
}
