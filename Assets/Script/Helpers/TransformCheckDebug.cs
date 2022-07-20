using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    public class TransformCheckDebug : MonoBehaviour
    {
        public Vector3 vectorToCheck;
        public Vector3 moveDir;
        public float dotProd;
        private Transform m_transform;
        private StateController controller;

        private void Start()
        {
            m_transform = this.transform;
            controller = GetComponent<StateController>();
        }

        private void Update()
        {
            vectorToCheck = m_transform.right;
            moveDir = controller.mouvementVariable.moveDirection;
            dotProd = Mathf.Clamp(Vector3.Dot(vectorToCheck, moveDir), - 1, 1);
        }
    }
}
