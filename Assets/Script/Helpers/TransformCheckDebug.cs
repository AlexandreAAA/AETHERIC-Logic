using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCheckDebug : MonoBehaviour
{
    public Vector3 vectorToCheck;
    public Vector3 vectorTocheckInverse;
    public Vector3 vectorToCheckTransformVector;
    private Transform m_transform;

    private void Start()
    {
        m_transform = this.transform;

    }

    private void Update()
    {
        vectorToCheck = m_transform.right;
        vectorTocheckInverse = m_transform.InverseTransformVector(vectorToCheck);
        vectorToCheckTransformVector = m_transform.TransformVector(vectorToCheck);
    }
}
