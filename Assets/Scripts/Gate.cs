using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    bool goalReached = false;

    public delegate void GoalEvent();
    public GoalEvent goalEvent;

    public bool GetGoal()
    {
        return goalReached;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        goalReached = true;
        goalEvent?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        goalReached = false;
        goalEvent?.Invoke();
    }
}
