using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/On Exit Attack")]
    public class OnExitAttack : StateAction
    {
        public float smoothRot = 7.0f;
        public override void Execute(StateController controller)
        {
            controller.isComboing = false;
            controller.anim.ResetTrigger("Attack");
            controller.anim.SetBool("IsCombo", controller.isComboing);
            controller.mouvementVariable.lookRotation = Quaternion.RotateTowards(
                                                        controller.rigidBody.rotation, 
                                                        controller.mouvementVariable.lookRotation, 
                                                        smoothRot);
        }
    }
}
