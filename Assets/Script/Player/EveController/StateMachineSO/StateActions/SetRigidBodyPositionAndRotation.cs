using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/Set RB pos and rot")]
    public class SetRigidBodyPositionAndRotation : StateAction
    {
        public Vector3Variable position;
        public QuaternionVariable rotation;
        public bool setPosition;
        public bool setRotation;

        public override void Execute(StateController controller)
        {
            if (setPosition)
            {
                controller.rigidBody.MovePosition(new Vector3(position.value.x, controller.mTransform.position.y, position.value.z));
            }

            if (setRotation)
            {
                controller.rigidBody.rotation = rotation.value;
            }
        }
    }
}
