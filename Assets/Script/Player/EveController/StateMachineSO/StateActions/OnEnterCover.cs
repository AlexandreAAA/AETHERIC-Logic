using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/On Enter Cover ")]
    public class OnEnterCover : StateAction
    {
        public Vector3Variable coverPosition;
        public QuaternionVariable coverRotation;

        public override void Execute(StateController controller)
        {
            controller.isInCover = true;
            controller.rigidBody.rotation = coverRotation.value;
            controller.anim.SetBool("TakeCover", controller.isInCover);
        }

    }
}
