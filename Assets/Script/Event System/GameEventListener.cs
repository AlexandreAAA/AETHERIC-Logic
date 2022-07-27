using UnityEngine;
using UnityEngine.Events;

namespace EveController.EventSystem
{
    public class GameEventListener : MonoBehaviour
    {
        #region Exposed
        public GameEventSO gameEvent;
        public UnityEvent onEventTriggered;
        #endregion

        #region Init
        private void OnEnable()
        {
            gameEvent.AddListener(this);
        }

        private void OnDisable()
        {
            gameEvent.RemoveListener(this);
        }

        public void OnEventTriggered()
        {
            onEventTriggered.Invoke();
        }
        #endregion
    }
}
