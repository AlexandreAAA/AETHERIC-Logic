using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EveController;

public class PlayerAudioManager : MonoBehaviour
{
    #region Exposed

    [Header("FootStep")]
    [SerializeField]
    private AudioClip[] _leftFootSteps;
    [SerializeField]
    private AudioClip[] _rightFootSteps;
    [Range(0, 1)]
    [SerializeField]
    private float _footStepVolume;

    [Header("Attacks")]
    [SerializeField]
    private AudioClip[] _attackSounds;

    [Range(0, 1)]
    [SerializeField]
    private float _attackVolume;

    [SerializeField]
    private AudioClip[] _hitSounds;
    [SerializeField]
    private AudioClip[] _deathSounds;

    [Header("Voice")]
    [SerializeField]
    private AudioClip[] _voiceFx;

    #endregion

    #region Unity API

    private void Awake()
    {
        _eveAudioSource = GetComponent<AudioSource>();
    }

    #endregion

    private void Start()
    {
        _playerController = GetComponentInParent<StateController>();
    }

    #region Main Method

    public void OnLeftFootSound()
    {
        if (!_playerController.isCasting && _playerController.isrunning)
        {
            _eveAudioSource.PlayOneShot(_leftFootSteps[Random.Range(0, _leftFootSteps.Length)], _footStepVolume);
        }
    }

    public void OnRightFoot()
    {
        if (!_playerController.isCasting && _playerController.isrunning)
        {
            _eveAudioSource.PlayOneShot(_rightFootSteps[Random.Range(0, _rightFootSteps.Length)], _footStepVolume);
        }
    }

    public void LeftCast()
    {
        if (_playerController.isCasting)
        {

            _eveAudioSource.PlayOneShot(_leftFootSteps[Random.Range(0, _leftFootSteps.Length)], _footStepVolume);
        }
    }

    public void RightCast()
    {
        if (_playerController.isCasting)
        {

            _eveAudioSource.PlayOneShot(_rightFootSteps[Random.Range(0, _rightFootSteps.Length)], _footStepVolume);
        }
    }

    public void AttackSound(int _audioClip)
    {
        _eveAudioSource.PlayOneShot(_attackSounds[_audioClip], _attackVolume);
    }

    public void HitSounds()
    {
        _eveAudioSource.PlayOneShot(_hitSounds[Random.Range(0, _hitSounds.Length)], _attackVolume);
    }

    public void DeathSound()
    {
        _eveAudioSource.PlayOneShot(_deathSounds[0], _attackVolume);
    }

    public void EveVoice()
    {
        _eveAudioSource.PlayOneShot(_voiceFx[0], 0.5f);
    }

    #endregion

    #region Privates
    public AudioSource _eveAudioSource;
    private StateController _playerController;
    #endregion
}
