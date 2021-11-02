using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serrure : MonoBehaviour, IUsable
{
    public Animator _doorAnimator;
    public PlayerInteraction _playerInteraction;
    public CharacterData _playerData;
    public PlayerAudioManager _playerAudio;

    public bool _needBlue;
    public bool _needGreen;
    public bool _accessGranted = false;
    public bool _needYellow;

    [SerializeField]
    private AudioClip[] _audioclips = new AudioClip[2];

    void Start()
    {
        _doorAnimator = GetComponentInParent<Animator>();
        _playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        _doorAudioSource = GetComponent<AudioSource>();
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Use()
    {
        //_playerHasKey = _playerInteraction._haveKey;

        if (_playerData._blueKey && _needBlue)
        {
            _doorAnimator.SetBool("Open", true);
            _doorAudioSource.PlayOneShot(_audioclips[0]);
            _playerData._blueKey = false;
        }
        else if (_playerData._yellowKey && _needYellow)
        {
            _doorAnimator.SetBool("Open", true);
            _doorAudioSource.PlayOneShot(_audioclips[0]);
            _playerData._yellowKey = false;
        }
        else if (_playerData._greenKey && _needGreen)
        {
            _accessGranted = true;
           _doorAudioSource.PlayOneShot(_audioclips[0]);
            _playerData._greenKey = false;
        }
        else
        {
            _doorAudioSource.PlayOneShot(_audioclips[1]);
            
            
        }
    }

    private bool _playerHasKey;
    private AudioSource _doorAudioSource;

}
