using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/GroundChecker")]
    public class GroundChecker : StateAction
    {
        public bool debug;
        public float _rayOffset;
        public float _raycastLength;
        public float _heightOffset;
        public LayerMask _groundLayer;
        private readonly Ray[] _groundDetectRaycasts = new Ray[5];

        public override void Execute(StateController controller)
        {
            _groundDetectRaycasts[0] = new Ray(controller.mTransform.position + Vector3.up, Vector3.down);
            _groundDetectRaycasts[1] = new Ray(controller.mTransform.position + Vector3.up + (Vector3.forward * _rayOffset), Vector3.down);
            _groundDetectRaycasts[2] = new Ray(controller.mTransform.position + Vector3.up - (Vector3.forward * _rayOffset), Vector3.down);
            _groundDetectRaycasts[3] = new Ray(controller.mTransform.position + Vector3.up + (Vector3.right * _rayOffset), Vector3.down);
            _groundDetectRaycasts[4] = new Ray(controller.mTransform.position + Vector3.up - (Vector3.right * _rayOffset), Vector3.down);

            int _hitCount = 0;
            controller.mouvementVariable.averageGroundHeight = Vector3.zero;

            for (int i = 0; i < _groundDetectRaycasts.Length; i++)
            {
                Ray _currentRay = _groundDetectRaycasts[i];

                if (debug)
                {
                    Debug.DrawRay(_currentRay.origin, _currentRay.direction * _raycastLength, Color.blue);
                }

                if (Physics.Raycast(_currentRay, out RaycastHit _hit, _raycastLength, _groundLayer))
                {
                    controller.mouvementVariable.averageGroundHeight += _hit.point;
                    _hitCount++;
                    controller.isGrounded = true;
                }
            }

            if (_hitCount > 0)
            {
                controller.mouvementVariable.averageGroundHeight /= _hitCount;
                controller.mouvementVariable.averageGroundHeight.y += _heightOffset;
            }

            if (_hitCount == 0)
            {
                controller.isGrounded = false;
            }
        }
    }
}
