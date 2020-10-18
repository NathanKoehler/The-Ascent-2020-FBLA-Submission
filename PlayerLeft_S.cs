using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeft_S : MonoBehaviour
{
    private Player_S player_S;
    private bool playerKillable = true;

    // Start is called before the first frame update
    void Start()
    {
        player_S = GetComponentInParent<Player_S>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            player_S.SetWallSlide(-1, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            player_S.SetWallSlide(-1, false);
        }
    }
}
