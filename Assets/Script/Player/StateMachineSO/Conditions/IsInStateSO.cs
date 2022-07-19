using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="Conditions/Is In State")]
    public class IsInStateSO : Condition
    {
        public BoolVariable inState;
        public override bool CheckCondition(StateController controller)
        {
            bool retval = false;

            if (!inState.value)
            {
                retval = true;
            }

            return retval;


        }
    }
}
