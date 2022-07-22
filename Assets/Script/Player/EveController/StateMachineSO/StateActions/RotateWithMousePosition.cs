using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName = "State Actions/Rotate With Mouse Position")]
    public class RotateWithMousePosition : StateAction
    {
        public CameraVariable mainCam;
        public Vector3Variable lookAtPoint;
        public LayerMask targetingLayer;
        public bool debug;
        public override void Execute(StateController controller)
        {
            Ray _mouseRay = mainCam.value.ScreenPointToRay(Input.mousePosition);

            if (debug)
            {
                Debug.DrawRay(_mouseRay.origin, _mouseRay.direction * 100f, Color.green);
            }

            if (Physics.Raycast(mainCam.value.ScreenPointToRay(Input.mousePosition), out RaycastHit _hit, 100, targetingLayer))
            {
                lookAtPoint.value = _hit.point;
                Vector3 _lookdirection = _hit.point - controller.mTransform.position;
                _lookdirection.y = 0f;
                _lookdirection.Normalize();

                controller.mouvementVariable.lookRotation = Quaternion.LookRotation(_lookdirection);
            }
        }
    }
}
