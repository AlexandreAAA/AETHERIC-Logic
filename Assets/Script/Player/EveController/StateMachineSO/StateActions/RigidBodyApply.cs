using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/RigidBodyApply")]
    public class RigidBodyApply : StateAction
    {
        public override void Execute(StateController controller)
        {
            controller.mouvementVariable.forwardVector.y = controller.rigidBody.velocity.y;
            controller.rigidBody.velocity = controller.mouvementVariable.forwardVector;
        }
    }
}
