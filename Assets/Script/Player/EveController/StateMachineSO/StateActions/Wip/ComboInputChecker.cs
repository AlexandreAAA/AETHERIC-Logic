using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/Combo Input Checker")]
    public class ComboInputChecker : StateAction
    {
        public override void Execute(StateController controller)
        {
            controller.anim.SetFloat("State Time", Mathf.Repeat(controller.anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
            controller.anim.ResetTrigger("Melee Attack");

            if (controller.playerInput.attackTrigger)
            {
                controller.anim.SetTrigger("Melee Attack");
                
            }

        }

        //public TransformVariable launcher;

        //public void LaunchFireball()
        //{
        //    GameObject fireball = PlayerObjectPool.SharedInstance.GetPooledObject(PlayerObjectPool.SharedInstance.fireball);
        //    fireball.transform.SetPositionAndRotation(launcher.value.position, launcher.value.rotation);
        //    fireball.SetActive(true);
        //}
    }
}
