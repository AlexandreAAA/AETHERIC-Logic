using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private float _maxDistance;

    public Image _crosshair;

    public bool _haveKey = false;

    public Transform _detection;

    public LayerMask _IusableLayer;

    public bool _canInteract;

    public Text _interactText;

    #endregion
    void Start()
    {
        _cameraTransform = Camera.main.transform;

    }

    #region Unity API

    void Update()
    {
        Interact();
        //FindTarget();

        UseTarget();

        //ChangeCrossHairState();
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.gameObject.ToString());
        if (other.CompareTag("Key"))
        {
            _haveKey = true;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Interaction") && other.GetComponent<IUsable>() != null)
        {

            _canInteract = true;
            _target = other.GetComponent<IUsable>();
            //Debug.Log(_target);
        }
        else
        {
            //_target = null;
            //_canInteract = false;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interaction"))
        {

        }
        _canInteract = false;
    }

    #endregion


    #region Main Method

    private void FindTarget()
    {
        Ray _ray = new Ray(_detection.position, _detection.forward);

        Debug.DrawRay(_ray.origin, _ray.direction * _maxDistance, Color.green);

        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit, _maxDistance))
        {
            if (_hit.collider.GetComponent<IUsable>() != null)
            {
                _target = _hit.collider.GetComponent<IUsable>();
            }
        }
        else
        {
            _target = null;
        }


    }

    private void UseTarget()
    {
        if (_canInteract)
        {

            if (Input.GetButtonDown("Use"))
            {
                if (_target != null)
                {
                    _target.Use();
                }
            }
        }
    }

    private void ChangeCrossHairState()
    {
        if (_target != null)
        {
            _crosshair.color = Color.green;
        }
        else
        {
            _crosshair.color = Color.red;
        }
    }

    private void Interact()
    {
        if (_canInteract)
        {
            _interactText.enabled = true;
        }
        else
        {
            _interactText.enabled = false;
        }
    }
    #endregion


    #region Privates

    private IUsable _target;

    private Transform _cameraTransform;

    #endregion
}
