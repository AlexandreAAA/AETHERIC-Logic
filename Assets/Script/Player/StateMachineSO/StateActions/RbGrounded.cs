using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/Rb Grounded")]
    public class RbGrounded : StateAction
    {
        public override void Execute(StateController controller)
        {
            if (controller.isGrounded)
            {
                controller.rigidBody.useGravity = false;
                Vector3 _position = controller.mTransform.position;
                _position.y = controller.mouvementVariable.averageGroundHeight.y;
                controller.rigidBody.MovePosition(_position);
            }
            else
            {
                controller.rigidBody.useGravity = true;
            }
        }
    }
}
