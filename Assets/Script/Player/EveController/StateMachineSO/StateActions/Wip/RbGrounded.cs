using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/Rb Grounded")]
    public class RbGrounded : StateAction
    {
        public float speed;
        public override void Execute(StateController controller)
        {
            if (controller.isGrounded)
            {
                controller.rigidBody.useGravity = false;
                Vector3 _position = controller.rigidBody.position;
                _position.y = controller.mouvementVariable.averageGroundHeight.y;
                _position.y = Mathf.MoveTowards(_position.y, controller.mouvementVariable.averageGroundHeight.y, speed * Time.deltaTime);
                controller.rigidBody.position = _position;
            }
            else
            {
                controller.rigidBody.useGravity = true;
            }
        }
    }
}
