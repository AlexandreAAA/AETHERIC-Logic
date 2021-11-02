using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    #region Exposed

    public CharacterData _playerData;
    public EnnemyData _ennemyData;
    public MeleeAttackState _meleeAttackState;
    private Collider _weaponCollider;

    #endregion


    #region Unity API

    private void Start()
    {
        _weaponCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //OnPlayerHit();
            //_weaponCollider.enabled = false;

        }
    }

    #endregion


    #region Main Method

    public void OnPlayerHit()
    {
        if (_meleeAttackState.m_isAttacking)
        {
            _playerData.m_currentHp -= _ennemyData.m_attackPowers[0];
        }

        else if (_meleeAttackState.m_strongAttack)
        {
            _playerData.m_currentHp -= _ennemyData.m_attackPowers[1];
        }

        else if (_meleeAttackState.m_Charge)
        {
            _playerData.m_currentHp -= _ennemyData.m_attackPowers[2];
        }
    }

    #endregion
}
