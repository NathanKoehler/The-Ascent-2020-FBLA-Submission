using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_S : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("Collectable Can't find Sprite Renderer");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TriggerCollect(Player_S player_S)
    {
        Debug.Log("Collected!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (spriteRenderer.isVisible && collision.gameObject.CompareTag("Player"))
        {
            TriggerCollect(collision.gameObject.GetComponent<Player_S>());
        }
    }
}
