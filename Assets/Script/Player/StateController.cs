using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    public class StateController : MonoBehaviour
    {
        #region Exposed
        public State currentState;
        public MouvementVariable mouvementVariable;
        public Transform mTransform;
        public Rigidbody rigidBody;
        public Animator anim;
        public InputHandler playerInput;
        public CapsuleCollider capsCollider;
        #endregion

        #region Hide
        public bool isrunning;
        public bool isGrounded;
        public bool isCrouching;
        #endregion

        void Start()
        {

            playerInput = GetComponent<InputHandler>();
            mTransform = this.transform;
            rigidBody = GetComponent<Rigidbody>();
            anim = GetComponentInChildren<Animator>();
            capsCollider = GetComponent<CapsuleCollider>();
            
            if (currentState != null)
            {
                currentState.OnStateEnter(this);
            }
        }

        void Update()
        {
            if (currentState != null)
            {
                currentState.OnStateUpdate(this);
            }
        }

        void FixedUpdate()
        {
            if (currentState != null)
            {
                currentState.OnStateFixedUpdate(this);
            }
        }
    }
}
