using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTile_S : MonoBehaviour
{
    private Player_S playerScript;

    private void Start()
    {
        playerScript = MasterController_S.player_S;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerScript.isCrouch)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }

            if (collision.gameObject.CompareTag("Player"))
        {
            if (playerScript.isCrouch)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerScript.isCrouch)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                GetComponent<BoxCollider2D>().enabled = true;
            }
            else
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
