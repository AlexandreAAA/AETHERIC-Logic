using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "Conditions/Aim Input SO")]
    public class AimInputSO : Condition
    {
        public bool isEnter;
        public override bool CheckCondition(StateController controller)
        {
            bool retval = isEnter ? controller.playerInput.aimTrigger : !controller.playerInput.aimTrigger;
            return retval;
        }
    }
}
