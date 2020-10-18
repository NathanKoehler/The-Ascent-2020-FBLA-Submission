using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items_S : MonoBehaviour
{
    public GameObject shop;
    public GameObject item = null;
    public float cost = 0;


    // Start is called before the first frame update
    void Start()
    {
        shop = GameObject.Find("Shop");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Buyable(bool itemBuyable)
    {
        if (itemBuyable)
        {
            moveItem();
        }
        else
        {
            DestroyImmediate(item);
        }
    }

    public void Bought(Player_S player_S)
        {   
            Debug.Log("Item Gained!");
            Destroy(gameObject);
        }

    public void moveItem()
        {
            item.transform.position = new Vector3(shop.transform.position.x, shop.transform.position.y + 0.75f, shop.transform.position.z);
        }
    }
