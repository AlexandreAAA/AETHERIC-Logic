using UnityEngine;


namespace EveController
{
    [CreateAssetMenu (menuName ="State Actions/ MoveViaInputUnderCover")]
    public class MoveViaInputUnderCover : StateAction
    {
        
        
        public override void Execute(StateController controller)
        {
            float h = controller.playerInput.horizontal;
            float v = controller.playerInput.vertical;


            float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
            controller.isrunning = moveAmount > 0.1f;
            controller.mouvementVariable.moveAmount = moveAmount;
            
            Vector3 playerRight = (controller.mTransform.right) * h;
            Vector3 moveDirection = playerRight.normalized;
            
            
            if (moveAmount > 0.1f)
            {
                controller.rigidBody.drag = 0.0f;
            }
            else
            {
                controller.rigidBody.drag = 4;
            }

            controller.mouvementVariable.moveDirection = moveDirection * controller.mouvementVariable.moveAmount;
        }
    }
}


