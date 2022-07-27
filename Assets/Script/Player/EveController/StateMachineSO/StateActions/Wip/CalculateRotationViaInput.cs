using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/Calculate Rotation Via Input")]
    public class CalculateRotationViaInput : StateAction
    {
        private Vector3 targetDir;
        public override void Execute(StateController controller)
        {
            targetDir = controller.mouvementVariable.moveDirection;

            if (targetDir == Vector3.zero)
            {
                targetDir = controller.mTransform.forward;
            }

            controller.mouvementVariable.moveDirection = targetDir;

            Quaternion tr = Quaternion.LookRotation(targetDir);

            //tr = Quaternion.RotateTowards(controller.mTransform.rotation, tr,
            //                           Time.deltaTime * 500);

            controller.mouvementVariable.lookRotation = tr;
        }
    }
}
