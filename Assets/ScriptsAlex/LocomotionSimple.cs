using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LocomotionSimple : MonoBehaviour
{

    #region Unity API

    void Start()
    {
        _soldierAnimator = GetComponent<Animator>();
        _soldierAgent = GetComponent<NavMeshAgent>();

        // don't Update position autmatically
        _soldierAgent.updatePosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _worldDeltaPosition = _soldierAgent.nextPosition - transform.position;

        //Map "WorldDeltaPosition" to local SPace
        float _directionX = Vector3.Dot(transform.right, _worldDeltaPosition);
        float _directionY = Vector3.Dot(transform.forward, _worldDeltaPosition);
        Vector2 _deltaPosition = new Vector2(_directionX, _directionY);

        // low-pass Filter the deltaMove
        float _smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, _deltaPosition, _smooth);

        // Update Velocity if Time advances
        if (Time.deltaTime > 1e-5f)
        {
            _velocity = _smoothDeltaPosition / Time.deltaTime;
        }

        bool _shouldMove = _velocity.magnitude > 0.5f && _soldierAgent.remainingDistance > _soldierAgent.radius;

        //Update Animation parameters
        _soldierAnimator.SetBool("Move", _shouldMove);
        _soldierAnimator.SetFloat("VeloX", _velocity.x);
        _soldierAnimator.SetFloat("VeloY", _velocity.y);

        GetComponent<LookAt>().lookAtTargetPosition = _soldierAgent.steeringTarget + transform.forward;
    }

    #endregion

    #region Main Method

    private void OnAnimatorMove()
    {
        //Update Position to Agent position
        transform.position = _soldierAgent.nextPosition;
    }
    #endregion

    #region Privates

    private Animator _soldierAnimator;
    private NavMeshAgent _soldierAgent;
    private Vector2 _smoothDeltaPosition;
    private Vector2 _velocity = Vector2.zero;

    #endregion
}
