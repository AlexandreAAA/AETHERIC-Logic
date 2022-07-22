using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    public class TransformVariableAssign : MonoBehaviour
    {
        public TransformVariable _transform;

        private void Start()
        {
            _transform.value = this.transform;

            Destroy(this);
        }
    }
}
