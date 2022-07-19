using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/Cover Edge Check")]
    public class CoverEdgeCheck : StateAction
    {
        public bool debug;
        public BoolVariable leftEdge;
        public BoolVariable rightEdge;
        public float rayCastLength = 1.5f;
        public LayerMask coverLayer;

        private Transform cTransform;
        public override void Execute(StateController controller)
        {
            cTransform = controller.mTransform;

            if (controller.mouvementVariable.moveAmount > 0)
            {
                Ray _rayRight = new Ray(cTransform.position + cTransform.right * 0.6f + cTransform.up * 0.6f, cTransform.forward);

                if (debug)
                {
                    Debug.DrawRay(_rayRight.origin, _rayRight.direction * rayCastLength, Color.magenta);
                }
                RaycastHit _hitRight;
                rightEdge.value = !Physics.Raycast(_rayRight, out _hitRight, rayCastLength, coverLayer);

                Ray _rayLeft = new Ray(cTransform.position + cTransform.right * -0.6f + cTransform.up * 0.6f, cTransform.forward);

                if (debug)
                {
                    Debug.DrawRay(_rayLeft.origin, _rayLeft.direction * rayCastLength, Color.magenta);
                }
                RaycastHit _hitLeft;
                leftEdge.value = !Physics.Raycast(_rayLeft, out _hitLeft, rayCastLength, coverLayer);
            }




        }
    }
}
