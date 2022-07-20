using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/CoverMouvementVector")]
    public class CoverMouvementVector : StateAction
    {
        public TransformVariable camTransform;
        public BoolVariable leftEdgeCheck, rightEdgeCheck;
        public bool LeftCheck, rightCheck;
        public float coverSpeed;

        private Vector3 camFwd, camRight, fwdRelInput, rightRelInput, targetDir, velocity;
        private float h, v, veloX;

        public override void Execute(StateController controller)
        {
            h = controller.playerInput.horizontal;
            v = controller.playerInput.vertical;
            float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

            LeftCheck = leftEdgeCheck.value;
            rightCheck = rightEdgeCheck.value;

            camFwd = camTransform.value.forward;
            camRight = camTransform.value.right;
            camFwd.y = 0;
            camRight.y = 0;
            camFwd.Normalize();
            camRight.Normalize();

            fwdRelInput = camFwd * v;
            rightRelInput = camRight * h;
            targetDir = (fwdRelInput + rightRelInput).normalized;
            targetDir = controller.mTransform.InverseTransformDirection(targetDir);
            targetDir.z = 0;
            targetDir = controller.mTransform.TransformDirection(targetDir);
            targetDir.Normalize();



            if (LeftCheck && h < 0)
            {
                
                controller.mouvementVariable.currentSpeed = 0;
            }
            else if (rightCheck && h > 0)
            {
                controller.mouvementVariable.currentSpeed = 0;
            }
            else
            {
                controller.mouvementVariable.currentSpeed = coverSpeed;
            }

            controller.mouvementVariable.moveAmount = moveAmount;
            controller.mouvementVariable.moveDirection = targetDir;


        }
    }
}
