using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftLockSimple : MonoBehaviour
{
    public PlayerController _playerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _playerController.m_ennemyTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _playerController.m_ennemyTransform = null;
        }
    }
}
