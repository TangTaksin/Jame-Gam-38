using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    [SerializeField] float smokeAddRate = .1f;

    List<Player> playerList = new List<Player>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        player.EnterSmoke(true);

        if (!playerList.Contains(player))
            playerList.Add(player);
    }

    private void Update()
    {
        if (playerList.Count > 0)
        {
            foreach (var player in playerList)
            {
                player.AddSmoke(smokeAddRate);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        player.EnterSmoke(false);

        if (playerList.Contains(player))
            playerList.Remove(player);
    }
}
