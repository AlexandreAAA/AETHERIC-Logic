using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/Constraint Movement In cover")]
    public class ConstraintMovementInCover : StateAction
    {
        public FloatVariable coverSpeed;
        public BoolVariable LeftEdgeCheck, rightEdgeCheck;

        private Vector3 targetDir;
        public override void Execute(StateController controller)
        {
            targetDir = controller.mouvementVariable.moveDirection;
            float dotProd = Mathf.Clamp(Vector3.Dot(controller.mTransform.right, targetDir), -1, 1);

            if (LeftEdgeCheck.value && dotProd < -0.9f)
            {
                controller.mouvementVariable.currentSpeed = 0;
            }
            else if (rightEdgeCheck.value && dotProd > 0.9f)
            {
                controller.mouvementVariable.currentSpeed = 0;
            }
            else
            {
                controller.mouvementVariable.currentSpeed = coverSpeed.value;
            }
        }
    }
}
