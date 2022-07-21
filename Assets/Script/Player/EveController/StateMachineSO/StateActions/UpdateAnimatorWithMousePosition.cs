using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/Update Animator With Mouse Position")]
    public class UpdateAnimatorWithMousePosition : StateAction
    {
        public Vector3Variable lookedAtPoint;

        private Transform cTransform;
        private float forwardBackwardsMagnitude, rightLeftMagnitude;
        public override void Execute(StateController controller)
        {
            cTransform = controller.mTransform;
            forwardBackwardsMagnitude = 0;
            rightLeftMagnitude = 0;

            if (controller.mouvementVariable.moveDirection.magnitude > 0)
            {
                Vector3 normalizedLookingAt = lookedAtPoint.value - cTransform.position;
                normalizedLookingAt.Normalize();

                forwardBackwardsMagnitude = Mathf.Clamp(Vector3.Dot(controller.mouvementVariable.moveDirection.normalized, normalizedLookingAt), -1, 1);

                Vector3 perpendicularLookingAt = new Vector3(normalizedLookingAt.z, 0, -normalizedLookingAt.x);

                rightLeftMagnitude = Mathf.Clamp(Vector3.Dot(controller.mouvementVariable.moveDirection.normalized, perpendicularLookingAt), -1, 1);

                controller.anim.SetBool("IsMoving", true);
            }
            else
            {
                controller.anim.SetBool("IsMoving", false);
            }

            // update the animator parameters
            controller.anim.SetFloat("VeloZ", forwardBackwardsMagnitude);
            controller.anim.SetFloat("VeloX", rightLeftMagnitude);
        }
    }
}
