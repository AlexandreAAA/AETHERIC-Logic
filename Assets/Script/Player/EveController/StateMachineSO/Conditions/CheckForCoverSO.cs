using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "Conditions/Check For Cover SO")]
    public class CheckForCoverSO : Condition
    {
        public bool debug;
        public float rayCastLength = 1.5f;
        public LayerMask coverLayer;
        public float offset = 0.3f;
        public Vector3Variable coverPosition;
        public QuaternionVariable coverRotation;
        public override bool CheckCondition(StateController controller)
        {
            bool retval = false;

            if (controller.playerInput.coverInput)
            {
                Ray _ray = new Ray(controller.mTransform.position + Vector3.up, controller.mTransform.forward);

                if (debug)
                {
                    Debug.DrawRay(_ray.origin, _ray.direction * rayCastLength, Color.green);
                }

                if (Physics.Raycast(_ray, out RaycastHit _hit, rayCastLength, coverLayer))
                {
                    coverPosition.value = _hit.point + (_hit.normal * offset);
                    Quaternion _toRotation = Quaternion.LookRotation(-_hit.normal);
                    coverRotation.value = _toRotation;
                    retval = true;
                }
            }

            return retval;
        }
    }
}
