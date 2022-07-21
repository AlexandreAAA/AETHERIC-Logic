using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    public class CameraVariableAssign : MonoBehaviour
    {
        public CameraVariable mainCamera;

        private void Start()
        {
            mainCamera.value = Camera.main;

            Destroy(this);
        }
    }
}
