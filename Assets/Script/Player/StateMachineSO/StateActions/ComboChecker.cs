using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/Combo Checker")]
    public class ComboChecker : StateAction
    {
        public FloatVariable comboTime;
        public FloatVariable timeToDropCombo;
        public override void Execute(StateController controller)
        {
            comboTime.value -= Time.deltaTime;

            if (controller.playerInput.attackTrigger)
            {
                comboTime.value = timeToDropCombo.value;
                controller.anim.SetTrigger("Attack");
            }
        }
    }
}
