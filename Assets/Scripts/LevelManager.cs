using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    
    //private bool roomComplete;
    [SerializeField]private Transform pos1;
    [SerializeField]private Transform pos2;
    [SerializeField]private Transform pos3;
    [SerializeField]private Transform pos4;
    [SerializeField]private Transform pos5;
    [SerializeField]private Transform pos6;
    [SerializeField]private Transform pos7;
    [SerializeField]private Transform pos8;
    [SerializeField]private Transform pos9;
    [SerializeField]private Transform pos10;
    [SerializeField]private Transform pos11;
    [SerializeField]private Transform pos12;
    [SerializeField]private Transform pos13;
    [SerializeField]private Transform pos14;

    [SerializeField]private Transform player;

    //ui
    [SerializeField]private GameObject endgame;
    [SerializeField]private GameObject notend;
    [SerializeField]private GameObject tutorial;
    [SerializeField]private GameObject gameover;
    //[SerializeField]private int checkpointCount = 1;

    private Vector3 prespawnPoint;

    private PlayerMovement1 CC;

    //scores
    [SerializeField]private int diamonds = 0;
    [SerializeField]private int max_diamonds;
    public Text diamondText;

    void Start()
    {
        PlayerMovement1 CC = player.GetComponent<PlayerMovement1>();
        prespawnPoint = pos1.transform.position;
        TutorialScreen();
    }

    void Update()
    {
        diamondText.text = diamonds + "/" + max_diamonds;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Respawn()
    {
        //reload player position to respawn point
        //reload diamonds and enemies if not yet cleared this part of level
        //if cleared, no longer need to reload
        transform.position = prespawnPoint;
        FindObjectOfType<PlayerMovement1>().Spawn();
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.CompareTag("DEAD"))
        {
           FindObjectOfType<Rigidbody2D>().velocity = Vector3.zero;
           FindObjectOfType<PlayerMovement1>().Dead();
           Debug.Log("You are dead");
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "checkpoint")
        {
            if(coll.gameObject.name == "CP1")
            {
                prespawnPoint = pos1.transform.position;
            }
            else if(coll.gameObject.name == "CP2")
            {
                prespawnPoint = pos2.transform.position;
            }
            else if(coll.gameObject.name == "CP3")
            {
                prespawnPoint = pos3.transform.position;
            }
            else if(coll.gameObject.name == "CP4")
            {
                prespawnPoint = pos4.transform.position;
            }
            else if(coll.gameObject.name == "CP5")
            {
                prespawnPoint = pos5.transform.position;
            }
            else if(coll.gameObject.name == "CP6")
            {
                prespawnPoint = pos6.transform.position;
            }
            else if(coll.gameObject.name == "CP6b")
            {
                prespawnPoint = pos6.transform.position;
            }
            else if(coll.gameObject.name == "CP7")
            {
                prespawnPoint = pos7.transform.position;
            }
            else if(coll.gameObject.name == "CP8")
            {
                prespawnPoint = pos8.transform.position;
            }
            else if(coll.gameObject.name == "CP9")
            {
                prespawnPoint = pos9.transform.position;
            }
            else if(coll.gameObject.name == "CP10")
            {
                prespawnPoint = pos10.transform.position;
            }
            else if(coll.gameObject.name == "CP11")
            {
                prespawnPoint = pos11.transform.position;
            }
            else if(coll.gameObject.name == "CP12")
            {
                prespawnPoint = pos12.transform.position;
            }
            else if(coll.gameObject.name == "CP13")
            {
                prespawnPoint = pos13.transform.position;
            }
            else if(coll.gameObject.name == "CP14")
            {
                prespawnPoint = pos14.transform.position;
            }
        }
        if(coll.gameObject.CompareTag("Diamond"))
        {
            //scoring diamonds
            SoundManager.PlaySound("playerScore");
            Destroy(coll.gameObject);
            diamonds++;
        }

        if(coll.gameObject.CompareTag("Endgame"))
        {
            Debug.Log("I am touching end");

            if(diamonds >= max_diamonds)
            {
                //show end game ui
                //disable movement
                //button to restart
                PauseGame();
                endgame.SetActive(true);
            }
            else if(diamonds < max_diamonds)
            {
                notend.SetActive(true);
            }

        }    
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.gameObject.CompareTag("Endgame"))
        {
            Debug.Log("I have left");
            notend.SetActive(false);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    //UI
    void TutorialScreen()
    {
        PauseGame();
        tutorial.SetActive(true);
    }

    public void GameOverScreen()
    {
        PauseGame();
        gameover.SetActive(true);
    }

}
