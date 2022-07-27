using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu(menuName ="State Actions/Enable GO")]
    public class EnableGO : StateAction
    {
        public GameObjectVariable obj;
        public bool enable;
        public override void Execute(StateController controller)
        {
            obj.value.SetActive(enable);
        }
    }
}
