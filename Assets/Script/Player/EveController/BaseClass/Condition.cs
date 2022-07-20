using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool CheckCondition(StateController controller);
    }
}
