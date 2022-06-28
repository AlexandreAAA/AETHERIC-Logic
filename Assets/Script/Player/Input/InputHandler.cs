using UnityEngine;

namespace EveController
{

    public class InputHandler : MonoBehaviour
    {
        #region Exposed
        public bool enableInput;
        public Vector3 movementVector;
        public float moveAmount;
        public bool attackTrigger;
        public bool walkInput;
        public bool crouchInput;
        public bool rollInput;
        public bool jumpInput;
        public bool aimTrigger;
        public bool interact;

        public bool cameraRotateRight;
        public bool cameraRotateLeft;
        public bool cameraSwitch;
        public Vector3 cameraRotationInput;

        #endregion

        private void Update()
        {
            if (enableInput)
            {
#if ENABLE_LEGACY_INPUT_MANAGER

                LegacyInput();

#endif

            }
        }

        private void LegacyInput()
        {
            movementVector.x = Input.GetAxis("Horizontal");
            movementVector.z = Input.GetAxis("Vertical");
            cameraRotationInput.x = Input.GetAxis("Mouse X");
            cameraRotationInput.y = Input.GetAxis("Mouse Y");
            moveAmount = Mathf.Clamp01(movementVector.sqrMagnitude);

            walkInput = Input.GetButton("Walk");
            jumpInput = Input.GetButton("Jump");
            aimTrigger = Input.GetButton("Fire2");

            attackTrigger = Input.GetButtonDown("Fire1");
            crouchInput = Input.GetButtonDown("Crouch");
            rollInput = Input.GetButtonDown("Fire3");
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
