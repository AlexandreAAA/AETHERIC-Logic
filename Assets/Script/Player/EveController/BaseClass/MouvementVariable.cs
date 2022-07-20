using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EveController
{
    [Serializable]
    public class MouvementVariable
    {
        public Vector3 moveDirection;
        public Vector3 forwardVector;
        public Vector3 averageGroundHeight;
        public Quaternion lookRotation;
        public float moveAmount;
        public float currentSpeed;
    }
}
