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
        public bool left;
        private float moveX;
        public override void Execute(StateController controller)
        {
            Vector3 velocity = controller.mTransform.InverseTransformVector(controller.rigidBody.velocity);
            float veloX = Mathf.Clamp(velocity.x, -1, 1);

            float moveAmount = controller.mouvementVariable.moveAmount;
            moveX = veloX * moveAmount;

            if (moveX <= -0.1f)
            {
                left = true;
            }
            else if (moveX >= 0.1f)
            {
                left = false;
            }

            if (moveAmount < 0.1)
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
