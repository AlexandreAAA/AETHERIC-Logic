using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/ Move Amount Polling")]
    public class MoveAmountPolling : Condition
    {
        public override bool CheckCondition(StateController controller )
        {

            if (controller.playerInput.moveAmount > 0.1f)
            {
                return true;
            }
            return false;
        }
    }
}
