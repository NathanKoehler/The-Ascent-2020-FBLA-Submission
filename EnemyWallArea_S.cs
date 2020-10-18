using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallArea_S : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            transform.parent.GetComponent<Enemy_Wolf_S>().HittingWall();
        }
    }
}
