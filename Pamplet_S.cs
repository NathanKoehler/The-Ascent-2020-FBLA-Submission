using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pamplet_S : Pickup_S
{
    [SerializeField]
    private int ammoNum = 5;
    private Collider2D rigidCollider;


    private void Start()
    {
        rigidCollider = GetComponent<Collider2D>();

        StartCoroutine(PickupDelay()) ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Floor"))
        {
            Physics2D.IgnoreCollision(rigidCollider, collision);
        }
        if (collision.CompareTag("Player") && seekingInput)
        {
            seekingInput = false;

            AddToInv();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Floor"))
        {
            Physics2D.IgnoreCollision(rigidCollider, collision.otherCollider);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && seekingInput)
        {
            seekingInput = false;

            AddToInv();
        }
    }

    public override void AddToInv()
    {
        if (MasterController_S.self.ammo >= 15)
        {
            seekingInput = true;
            MasterController_S.canvasMessageText_S.SendCanvasMessage("Already Full on Pamplets!!");
        }
        else if (Ammo_S.exists)
        {
            if (MasterController_S.self.ammo < 10)
            {
                MasterController_S.canvasMessageText_S.SendCanvasMessage("Added 5 Pamplets to Existing Pamplets");
                MasterController_S.self.ChangeAmmo(MasterController_S.self.ammo + 5);
            }
            else
            {
                int num = 15 - MasterController_S.self.ammo;
                MasterController_S.self.ChangeAmmo(15);
                MasterController_S.canvasMessageText_S.SendCanvasMessage("Added " + num + " Pamplets to Existing Pamplets");
            }
            Destroy(gameObject);
        }
        else
        {
            base.AddToInv();
            MasterController_S.canvasMessageText_S.SendCanvasMessage("Added New Pamplets to Inventory");
            MasterController_S.player_S.SetCanAttack(true, ammoNum);
            Destroy(gameObject);
        }
    }

    IEnumerator PickupDelay()
    {
        yield return new WaitForSeconds(1.5f);
        seekingInput = true;
    }
}
