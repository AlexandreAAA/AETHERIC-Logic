using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{
    [CreateAssetMenu]
    public class State : ScriptableObject
    {
        #region Exposed

        public StateAction[] onEnter;
        public StateAction[] onUpdate;
        public StateAction[] onFixedUpdate;
        public StateAction[] onExit;

        public List<Transition> transitions;

        public void OnStateEnter(StateController controller)
        {
            ExecuteActions(controller, onEnter);
        }

        public void OnStateUpdate(StateController controller)
        {
            ExecuteActions(controller, onUpdate);
            CheckTransitions(controller);
        }

        public void OnStateFixedUpdate(StateController controller)
        {
            ExecuteActions(controller, onFixedUpdate);
        }

        public void OnStateExit(StateController controller)
        {
            ExecuteActions(controller, onExit);
        }

        public void ExecuteActions(StateController controller, StateAction[] actions)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i] != null)
                {
                    actions[i].Execute(controller);
                }
            }
        }

        public void CheckTransitions(StateController controller)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].disable)
                {
                    continue;
                }

                if (transitions[i].condition.CheckCondition(controller))
                {
                    if (transitions[i].targetState != null)
                    {
                        OnStateExit(controller);
                        controller.currentState = transitions[i].targetState;
                        controller.currentState.OnStateEnter(controller);
                    }
                    return;
                }
            }
        }

        #endregion
    }
}
