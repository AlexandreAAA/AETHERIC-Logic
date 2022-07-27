using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/RigidBodyRollApply")]
    public class RigidBodyRollApply : StateAction
    {
        public override void Execute(StateController controller)
        {
            Vector3 velocity = controller.mouvementVariable.currentSpeed * Time.fixedDeltaTime * controller.mouvementVariable.moveDirection;
            velocity.y = controller.rigidBody.velocity.y;
            controller.rigidBody.velocity = velocity;
        }
    }
}
