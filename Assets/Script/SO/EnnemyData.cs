using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnnemyData : ScriptableObject
{
    public int m_totalSentryHp;

    public int[] m_attackPowers;

    public int _currentAttackPower;
}
