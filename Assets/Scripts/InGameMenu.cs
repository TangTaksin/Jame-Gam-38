using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    enum panel {none, paused, goal, fail }
    panel currentPanel;

    [SerializeField] GameObject GoalPanel;
    [SerializeField] GameObject PausePanel;
    Animator pauseAnim;

    [SerializeField] GameObject FailPanel;

    [SerializeField] KeyCode pauseKey;

    private void Start()
    {
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
        LevelManager.CallPause();
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
    }
}
