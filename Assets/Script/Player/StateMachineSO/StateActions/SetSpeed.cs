using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/Set Speed")]
    public class SetSpeed : StateAction
    {
        public float speed;
        public override void Execute(StateController controller)
        {
            controller.mouvementVariable.currentSpeed = speed;
        }
    }
    
}
