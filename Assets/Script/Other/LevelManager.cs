using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Exposed

    public bool M_onPause;
    public bool m_canPause;
    public bool m_cinematic = true;
    public bool m_win;
    public float _pauseSpeed = 1f;

    public Camera _cameramain;
    public Camera _cinematicCamera;

    public PlayerController _playerController;

    public CinemachineCollider _clearShotCollider;

    public GameObject _healthBar;
    public GameObject _titleScreen;
    public GameObject _deathScreen;
    public GameObject _winScreen;
    public Animator _uiAnimator;

    public int m_sceneIndex;



    #endregion
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_cinematic)
        {
            m_canPause = false;
            _cameramain.enabled = false;
            _cinematicCamera.enabled = true;
            _clearShotCollider.enabled = false;

            _titleScreen.SetActive(true);
            _healthBar.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_cinematic = false;
                _uiAnimator.SetTrigger("Start");
                SceneManager.LoadSceneAsync(m_sceneIndex);
            }

        }
        else
        {
            if (!_playerController.m_isDead)
            {

                m_canPause = true;
                _cameramain.enabled = true;
                _cinematicCamera.enabled = false;
                _clearShotCollider.enabled = true;
                _healthBar.SetActive(true);
                _titleScreen.SetActive(false);
                _deathScreen.SetActive(false);
            }
            else
            {
                return;
            }
        }

        if (m_canPause)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseActivate();
            }

        }

        IsDead();

        //YouWin();
    }

    public void PauseActivate()
    {
        M_onPause = !M_onPause;

        if (M_onPause)
        {
            Time.timeScale = 0f; //Mathf.Lerp(1, 0, _pauseSpeed * Time.deltaTime);
        }

        if (!M_onPause)
        {
            Time.timeScale = 1f;
        }
    }

    public void IsDead()
    {
        if (_playerController.m_isDead)
        {
            _healthBar.SetActive(false);
            _deathScreen.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {

                _playerController.m_isDead = false;

            }
            else if (Input.GetButtonDown("Jump"))
            {
                Application.Quit();
            }
        }
    }

    public void YouWin()
    {
        PauseActivate();
        _winScreen.SetActive(true);
        _healthBar.SetActive(false);
        m_win = true;

        if (m_win)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Application.Quit();
            }
        }

    }


    #region privates

    private Camera _mainCamera;


    #endregion
}
