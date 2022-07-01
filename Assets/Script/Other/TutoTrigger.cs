using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutoTrigger : MonoBehaviour
{
    
    public TMP_Text _tutoText;
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            
            _tutoText.enabled = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _tutoText.enabled = false;
    }

    
}
