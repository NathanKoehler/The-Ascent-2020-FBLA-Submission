using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_S : MonoBehaviour
{
    public static AudioClip playerHitSound, walk, throwing, hurt, pinCollect, snowman, npc, shopBought, buttonHover, bark, dialogueSwish, correct, incorrect; //add more sound variables here!
    static AudioSource audioSource;


    void Start()
    {
        playerHitSound = Resources.Load<AudioClip>("npc_hit"); //set more sounds to variables here!
        walk = Resources.Load<AudioClip>("walking");
        throwing = Resources.Load<AudioClip>("throw_snowball");
        hurt = Resources.Load<AudioClip>("hurt");
        pinCollect = Resources.Load<AudioClip>("reward");
        snowman = Resources.Load<AudioClip>("snowman_death");
        npc = Resources.Load<AudioClip>("shop");
        shopBought = Resources.Load<AudioClip>("shop_bought");
        buttonHover = Resources.Load<AudioClip>("button_hover");
        bark = Resources.Load<AudioClip>("bark");
        dialogueSwish = Resources.Load<AudioClip>("whoosh");
        correct = Resources.Load<AudioClip>("correct");
        incorrect = Resources.Load<AudioClip>("wrong");

        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip) //play more sounds here!
        {
            case "hit":
                audioSource.PlayOneShot(playerHitSound);
                break;
            case "walk":
                audioSource.PlayOneShot(walk);
                break;
            case "throw":
                audioSource.PlayOneShot(throwing);
                break;
            case "hurt":
                audioSource.PlayOneShot(hurt);
                break;
            case "pin":
                audioSource.PlayOneShot(pinCollect);
                break;
            case "snowman":
                audioSource.PlayOneShot(snowman);
                break;
            case "npc":
                audioSource.PlayOneShot(npc);
                break;
            case "buy":
                audioSource.PlayOneShot(shopBought);
                break;
            case "button":
                audioSource.PlayOneShot(buttonHover);
                break;
            case "bark":
                audioSource.PlayOneShot(bark);
                break;
            case "woosh":
                audioSource.PlayOneShot(dialogueSwish);
                break;
            case "correct":
                audioSource.PlayOneShot(correct);
                break;
            case "incorrect":
                audioSource.PlayOneShot(incorrect);
                break;
        }
    }
}
