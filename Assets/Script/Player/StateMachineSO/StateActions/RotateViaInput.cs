using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/ Rotate Via Input")]
    public class RotateViaInput : StateAction
    {
        public TransformVariable cameraTransform;
        public float speed;
        public override void Execute(StateController controller)
        {
            if (cameraTransform == null)
            {
                return;
            }

            float h = controller.playerInput.horizontal;
            float v = controller.playerInput.vertical;

            Vector3 targetdir =  cameraTransform.value.forward * v;
            targetdir += cameraTransform.value.right * h;
            targetdir.y = 0;
            targetdir.Normalize();

            if (targetdir == Vector3.zero)
            {
                targetdir = controller.mTransform.forward;
            }

            controller.mouvementVariable.moveDirection = targetdir;

            Quaternion tr = Quaternion.LookRotation(targetdir);
            //Quaternion targetRotation = Quaternion.Slerp(controller.mTransform.rotation, tr, 
            //                            Time.deltaTime * controller.mouvementVariable.moveAmount * speed);

            controller.mouvementVariable.lookRotation = tr;
        }
    }
}
