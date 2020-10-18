using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_S : MonoBehaviour
{
    public static bool isPlayerShopping;

    public GameObject shop;

    public GameObject itemIcon;

    public GameObject explosion;

    [HideInInspector]
    public GameObject itemObject;
    public GameObject itemPickup;
    public int cost = 25;

    public Transform[] shopLocs;

    
    private Inventory_S inventory;
    private Vector3 shopStartPos;
    private int numberOfItems;
    private List<SpriteRenderer> shopItems;

    public bool shopOpen;

    private bool closed;

    // Start is called before the first frame update
    void Start()
    {
        inventory = MasterController_S.player_S.GetComponent<Inventory_S>();
        shopStartPos = gameObject.transform.position;

        numberOfItems = shopLocs.Length;
        shopItems = new List<SpriteRenderer>();

        foreach (Transform shopLoc in shopLocs)
        {
            shopItems.Add(Instantiate(itemIcon, shopLoc.position, Quaternion.identity, shopLoc).GetComponent<SpriteRenderer>());
        }

        foreach (SpriteRenderer shopItem in shopItems)
        {
            shopItem.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfItems > 0)
        {
            if (shopOpen && Input.GetKeyDown("space"))
            {
                if (MasterController_S.self.money >= cost)
                {
                    MasterController_S.canvasMessageText_S.SendCanvasMessage("Purchase Complete!!");
                    SoundManager_S.PlaySound("buy");
                    Sell();
                }
                else
                {
                    MasterController_S.canvasMessageText_S.SendCanvasMessage("Needs 25 Pins to Buy");
                }
            }
        }
        else
            if (!closed)
                Close();
    }

    public void TriggerCollect()
    {
        MasterController_S.canvasMessageText_S.SendCanvasMessage(cost + " Pins");
        Debug.Log("Approached Shop!");
    }

    private void Close()
    {
        closed = true;
        Destroy(transform.GetChild(3).gameObject);
        Destroy(transform.GetChild(2).gameObject);
        Destroy(transform.GetChild(1).gameObject);
        GetComponentInChildren<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    private void Sell()
    {
        MasterController_S.self.ChangeMoneyAmount(-cost);

        if (Random.value > 0.6f)
            Instantiate(itemPickup, transform.position + new Vector3(0.5f, 0.7f, 0), transform.rotation);
        else
            Instantiate(itemPickup, transform.position + new Vector3(-0.5f, 0.7f, 0), transform.rotation);
        shopItems[numberOfItems - 1].enabled = false;
        numberOfItems--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!closed && collision.transform.CompareTag("Player"))
        {
            TriggerCollect();
            shopOpen = true;
            isPlayerShopping = true;

            for (int i = 0; i < numberOfItems; i++)
            {
                shopItems[i].enabled = true;
            }
        }
    }
    
    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        if (!closed && collision.gameObject.CompareTag("Player"))
        {
            shopOpen = false;
            isPlayerShopping = false;

            for (int i = 0; i < numberOfItems; i++)
            {
                shopItems[i].enabled = false;
            }
        }
    }
}
