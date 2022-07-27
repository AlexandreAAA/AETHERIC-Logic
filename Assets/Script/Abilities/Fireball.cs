using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EveController.Abilities
{
    public class Fireball : AbstractAbility
    {
        #region Exposed

        [SerializeField]
        private GameObject _bigExplosion;

        #endregion

        #region Unity API
        private void Awake()
        {
            _fireballRb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Vector3 velocity = _fireBallSpeed * Time.fixedDeltaTime * transform.forward;
            Vector3 _newPos = transform.position + velocity;
            _fireballRb.MovePosition(_newPos);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                GameObject explosion = PlayerObjectPool.SharedInstance.GetPooledObject(PlayerObjectPool.SharedInstance.explosion);
                explosion.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                explosion.SetActive(true);
                gameObject.SetActive(false);
            }
        }
        #endregion

        #region MainMethod

        public void SetFireballSpeed(float _speed)
        {
            _fireBallSpeed = _speed;
        }

        #endregion

        #region Privates

        private Rigidbody _fireballRb;
        private float _fireBallSpeed;

        #endregion
    }
}
