using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkTest : MonoBehaviour
{
    #region Exposed

    [Range(0,1)]
    public float m_distanceToGround;       

    public LayerMask _groundLayer;

    [Range(0,1)]
    public float m_posWeight;

    [Range(0, 1)]
    public float m_RotaWeight;

    #endregion

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_anim)
        {
            _anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, m_posWeight);
            _anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, m_RotaWeight);
            _anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, m_posWeight);
            _anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, m_RotaWeight);

            RaycastHit _hit;
            Ray _ray = new Ray(_anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            Debug.DrawRay(_ray.origin, _ray.direction * 2, Color.green);


            //Left Foot

            if (Physics.Raycast( _ray, out _hit, m_distanceToGround +1f, _groundLayer))
            {
                if (_hit.collider != null)
                {
                    Vector3 _footPosition = _hit.point;
                    _footPosition.y += m_distanceToGround;
                    _anim.SetIKPosition(AvatarIKGoal.LeftFoot, _footPosition);

                    Quaternion rot = Quaternion.LookRotation(transform.forward);

                    Quaternion _targetRotation = Quaternion.FromToRotation(Vector3.up, _hit.normal) * rot;

                    _anim.SetIKRotation(AvatarIKGoal.LeftFoot, _targetRotation);
                }
            }

            //Right Foot

            
            _ray = new Ray(_anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            Debug.DrawRay(_ray.origin, _ray.direction * 2, Color.green);

            if (Physics.Raycast(_ray, out _hit, m_distanceToGround + 1f, _groundLayer))
            {
                if (_hit.collider != null)
                {
                    Vector3 _footPosition = _hit.point;
                    _footPosition.y += m_distanceToGround;
                    _anim.SetIKPosition(AvatarIKGoal.RightFoot, _footPosition);

                    Quaternion rot = Quaternion.LookRotation(transform.forward);

                    Quaternion _targetRotation = Quaternion.FromToRotation(Vector3.up, _hit.normal) * rot;

                    _anim.SetIKRotation(AvatarIKGoal.RightFoot, _targetRotation);
                }
            }
        }
    }

    #region Privates

    private Animator _anim;

    #endregion
}
