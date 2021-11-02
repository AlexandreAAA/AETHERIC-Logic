using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnJTalker : MonoBehaviour, IUsable
{

    #region Exposed

    [SerializeField]
    private AudioClip[] _audioClips;

    #endregion

    void Start()
    {
        _pnjAudiosource = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        _pnjAudiosource.PlayOneShot(_audioClips[Random.Range(0, _audioClips.Length)]);
    }

    private AudioSource _pnjAudiosource;
}
