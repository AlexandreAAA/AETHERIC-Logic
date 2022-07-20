using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/SwitchCollider")]
    public class SwitchCollider : StateAction
    {
        public bool switchCollider = false;

        public override void Execute(StateController controller)
        {
            if (switchCollider)
            {
                controller.capsCollider.enabled = false;
                controller.stealthCollider.enabled = true;
            }
            else
            {
                controller.capsCollider.enabled = true;
                controller.stealthCollider.enabled = false;

            }
        }
    }
}
