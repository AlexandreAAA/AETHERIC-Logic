using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/OnEnterRoll")]
    public class OnEnterRoll : StateAction
    {
        public float rollSpeed = 630;
        public float rollDuration = 0.8f;
        public FloatVariable timeRollFinish;
        public AudioClip _rollSound;
        public override void Execute(StateController controller)
        {
            controller.mouvementVariable.currentSpeed = rollSpeed;
            timeRollFinish.value = Time.timeSinceLevelLoad + rollDuration;
            controller.anim.SetBool("IsRolling", true);
            controller.audioSource.PlayOneShot(_rollSound, 0.5f);
        }
    }
}
