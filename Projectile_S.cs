using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_S : MonoBehaviour
{
    public float speed = 2f;
    public Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
        rb.angularVelocity = 1000;
        Destroy(gameObject, 4);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy_Wolf_S enemy = collision.gameObject.GetComponent<Enemy_Wolf_S>();
            if (enemy != null)
            {
                enemy.ChangeHealth(-1);
            }
            else
            {
                collision.gameObject.GetComponent<Enemy_Snowman_S>();
            }
            Destroy(gameObject);
        }
        */


        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
        
    }


    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy_Snowman_S enemy = collision.gameObject.GetComponent<Enemy_Snowman_S>();
            if (enemy != null)
            {
                enemy.ChangeHealth(-1);
                Destroy(gameObject);
            }
        }
        */
    }
}
