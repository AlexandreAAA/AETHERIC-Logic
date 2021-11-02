using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    public int m_totalHp;

    public int m_currentHp;

    public int m_potions;

    public int[] m_magicSelected;

    public int[] m_attackPower;

    public bool m_detected;

    public bool m_alert;

    public float m_timeToRaiseAlert;

    public float m_defaultAlertTime;

    public Vector3 m_lastKnownPosition;

    public bool _greenKey;

    public bool _blueKey;

    public bool _yellowKey;

    
}
