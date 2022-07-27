using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour
{
    #region Exposed
    public float _ankleHeight = 0.15f;
    public float _ankleOffset = 0.25f;
    public float _rayCastFloorLength = 0.4f;
    public float m_ikWeightLeft;
    public float m_ikWeightRight;
    #endregion

    private void Start()
    {
        _eveAnimator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_eveAnimator)
        {
            //LeftFoot IK
            Vector3 _LeftFootDetector = _eveAnimator.GetIKPosition(AvatarIKGoal.LeftFoot);
            _LeftFootDetector.y += _ankleOffset;
            Ray _ray = new Ray(_LeftFootDetector, Vector3.down);
            Debug.DrawRay(_ray.origin, _ray.direction * _rayCastFloorLength, Color.red) ;

            if (Physics.Raycast(_ray, out RaycastHit _hitInfo, _rayCastFloorLength + _ankleOffset))
            {
                Vector3 _leftFootPosition = new Vector3(_hitInfo.point.x, _hitInfo.point.y + _ankleHeight, _hitInfo.point.z);

                _eveAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, _leftFootPosition);

                Quaternion _rot = Quaternion.LookRotation(transform.forward);

                _eveAnimator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.FromToRotation(Vector3.up, _hitInfo.normal) * _rot);

                float _rayUnderFoot = _rayCastFloorLength - _ankleHeight - _ankleOffset;
                float _rayInFloor = _hitInfo.distance - _ankleHeight - _ankleOffset;
                float _IKWeight = 1 - (_rayInFloor / _rayUnderFoot);
                m_ikWeightLeft = _IKWeight;

                _eveAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _IKWeight);
                _eveAnimator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _IKWeight);
            }

            //RightFoot IK

            Vector3 _rightFootDetector = _eveAnimator.GetIKPosition(AvatarIKGoal.RightFoot);
            _rightFootDetector.y += _ankleOffset;
            Ray _rayRight = new Ray(_rightFootDetector, Vector3.down);
            Debug.DrawRay(_rayRight.origin, _rayRight.direction * _rayCastFloorLength, Color.red);

            if (Physics.Raycast(_rayRight, out RaycastHit _hitInfoRight, _rayCastFloorLength + _ankleOffset))
            {
                Vector3 _rightFootPosition = new Vector3(_hitInfoRight.point.x, _hitInfoRight.point.y + _ankleHeight, _hitInfoRight.point.z);

                _eveAnimator.SetIKPosition(AvatarIKGoal.RightFoot, _rightFootPosition);

                Quaternion _rot = Quaternion.LookRotation(transform.forward);

                _eveAnimator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.FromToRotation(Vector3.up, _hitInfoRight.normal) * _rot);

                float _rayUnderFoot = _rayCastFloorLength - _ankleHeight - _ankleOffset;
                float _rayInFloor = _hitInfoRight.distance - _ankleHeight - _ankleOffset;
                float _IKWeight = 1 - (_rayInFloor / _rayUnderFoot);
                m_ikWeightRight = _IKWeight;

                _eveAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, _IKWeight);
                _eveAnimator.SetIKRotationWeight(AvatarIKGoal.RightFoot, _IKWeight);
            }

            //_eveAnimator.SetIKPosition(AvatarIKGoal.RightFoot, Vector3.up);
            //_eveAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0.5f);

            //_eveAnimator.SetIKPosition(AvatarIKGoal.LeftHand, Vector3.up);
            //_eveAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);

            //_eveAnimator.SetIKPosition(AvatarIKGoal.RightHand, Vector3.up);
            //_eveAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.5f);

            //_eveAnimator.SetLookAtPosition(_lookAtTarget.position);
            //_eveAnimator.SetLookAtWeight(1f);
        }
    }

    #region Privates

    private Animator _eveAnimator;

    #endregion
}
