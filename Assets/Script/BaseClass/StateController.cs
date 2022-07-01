using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    public class StateController : MonoBehaviour
    {
        public State currentState;
        #region Exposed
        public Transform mTransform;
        public Rigidbody rigidBody;
        public Animator anim;
#if ENABLE_LEGACY_INPUT_MANAGER
        public InputHandler playerInput;
#endif

        #endregion

        void Start()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            playerInput = GetComponent<InputHandler>();
#endif
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
