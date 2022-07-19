using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/Update Animator In Cover")]
    public class UpdateAnimatorInCover : StateAction
    {
        public BoolVariable leftEdge;
        public BoolVariable rightEdge;
        private float moveX;
        public bool left;
        public override void Execute(StateController controller)
        {
            moveX = controller.playerInput.horizontal;

            if (moveX <= -0.1f)
            {
                left = true;
            }
            else if (moveX >= 0.1f)
            {
                left = false;
            }


            if (controller.mouvementVariable.moveAmount < 0.1)
            {
                if (left)
                {
                    moveX = -0.1f;
                }
                else
                {
                    moveX = 0.1f;
                }
            }

            controller.anim.SetFloat("MoveX", moveX);
        }
    }
}
