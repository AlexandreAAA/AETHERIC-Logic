using UnityEngine;

namespace EveController
{

    public class InputHandler : MonoBehaviour
    {
        #region Exposed
        public bool enableInput;
        public float horizontal;
        public float vertical;
        public bool attackTrigger;
        public bool secondaryAttackTrigger;
        public bool specialAttackTrigger;
        public bool walkInput;
        public bool crouchInput;
        public bool rollInput;
        public bool coverInput;
        public bool jumpInput;
        public bool aimTrigger;
        public bool interact;

        public bool cameraRotateRight;
        public bool cameraRotateLeft;
        public bool cameraSwitch;
        public Vector3 cameraRotationInput;
        #endregion

        #region Unity API

        private void Update()
        {
            if (enableInput)
            {
#if ENABLE_LEGACY_INPUT_MANAGER

                LegacyInput();

#endif

            }
        }
        #endregion

        private void LegacyInput()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            cameraRotationInput.x = Input.GetAxis("Mouse X");
            cameraRotationInput.y = Input.GetAxis("Mouse Y");

            walkInput = Input.GetButton("Walk");
            jumpInput = Input.GetButton("Jump");
            aimTrigger = Input.GetButton("Fire2");

            attackTrigger = Input.GetButtonDown("Fire1");
            DebugCheck(attackTrigger);
            secondaryAttackTrigger = Input.GetButtonDown("Fire2");
            specialAttackTrigger = Input.GetButtonDown("Fire3");
            crouchInput = Input.GetButtonDown("Crouch");
            rollInput = Input.GetButtonDown("Fire3");
            coverInput = Input.GetButtonDown("Fire3");
            interact = Input.GetButtonDown("Interact");
            cameraRotateLeft = Input.GetButtonDown("CameraLeft");
            cameraRotateRight = Input.GetButtonDown("CameraRight");

        }

        private void DebugCheck(bool input)
        {
            if (input)
            {
                Debug.Log(input);
            }
        }
    }
}
