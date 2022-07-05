using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu]
    public class StateDataContainer : ScriptableObject
    {
        public List<ScriptableObject> stateDatas = new List<ScriptableObject>();

        public ScriptableObject Get(ScriptableObject SO)
        {
            if (stateDatas.Contains(SO))
            {
                return SO;
            }
            else
            {
                return null;
            }
        }
    }
}
