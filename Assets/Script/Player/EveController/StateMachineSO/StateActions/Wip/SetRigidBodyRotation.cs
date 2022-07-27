using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/Set RigidBody Rotation")]
    public class SetRigidBodyRotation : StateAction
    {
        public override void Execute(StateController controller)
        {
            controller.rigidBody.MoveRotation(controller.mouvementVariable.lookRotation.normalized);
        }
    }
}
