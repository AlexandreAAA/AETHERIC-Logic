using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraBehaviour : MonoBehaviour
{

    [SerializeField]
    private Transform[] _lookPos;

    public float _startScanTime = 10f;

    public bool _isSensorCam;

    public AudioClip[] _camClips;


    void Start()
    {
        _camNav = GetComponent<NavMeshAgent>();
        _camDetect = GetComponentInChildren<CamDetect>();
        _scanTime = _startScanTime;
        _camAudio = GetComponent<AudioSource>();
    }


    void Update()
    {
        Scan();

        Alert();
    }


    private void Scan()
    {
        if (_scanning)
        {
            _camNav.velocity = Vector3.zero;

            _scanTime -= Time.deltaTime;

            if (_scanTime > _startScanTime / 2)
            {
                _camNav.SetDestination(_lookPos[0].position);

            }
            else if (_scanTime < _startScanTime / 2)
            {
                _camNav.SetDestination(_lookPos[1].position);
            }

            if (_scanTime <= 0f)
            {
                _scanTime = _startScanTime;
            }

        }
    }

    private void Alert()
    {
        if (_isSensorCam)
        {
            if (_camDetect._alerting)
            {
                _camNav.ResetPath();
                _scanning = false;
            }

            if (_camDetect._scanPlayer)
            {
                _camAudio.enabled = true;
                _camNav.velocity = Vector3.zero;
                _scanning = false;
                _camNav.SetDestination(_camDetect._lastKnownPos);
            }
            else
            {
                _scanning = true;
                _camAudio.enabled = false;

            }
        }
        else if (!_isSensorCam)
        {
            if (_camDetectCone._alerting)
            {
                _camNav.ResetPath();
                _scanning = false;
            }

            if (_camDetectCone._scanPlayer)
            {
                _camAudio.enabled = true;
                _camNav.velocity = Vector3.zero;
                _scanning = false;
                _camNav.SetDestination(_camDetectCone._lastKnownPos);
            }
            else
            {
                _scanning = true;
                _camAudio.enabled = false;

            }
        }
    }


    private NavMeshAgent _camNav;

    [SerializeField]
    private bool _scanning = true;

    [SerializeField]
    private float _scanTime;

    private CamDetect _camDetect;
    public CamDetectCone _camDetectCone;
    private AudioSource _camAudio;

}
