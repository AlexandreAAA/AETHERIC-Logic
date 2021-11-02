using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTest : MonoBehaviour
{


    public Animator _playerAnimator;
    void Start()
    {
        _playerAnimator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            _playerAnimator.SetBool("Cast", true);

            if (Time.time - _lastClickedTime > _maxComboDelay)
            {
                noOfClicks = 0;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                _lastClickedTime = Time.time;
                noOfClicks++;

                if (noOfClicks == 1)
                {
                    _playerAnimator.SetBool("Attack1", true);
                }

                noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
            }
        }
        else
        {
            _playerAnimator.SetBool("Cast", false);
        }
    }

    public void Return1()
    {
        if (noOfClicks >= 2)
        {
            _playerAnimator.SetBool("Attack2", true);
        }
        else
        {
            _playerAnimator.SetBool("Attack1", false);
            noOfClicks = 0;
        }
    }

    public void Return2()
    {
        if (noOfClicks >= 3)
        {
            _playerAnimator.SetBool("Attack3", true);
        }
        else
        {
            _playerAnimator.SetBool("Attack2", false);
            noOfClicks = 0;
        }
    }

    public void Return3()
    {

        _playerAnimator.SetBool("Attack1", false);
        _playerAnimator.SetBool("Attack2", false);
        _playerAnimator.SetBool("Attack3", false);
        noOfClicks = 0;

    }

    public int noOfClicks = 0;
    private float _lastClickedTime = 0;
    public float _maxComboDelay = 0.9f;
}
