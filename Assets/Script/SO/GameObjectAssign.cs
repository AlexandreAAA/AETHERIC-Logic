using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    public class GameObjectAssign : MonoBehaviour
    {
        public GameObjectVariable obj;

        private void Awake()
        {
            obj.value = this.gameObject;
        }
    }
}
