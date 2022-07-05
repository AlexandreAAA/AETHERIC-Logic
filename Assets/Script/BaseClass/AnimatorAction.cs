using UnityEngine;

namespace EveController
{
    public abstract class AnimatorAction : StateAction
    {
        public abstract void Execute(Animator animator);
    }
}
