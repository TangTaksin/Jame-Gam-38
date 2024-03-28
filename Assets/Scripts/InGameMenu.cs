using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    LevelManager lm;

    enum panel {none, paused, goal, fail }
    panel currentPanel;

    [SerializeField] GameObject GoalPanel;
    [SerializeField] GameObject PausePanel;
    Animator pauseAnim;

    [SerializeField] GameObject FailPanel;

    [SerializeField] KeyCode pauseKey;

    private void Start()
    {
        if (lm == null)
        {
            lm = FindAnyObjectByType<LevelManager>();
        }

        Player.OnDie += CallFail;
        pauseAnim = PausePanel.GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        Player.OnDie -= CallFail;
    }


    public void CallPause()
    {
        PausePanel.SetActive(true);
        LevelManager.CallPause();
        currentPanel = panel.paused;
    }

    public void UnPause()
    {
        pauseAnim.Play("LevelClear_Out");
        LevelManager.CallUnPause();
        currentPanel = panel.none;
    }

    public void CallFail()
    {
        FailPanel.SetActive(true);
        LevelManager.CallPause();
        currentPanel = panel.fail;
    }

    public void CallGoal()
    {
        GoalPanel.SetActive(true);
        LevelManager.CallPause();
        currentPanel = panel.goal;
    }


    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (currentPanel == panel.none)
                CallPause();
            else if (currentPanel == panel.paused)
                UnPause();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentPanel == panel.fail || currentPanel == panel.paused || currentPanel == panel.goal)
            {
                LevelManager.ReplayLevel();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentPanel == panel.goal)
            {
                lm.LoadNextLevel();
            }
        }
    }
}
