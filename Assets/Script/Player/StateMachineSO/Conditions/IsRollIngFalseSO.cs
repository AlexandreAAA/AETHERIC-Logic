using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="Conditions/Is Rolling False")]
    public class IsRollIngFalseSO : Condition
    {
        public override bool CheckCondition(StateController controller)
        {
            bool retval = false;
            if (!controller.isRolling)
            {
                retval = true;
            }
            return retval;
        }
    }
}
