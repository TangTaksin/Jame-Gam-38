using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("DeadZone"))
        {
            collision.GetComponent<Player>().FallDie();
            
        }
        else
        {
            collision.GetComponent<Player>().Die();
        }
        
    }
}
