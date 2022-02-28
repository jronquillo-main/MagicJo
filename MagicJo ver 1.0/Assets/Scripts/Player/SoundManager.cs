using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip playerJump, playerScore, playerDeath, BGM;
    static AudioSource audioSrc;

    void Start()
    {
        playerScore = Resources.Load<AudioClip>("playerScore");
        playerJump = Resources.Load<AudioClip>("playerJump");
        playerDeath = Resources.Load<AudioClip>("playerDeath");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        switch(clip)
        {
            case "playerScore": audioSrc.PlayOneShot(playerScore);
            break;

            case "playerDeath": audioSrc.PlayOneShot(playerDeath);
            break;

            case "playerJump": audioSrc.PlayOneShot(playerJump);
            break;
        }
    }
}
