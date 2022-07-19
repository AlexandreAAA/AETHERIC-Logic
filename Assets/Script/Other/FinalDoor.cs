using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{

    #region Exposed

    public LevelManager m_levelManager;

    #endregion
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            m_levelManager.YouWin();

        }
    }
}
