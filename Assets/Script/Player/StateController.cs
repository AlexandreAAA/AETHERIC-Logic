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
        public AudioSource audioSource;
        public CapsuleCollider capsCollider;
        public CapsuleCollider stealthCollider;
        #endregion

        #region Hide
        public bool isrunning;
        public bool isGrounded;
        public bool isCrouching;
        public bool isRolling;
        public bool isInCover;
        public bool isCasting;
        public bool isComboing;
        public bool castingAttack;
        public bool isJumping;
        #endregion

        void Start()
        {

            playerInput = GetComponent<InputHandler>();
            mTransform = this.transform;
            rigidBody = GetComponent<Rigidbody>();
            anim = GetComponentInChildren<Animator>();
            audioSource = GetComponentInChildren<AudioSource>();
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
