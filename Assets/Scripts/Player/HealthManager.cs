using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    //public Image[] lives;
    public int healthRemaining;
    public int livesRemaining;
    public Text livesText;
    public Text livesText2;

    void Update()
    {
        livesText.text = livesRemaining.ToString();
        livesText2.text = livesRemaining.ToString();
    }

    public void LoseHealth()
    {
        healthRemaining--;

        if(healthRemaining == 0)
        {
            FindObjectOfType<PlayerMovement1>().Dead();
            LoseLife();
        }
    }

    public void LoseLife()
    {
        livesRemaining--;

        if(livesRemaining == 0)
        {
            FindObjectOfType<LevelManager>().GameOverScreen();
        }
        else
        {
            FindObjectOfType<LevelManager>().Respawn();
        }
    }

    
}
