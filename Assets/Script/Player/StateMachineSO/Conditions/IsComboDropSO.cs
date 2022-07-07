using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="Conditions/Is Combo Drop")]
    public class IsComboDropSO : Condition
    {
        public FloatVariable comboTime;
        public override bool CheckCondition(StateController controller)
        {
            bool retval = false;

            if (comboTime.value < 0)
            {
                retval = true;
            }

            return retval;
        }
    }
}
