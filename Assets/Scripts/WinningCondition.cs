using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningCondition : MonoBehaviour
{
    [SerializeField] Gate[] goalList;
    [SerializeField] InGameMenu inGameMenu;

    void CheckGoal()
    {
        var win = false;

        foreach (Gate goal in goalList)
        {
            win = goal.GetGoal();

            if (!win)
                break;
        }

        if (win)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.truckEngineSFX);
            // Call Level Clear
            inGameMenu.CallGoal();
        }
    }

    private void Start()
    {
        foreach (Gate goal in goalList)
            goal.goalEvent += CheckGoal;
    }

    private void OnDestroy()
    {
        foreach (Gate goal in goalList)
            goal.goalEvent -= CheckGoal;
    }
}
