using UnityEngine;

namespace EveController
{
    [CreateAssetMenu (menuName ="State Actions/ MoveViaInput")]
    public class MoveViaInput : StateAction
    {
        public override void Execute(StateController controller)
        {
            float h = controller.playerInput.horizontal;
            float v = controller.playerInput.vertical;

            float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
            controller.isrunning = moveAmount > 0.1f;
            controller.mouvementVariable.moveAmount = moveAmount;

            if (moveAmount > 0.1f)
            {
                controller.rigidBody.drag = 0.0f;
            }
            else
            {
                controller.rigidBody.drag = 4;
            }

            controller.anim.SetFloat("DirMag", moveAmount);
            controller.anim.SetBool("IsRunning", controller.isrunning);

            controller.mouvementVariable.forwardVector = controller.mouvementVariable.currentSpeed * controller.mouvementVariable.moveAmount * Time.fixedDeltaTime * controller.mTransform.forward;
        }
    }
}


