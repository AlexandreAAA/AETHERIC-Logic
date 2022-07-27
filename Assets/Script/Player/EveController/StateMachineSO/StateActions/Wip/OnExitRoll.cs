using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/OnExitRoll")]
    public class OnExitRoll : StateAction
    {
        public override void Execute(StateController controller)
        {
            controller.anim.SetBool("IsRolling", false);
        }
    }
}
