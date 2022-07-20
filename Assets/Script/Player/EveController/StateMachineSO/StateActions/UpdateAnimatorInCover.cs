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
        public float smooth;
        private float moveX;
        public bool left;
        public override void Execute(StateController controller)
        {
            //moveX = controller.playerInput.horizontal;

            Vector3 velocity = controller.mTransform.InverseTransformVector(controller.rigidBody.velocity);
            float veloX = Mathf.Clamp(velocity.x, -1, 1);

            moveX = veloX * controller.mouvementVariable.moveAmount;

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

            if (leftEdge.value)
            {
                moveX = Mathf.Lerp(moveX, -0.1f, smooth * Time.deltaTime);
            }
            else if (rightEdge.value)
            {
                moveX = Mathf.Lerp(moveX, 0.1f, smooth * Time.deltaTime);
            }
            controller.anim.SetFloat("MoveX", moveX);
        }
    }
}
