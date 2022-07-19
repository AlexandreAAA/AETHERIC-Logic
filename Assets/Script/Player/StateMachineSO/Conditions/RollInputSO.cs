using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "Input Actions/Roll Input")]
    public class RollInputSO : Condition
    {
        public override bool CheckCondition(StateController controller)
        {
            bool retval = false;

            if (controller.playerInput.rollInput)
            {
                retval = true;
            }

            return retval;
        }
    }
}
