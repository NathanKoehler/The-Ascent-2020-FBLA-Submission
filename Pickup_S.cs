using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup_S : MonoBehaviour
{
    private Inventory_S inventory;

    
    public GameObject itemButton;
    [Header("On Pickup Event")]
    public UnityEvent Event;
    [HideInInspector]
    protected bool seekingInput = false;
    // Start is called before the first frame update
    void Start()
    {
        seekingInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public virtual void AddToInv()
    {
        inventory = MasterController_S.player_S.GetComponent<Inventory_S>();

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.isFull[i] == false)
            {
                inventory.isFull[i] = true;
                Instantiate(itemButton, inventory.slots[i].transform, false);
                Event.Invoke();
                Destroy(gameObject);
                MasterController_S.StoreInventoryGameObject(itemButton, i);
                break;
            }
        }
    }
}
