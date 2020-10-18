using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup_S : Pickup_S
{
    [HideInInspector]
    public HealthItem_S healthItem;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Collider2D rigidCollider;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        rigidCollider = GetComponent<Collider2D>();

        if (!UIHealth_S.roleCall[0])
            StartCoroutine(Wait());
        else
            Setup();

        StartCoroutine(PickupDelay());
    }

    public void Setup()
    {
        List<int> availableSlots = new List<int>();
        for (int i = 0; i < UIHealth_S.roleCall.Length; i++)
        {
            if (!UIHealth_S.roleCall[i])
            {
                availableSlots.Add(i);
            }
        }
        if (availableSlots.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
        int rand = availableSlots[Random.Range(0, availableSlots.Count)];
        healthItem = Canvas_S._UIHealth_S.refHealthItems[rand];
        spriteRenderer.sprite = healthItem.sprite;
    }

    public override void AddToInv()
    {
        if (!UIHealth_S.roleCall[healthItem.order])
        {
            MasterController_S.self.ChangeSpecificHealth(true, false, healthItem.order);
            Destroy(gameObject);
        }
        else
        {
            seekingInput = true;
            MasterController_S.canvasMessageText_S.SendCanvasMessage("Already Wearing That...");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Floor"))
        {
            Physics2D.IgnoreCollision(rigidCollider, collision);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Floor"))
        {
            Physics2D.IgnoreCollision(rigidCollider, collision.otherCollider);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForFixedUpdate();
        Setup();
    }

    IEnumerator PickupDelay()
    {
        yield return new WaitForSeconds(1.5f);
        seekingInput = true;
    }
}
