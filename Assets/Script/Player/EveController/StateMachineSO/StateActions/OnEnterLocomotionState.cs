using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/On Enter Locomotion State")]
    public class OnEnterLocomotionState : StateAction
    {
        public enum LocomotionState
        {
            Base = 1 << 0,
            Crouch = 1 << 1,
            Cover = 1 << 2,
            Casting = 1 << 3
        }

        public LocomotionState currentState;
        public FloatVariable stateSpeed;

        private Animator anim;

        public override void Execute(StateController controller)
        {
            if (controller.anim)
            {
                anim = controller.anim;
            }

            switch (currentState)
            {
                case LocomotionState.Base:
                    break;
                case LocomotionState.Crouch:
                    controller.isCrouching = true;
                    anim.SetBool("IsCrouching", controller.isCrouching);
                    break;
                case LocomotionState.Cover:
                    controller.isInCover = true;
                    anim.SetBool("TakeCover", controller.isInCover);
                    break;
                case LocomotionState.Casting:
                    controller.isCasting = true;
                    anim.SetBool("IsCasting", controller.isCasting);
                    break;
                default:
                    break;
            }

            controller.mouvementVariable.currentSpeed = stateSpeed.value;
        }
    }
}
