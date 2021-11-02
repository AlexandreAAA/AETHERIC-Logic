using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryAudio : MonoBehaviour
{
    #region Exposed
    
    //Footsteps
    [SerializeField]
    private AudioClip[] _footSteps;

    [Range(0, 1)]
    [SerializeField]
    private float _footStepsVolume;

    //Voice
    [SerializeField]
    private AudioClip[] _voiceClipsSuspicious;
    [Range(0, 1)]
    public float _voiceVolume;

    [SerializeField]
    private AudioClip[] _voiceAttack;
    [SerializeField]
    private AudioClip[] _voiceHits;

    [Range(0,1)]
    public float _hitVolume;

    [SerializeField]
    private AudioClip[] _weaponSounds;
    [Range(0, 1)]
    public float _weaponVolume;
    


    #endregion


    #region UnitY API

    private void Start()
    {
        _sfxSounds = GetComponent<AudioSource>();
        _voiceSfx = GetComponentInChildren<AudioSource>();
    }

    #endregion


    #region Main Method

    //FootsSteps
    public void SentryFootSteps(int _index)
    {
        _sfxSounds.PlayOneShot(_footSteps[_index], _footStepsVolume);
    }

    public void SentryWeaponSounds()
    {
        _sfxSounds.PlayOneShot(_weaponSounds[Random.Range(0, _weaponSounds.Length)], _weaponVolume);
    }

    //Voice
    public void SentrySuspiciousdex()
    {
        _voiceSfx.PlayOneShot(_voiceClipsSuspicious[Random.Range(0, _voiceClipsSuspicious.Length)], _voiceVolume);
    }

    public void SentryAttackSounds(int _index)
    {
        _voiceSfx.PlayOneShot(_voiceAttack[_index], _voiceVolume);
    }

    public void OnHit()
    {
        _sfxSounds.PlayOneShot(_voiceHits[Random.Range(0, _voiceHits.Length)], _hitVolume);
    }

    #endregion


    #region Privates

    private AudioSource _sfxSounds;
    private AudioSource _voiceSfx;
   


    #endregion
}
