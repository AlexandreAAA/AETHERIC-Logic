using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/On Enter Or Exit Locomotion State")]
    public class OnEnterOrExitLocomotionState : StateAction
    {
        public enum LocomotionState
        {
            Base = 1 << 0,
            Crouch = 1 << 1,
            Cover = 1 << 2,
            Casting = 1 << 3
        }
        public LocomotionState selectedState;

        public enum ActionType
        {
            IsEnter = 1 << 0,
            IsExit = 1 << 1
        }
        public ActionType isEnterOrExit;

        private bool state;
        private Animator anim;
        public FloatVariable speedState;

        public override void Execute(StateController controller)
        {
            if (controller.anim)
            {
                anim = controller.anim;
            }

            switch (isEnterOrExit)
            {
                case ActionType.IsEnter:
                    state = true;
                    controller.mouvementVariable.currentSpeed = speedState.value;
                    break;
                case ActionType.IsExit:
                    state = false;
                    break;
                default:
                    break;
            }

            switch (selectedState)
            {
                case LocomotionState.Base:
                    if (isEnterOrExit == ActionType.IsExit)
                    {
                        controller.isrunning = state;
                        anim.SetBool("IsRunning", controller.isrunning);
                    }
                    break;
                case LocomotionState.Crouch:
                    controller.isCrouching = state;
                    anim.SetBool("IsCrouching", controller.isCrouching);
                    break;
                case LocomotionState.Cover:
                    controller.isInCover = state;
                    anim.SetBool("TakeCover", controller.isInCover);
                    break;
                case LocomotionState.Casting:
                    controller.isCasting = state;
                    anim.SetBool("IsCasting", controller.isCasting);
                    break;
                default:
                    break;
            }
        }
    }
}
