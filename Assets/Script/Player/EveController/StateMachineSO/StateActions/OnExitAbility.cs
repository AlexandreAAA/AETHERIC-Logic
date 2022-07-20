using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/On Exit Ability")]
    public class OnExitAbility : StateAction
    {
        public enum AbilityType
        {
            MeleeAttack = 1 << 0,
            CastingAttack = 1 << 1,
            JumpAbility = 1 << 2
        }

        public AbilityType selectedType;
        public bool isStop;
        
        public override void Execute(StateController controller)
        {

            if (isStop)
            {
                controller.rigidBody.velocity = Vector3.zero;
            }

            switch (selectedType)
            {
                case AbilityType.MeleeAttack:
                    controller.isComboing = false;
                    break;
                case AbilityType.CastingAttack:
                    controller.castingAttack = false;
                    break;
                case AbilityType.JumpAbility:
                    controller.isJumping = false;
                    break;
                default:
                    break;
            }

            
        }
    }
}