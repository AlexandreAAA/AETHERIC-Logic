using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMouseInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _playerAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit _hit;
            
            if (Physics.Raycast( Camera.main.ScreenPointToRay(Input.mousePosition), out _hit, 100))
            {
                _playerAgent.destination =_hit.point;
            }
        }

    }

    private NavMeshAgent _playerAgent;
}
