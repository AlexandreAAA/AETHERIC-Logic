using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchAnimTest : MonoBehaviour
{
    public PlayerCasting _playerCasting;

    void Start()
    {
        _playerCasting = GetComponentInParent<PlayerCasting>();
    }

    
    void Update()
    {
        
    }

    public void LaunchFireBall()
    {
        _playerCasting.FireBall();
    }
    
}
