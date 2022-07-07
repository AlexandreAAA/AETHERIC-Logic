using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/Roll Checker")]
    public class RollChecker : StateAction
    {
        public FloatVariable timeRollFinish;
        public override void Execute(StateController controller)
        {
            controller.isRolling = Time.timeSinceLevelLoad < timeRollFinish.value;
        }
    }
}
