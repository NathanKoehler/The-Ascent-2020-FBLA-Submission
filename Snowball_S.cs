using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball_S : MonoBehaviour
{
    public GameObject explosion;
    public GameObject explosionSmall;
    public float speed = 2f;
    public Rigidbody2D rb;
    
    [Space]
    [SerializeField]
    private GameObject particles;
    private GameObject player;

    void Start()
    {
        player = MasterController_S.player_S.gameObject;
        rb.velocity = transform.right * speed;
        if (particles == null)
        {
            particles = transform.GetChild(0).gameObject;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            MasterController_S.self.ChangeHealth(-1, true, new Vector2((player.transform.position.x - transform.position.x) * 5f, 1f));
            SoundManager_S.PlaySound("hit");
            transform.DetachChildren();
            particles.AddComponent<DoomedToDieParticles_S>();
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            transform.DetachChildren();
            particles.AddComponent<DoomedToDieParticles_S>();
            Instantiate(explosionSmall, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

    }
}
