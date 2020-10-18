using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Snowman_S : MonoBehaviour
{
    public GameObject snowballObj;
    [Space]
    public int health;

    [SerializeField]
    private bool shoot;
    [SerializeField]
    private bool shootRight;
    [SerializeField]
    private Vector3 shootingOffset;

    private IEnumerator shooting;
    private SpriteRenderer rend;
    private bool delay;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        if (shootRight)
            rend.flipX = true;
    }

    private void Update()
    {
        if (rend.isVisible && !delay && shoot && shooting == null)
        {
            shoot = false;
            delay = true;
            shooting = Shoot();
            StartCoroutine(shooting);
        }
    }

    public void Die()
    {
        SoundManager_S.PlaySound("snowman");
        GetComponent<Animator>().SetTrigger("isDead");
        Destroy(transform.parent.gameObject, 0.8f);
    }

    public void ChangeHealth(int change)
    {
        health += change;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    void ShootRight()
    {
        Instantiate(snowballObj, new Vector2(transform.position.x + shootingOffset.x, transform.position.y + shootingOffset.y), Quaternion.Euler(0, 0, 0));
    }

    void ShootLeft()
    {
        Instantiate(snowballObj, new Vector2(transform.position.x - shootingOffset.x, transform.position.y + shootingOffset.y), Quaternion.Euler(0, 0, 180));
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Die();
        }
        else if (collider.gameObject.CompareTag("Shootable"))
        {
            ChangeHealth(-1);
            Destroy(collider.gameObject);
        }
    }
    IEnumerator Shoot()
    {
        if (shootRight)
            ShootRight();
        else
            ShootLeft();
        yield return new WaitForSeconds(0.2f);
        delay = false;
        shooting = null;
    }
}
