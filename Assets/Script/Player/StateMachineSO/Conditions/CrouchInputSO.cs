using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="Input Actions/Crouch Input")]
    public class CrouchInputSO : Condition
    {
        public override bool CheckCondition(StateController controller)
        {
            bool retval = false;

            if (controller.playerInput.crouchInput)
            {
                retval = true;
            }

            return retval;
        }
    }
}
