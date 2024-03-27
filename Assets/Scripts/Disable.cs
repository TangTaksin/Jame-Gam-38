using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    public void DisableThis()
    {
        gameObject.SetActive(false);
    }
}
