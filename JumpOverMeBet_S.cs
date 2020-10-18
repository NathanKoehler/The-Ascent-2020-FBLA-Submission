using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOverMeBet_S : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && transform.parent.GetComponent<Enemy_Wolf_S>().Aggro)
        {
            //transform.parent.GetComponent<Enemy_Wolf_S>().HeadUp();
        }
    }
}
