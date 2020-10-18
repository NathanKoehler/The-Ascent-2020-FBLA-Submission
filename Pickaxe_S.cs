using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe_S : Pickup_S
{
    private void Start()
    {
        StartCoroutine(PickupDelay());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && seekingInput)
        {
            seekingInput = false;

            AddToInv();
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
        if (!MasterController_S.self.hasClimb)
        {
            base.AddToInv();

            MasterController_S.canvasMessageText_S.SendCanvasMessage("Added Climbing Gear to Inventory");

            MasterController_S.player_S.SetCanClimb(true);
        }
        else
        {
            MasterController_S.canvasMessageText_S.SendCanvasMessage("Already Has Climbing Equipment!!");
            seekingInput = true;
        }
    }

    IEnumerator PickupDelay()
    {
        yield return new WaitForSeconds(1.5f);
        seekingInput = true;
    }
}
