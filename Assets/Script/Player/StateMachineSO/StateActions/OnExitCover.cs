using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/OnExitCover")]
    public class OnExitCover : StateAction
    {
        public override void Execute(StateController controller)
        {
            controller.isInCover = false;
            controller.anim.SetBool("TakeCover", controller.isInCover);
        }
    }
}
