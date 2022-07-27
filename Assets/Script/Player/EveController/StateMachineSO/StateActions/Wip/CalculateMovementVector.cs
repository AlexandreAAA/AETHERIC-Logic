using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/Calculate Mouvement Vector")]
    public class CalculateMovementVector : StateAction
    {
        public TransformVariable camTransform;
        public bool isAxisConstraint;

        public enum Axis { x, y, z }
        public Axis axisToConstraint;

        private Vector3 camFwd, camRight, fwdRelInput, rightRelInput, targetDir;
        private float h, v;

        public override void Execute(StateController controller)
        {
            h = controller.playerInput.horizontal;
            v = controller.playerInput.vertical;
            float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
            controller.isrunning = moveAmount > 0.1f;

            camFwd = camTransform.value.forward;
            camRight = camTransform.value.right;
            camFwd.y = 0;
            camRight.y = 0;
            camFwd.Normalize();
            camRight.Normalize();

            fwdRelInput = camFwd * v;
            rightRelInput = camRight * h;
            targetDir = (fwdRelInput + rightRelInput).normalized;

            if (isAxisConstraint)
            {
                targetDir = controller.mTransform.InverseTransformDirection(targetDir);

                switch (axisToConstraint)
                {
                    case Axis.x:
                        targetDir.x = 0;
                        break;
                    case Axis.y:
                        targetDir.y = 0;
                        break;
                    case Axis.z:
                        targetDir.z = 0;
                        break;
                    default:
                        break;
                }
                targetDir = controller.mTransform.TransformDirection(targetDir);
                targetDir.Normalize();
            }

            if (controller.isGrounded)
            {
                if (moveAmount > 0.1f)
                {
                    controller.rigidBody.drag = 0.0f;
                }
                else
                {
                    controller.rigidBody.drag = 4;
                }
            }
            else
            {
                controller.rigidBody.drag = 0.0f;
            }

            controller.mouvementVariable.moveAmount = moveAmount;
            //targetDir.y = 0;
            controller.mouvementVariable.moveDirection = targetDir;
            controller.mouvementVariable.forwardVector = controller.mouvementVariable.currentSpeed * moveAmount * Time.fixedDeltaTime * targetDir;
        }
    }
}
