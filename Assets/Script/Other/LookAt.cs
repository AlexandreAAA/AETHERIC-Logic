using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

    #region Exposed

    public Transform m_head = null;
    public Vector3 lookAtTargetPosition;
    public float m_lookAtCoolTime = 0.2f;
    public float m_lookAtHeatTime = 0.2f;
    public bool m_looking = true;

    #endregion


    #region Unity API
    void Start()
    {
        if (!m_head)
        {
            Debug.Log("Nohead Transfrom - LookAT disabled");
            enabled = false;
            return;
        }

        _soldierAnimlator = GetComponent<Animator>();
        lookAtTargetPosition = m_head.position + transform.forward;
        lookAtPosition = lookAtTargetPosition;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        lookAtTargetPosition.y = m_head.position.y;
        float _lookAtTargetWeight = m_looking ? 1.0f : 0.0f;

        Vector3 _curDir = lookAtPosition - m_head.position;
        Vector3 _futDir = lookAtTargetPosition - m_head.position;

        _curDir = Vector3.RotateTowards(_curDir, _futDir, 6.28f * Time.deltaTime, float.PositiveInfinity);
        lookAtPosition = m_head.position + _curDir;

        float _blendTime = _lookAtTargetWeight > _lookAtweight ? m_lookAtHeatTime : m_lookAtCoolTime;
        _lookAtweight = Mathf.MoveTowards(_lookAtweight, _lookAtTargetWeight, Time.deltaTime / _blendTime);

        _soldierAnimlator.SetLookAtWeight(_lookAtweight, 02f, 0.5f, 0.7f, 0.5f);
        _soldierAnimlator.SetLookAtPosition(lookAtPosition);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion


    #region Privates

    private Vector3 lookAtPosition;
    private Animator _soldierAnimlator;
    private float _lookAtweight = 0.0f;

    #endregion
}
