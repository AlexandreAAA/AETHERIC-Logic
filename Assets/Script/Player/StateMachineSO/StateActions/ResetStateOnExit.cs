using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/Reset State On Exit")]
    public class ResetStateOnExit : StateAction
    {
        public override void Execute(StateController controller)
        {
            controller.isrunning = false;
            controller.anim.SetBool("IsRunning", false);
        }
    }
}
