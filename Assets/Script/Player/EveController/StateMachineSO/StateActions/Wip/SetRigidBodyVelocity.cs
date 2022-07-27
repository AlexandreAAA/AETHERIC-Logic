using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/Set RigidBody Velocity")]
    public class SetRigidBodyVelocity : StateAction
    {
        public override void Execute(StateController controller)
        {
            if (controller.isGrounded)
            {
                controller.mouvementVariable.forwardVector.y = 0;
            }
            else
            {
                controller.mouvementVariable.forwardVector.y = controller.rigidBody.velocity.y;
            }

            controller.rigidBody.velocity = controller.mouvementVariable.forwardVector;
        }
    }
}
