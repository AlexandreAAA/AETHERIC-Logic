using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    public abstract class StateAction : ScriptableObject
    {
        public abstract void Execute(StateController controller);
    }
}
