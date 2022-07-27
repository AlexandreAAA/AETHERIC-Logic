using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/Update Animator")]
    public class UpdateAnimator : StateAction
    {
        public override void Execute(StateController controller)
        {
            controller.anim.SetFloat("DirMag", controller.mouvementVariable.moveAmount);
            controller.anim.SetBool("IsRunning", controller.isrunning);
        }
    }
}
