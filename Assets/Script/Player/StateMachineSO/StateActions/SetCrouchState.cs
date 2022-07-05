using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/Set Crouch State") ]
    public class SetCrouchState : StateAction
    {
        public BoolVariable isCrouch;
        public override void Execute(StateController controller)
        {
            controller.isCrouching = isCrouch.value;
            controller.anim.SetBool("IsCrouching", isCrouch.value);
        }
    }
}
