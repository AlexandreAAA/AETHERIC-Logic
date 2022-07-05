using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/ Move Based On camera")]
    public class MoveBasedOnCamera : StateAction
    {
        public float runningSpeed;
        public TransformVariable cameraTransform;
        private float h, v;
        private Vector3 forward, right, forwardRelativeInput, rightRelativeInput;

        public override void Execute(StateController controller)
        {
            if (cameraTransform == null)
            {
                return;
            }

            h = controller.playerInput.horizontal;
            v = controller.playerInput.vertical;

            float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
            controller.isrunning = moveAmount > 0.1f;
            controller.mouvementVariable.moveAmount = moveAmount;

            if (moveAmount > 0.1f)
            {
                controller.rigidBody.drag = 0.0f;
            }
            else
            {
                controller.rigidBody.drag = 4;
            }


            forward = controller.mTransform.InverseTransformVector(cameraTransform.value.forward);
            right = controller.mTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            forwardRelativeInput = v * forward;
            rightRelativeInput = h * right;

            Vector3 targetDir = (forwardRelativeInput + rightRelativeInput).normalized;
            if (targetDir == Vector3.zero)
            {
                targetDir = controller.mTransform.forward;
            }

            controller.mouvementVariable.moveDirection = targetDir;
            controller.mouvementVariable.lookRotation = Quaternion.LookRotation(controller.mouvementVariable.moveDirection.normalized);
            

            controller.anim.SetFloat("DirMag", moveAmount);
            controller.anim.SetBool("IsRunning", controller.isrunning);
        }
    }
}
