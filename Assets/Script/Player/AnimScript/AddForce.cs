using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : StateMachineBehaviour
{
    private Rigidbody _eveRigidbody;

    [SerializeField]
    private float _chargeDuration = 0.5f;
    public  float _chargeTime;
    private bool _isCharging = false;
    public float _chargeForce = 10;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _eveRigidbody = GameObject.Find("PlayerOKV1").GetComponent<Rigidbody>();
        _chargeTime = _chargeDuration;
        _isCharging = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _chargeTime -= Time.deltaTime;

        if (_isCharging)
        {
            _eveRigidbody.AddForce(_eveRigidbody.transform.forward * _chargeForce * Time.deltaTime, ForceMode.Impulse);
        }

        if (_chargeTime < 0f)
        {
            _isCharging = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
