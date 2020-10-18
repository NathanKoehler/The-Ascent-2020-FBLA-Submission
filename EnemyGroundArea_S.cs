using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundArea_S : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Elusive"))
        {
            transform.parent.GetComponent<Enemy_Wolf_S>().BelowYou();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Elusive"))
        {
            transform.parent.GetComponent<Enemy_Wolf_S>().OnGround();
        }
    }
}
