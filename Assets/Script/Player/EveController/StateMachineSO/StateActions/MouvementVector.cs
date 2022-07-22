using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/Mouvement Vector")]
    public class MouvementVector : StateAction
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

            if (moveAmount > 0.1f)
            {
                controller.rigidBody.drag = 0.0f;
            }
            else
            {
                controller.rigidBody.drag = 4;
            }

            controller.mouvementVariable.moveAmount = moveAmount;
            controller.mouvementVariable.moveDirection = targetDir;
        }
    }
}
