using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/On Enter Ability")]
    public class OnEnterAbility : StateAction
    {
        public enum AbilityType 
        {
            MeleeAttack = 1 << 0,
            CastingAttack = 1 << 1, 
            JumpAbility = 1 << 2
        }

        public AbilityType selectedType;
        public BoolVariable inState;
        public bool isStop;
        public string animState;
        public float crossfadeSmooth;


        public override void Execute(StateController controller)
        {

            if (isStop)
            {
                controller.rigidBody.velocity = Vector3.zero;
            }

            inState.value = true;

            switch (selectedType)
            {
                case AbilityType.MeleeAttack:
                    controller.isComboing = true;
                    break;
                case AbilityType.CastingAttack:
                    controller.castingAttack = true;
                    break;
                case AbilityType.JumpAbility:
                    controller.isJumping = true;
                    break;
                default:
                    break;
            }

            controller.anim.CrossFadeInFixedTime(animState, crossfadeSmooth);
        }
    }
}
