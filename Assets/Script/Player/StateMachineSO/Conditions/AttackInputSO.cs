using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "Input Actions/Attack Input")]
    public class AttackInputSO : Condition
    {
        public override bool CheckCondition(StateController controller)
        {
            bool retval = false;

            if (controller.playerInput.attackTrigger)
            {
                retval = true;
            }

            return retval;
        }
    }
}