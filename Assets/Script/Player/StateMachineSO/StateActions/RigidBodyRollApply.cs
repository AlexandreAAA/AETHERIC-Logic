using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/RigidBodyRollApply")]
    public class RigidBodyRollApply : StateAction
    {
        public override void Execute(StateController controller)
        {
            controller.rigidBody.velocity = controller.mouvementVariable.currentSpeed *
                                            controller.mouvementVariable.moveDirection *
                                            Time.fixedDeltaTime;
        }
    }
}
