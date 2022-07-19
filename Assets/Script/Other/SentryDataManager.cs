using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryDataManager : MonoBehaviour
{
    #region Exposed

    public EnnemyData _sentryData;
    public MeleeAttackState _meleeAttackState;

    #endregion


    #region Unity API

    private void Start()
    {
        _meleeAttackState = GetComponent<MeleeAttackState>();
    }

    private void Update()
    {
        if (_meleeAttackState.m_isAttacking)
        {
            _sentryData._currentAttackPower = _sentryData.m_attackPowers[0];
        }

        if (_meleeAttackState.m_strongAttack)
        {
            _sentryData._currentAttackPower = _sentryData.m_attackPowers[1];
        }

        if(_meleeAttackState.m_Charge)
        {
            _sentryData._currentAttackPower = _sentryData.m_attackPowers[2];
        }
    }

    #endregion
}
