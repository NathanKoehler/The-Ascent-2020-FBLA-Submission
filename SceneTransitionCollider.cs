using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.GetComponent<SceneLoader_S>().LoadNextScreen();
        }
    }
}
