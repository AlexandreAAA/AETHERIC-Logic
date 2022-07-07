using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/On Enter Attack")]
    public class OnEnterAttack : StateAction
    {
        public FloatVariable comboTime;
        public FloatVariable timeToDropCombo;
        public override void Execute(StateController controller)
        {
            controller.mouvementVariable.currentSpeed = 0f;

            controller.rigidBody.velocity = Vector3.zero;
            controller.isComboing = true; 
            comboTime.value = timeToDropCombo.value;
            controller.anim.SetBool("IsCombo", controller.isComboing);
        }
    }
}
