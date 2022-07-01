using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController
{

    [RequireComponent(typeof(Rigidbody))]
    public class Motor : MonoBehaviour
    {
        #region Exposed

        [SerializeField] protected Rigidbody m_rigidbody;

        #endregion

        #region Init
        protected virtual void Start()
        {
            m_rigidbody = this.GetComponent<Rigidbody>();
        }
        #endregion

        #region Method
        public virtual void SetDrag(float drag) => m_rigidbody.drag = drag;
        public virtual void SetKinematic(bool isKinematic) => m_rigidbody.isKinematic = isKinematic;
        public virtual void SetVelocity(Vector3 velocity, float speed) 
        => m_rigidbody.velocity = velocity * speed * Time.fixedDeltaTime;
        public virtual void SetRotation(Quaternion lookRotation)
        => m_rigidbody.MoveRotation(lookRotation);
        #endregion




    }
}
