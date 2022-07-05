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
        #endregion

        #region Hide
        public bool isrunning;
        public bool isGrounded;
        #endregion

        void Start()
        {

            playerInput = GetComponent<InputHandler>();
            mTransform = this.transform;
            rigidBody = GetComponent<Rigidbody>();
            anim = GetComponentInChildren<Animator>();
            
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