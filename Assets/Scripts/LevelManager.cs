using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public delegate void PauseEvent();
    public static PauseEvent OnPause;
    public static PauseEvent OnUnPause;

    public string nextLevel = string.Empty;

    public static void CallPause()
    {
        OnPause?.Invoke();
    }

    public static void CallUnPause()
    {
        OnUnPause?.Invoke();
    }

    public void LoadNextLevel()
    {
        AudioManager.Instance.StopAllSFX();
        SceneManager.LoadScene(nextLevel);
    }

    public static void ReplayLevel()
    {
        AudioManager.Instance.StopAllSFX();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
