using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gamemanager : MonoBehaviour
{
   [HideInInspector] public static gamemanager instance;
    [Header("---- Player References ----")]
    public GameObject player;
    public playerController playerScript;

    [Header("----- UI -----")]
    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject playerDamageFlash;
    public GameObject winGameMenu;
    public Image HPBar;
    public TMP_Text enemyDead;
    public TMP_Text enemyTotal;
    [Header("----- Game Goals -----")]
    public int enemyKillGoal;
    int enemiesKilled;

    [HideInInspector] public GameObject menuCurrentOpen;
    [HideInInspector] public bool pause = false;
    [HideInInspector] public bool gameOver;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel") && !gameOver)
        {

          if(!pause && !menuCurrentOpen)
          {
                pause = true;
                menuCurrentOpen = pauseMenu;
                menuCurrentOpen.SetActive(true);
                lockCursorPause();
            }
          else
          {
                resume();
                
            }
        }
    }
    public void resume()
    {
        pause = false;
        menuCurrentOpen.SetActive(false);
        menuCurrentOpen = null;
        UnlockCursorUnpause();
    }
    public void playerDead()
    {
        gameOver = true;
        menuCurrentOpen = playerDeadMenu;
        menuCurrentOpen.SetActive(true);
        lockCursorPause();

    }
    public void checkEnemyKills()
    {
        enemiesKilled++;
        enemyDead.text = enemiesKilled.ToString("F0");

        if (enemiesKilled >= enemyKillGoal)
        {
            menuCurrentOpen = winGameMenu;
            menuCurrentOpen.SetActive(true);
            gameOver = true;
            lockCursorPause();
        }
    }
    public void restart()
    {
        gameOver = false;
        menuCurrentOpen.SetActive(false);
        menuCurrentOpen=null;
        UnlockCursorUnpause();
    }
    public void lockCursorPause()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void UnlockCursorUnpause()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked; //Old line wasCursor.lockState &= ~CursorLockMode.Locked
        Cursor.visible = false;
    }
    
    public void updateEnemyNumber()
    {
        enemyKillGoal++;
        enemyTotal.text = enemyKillGoal.ToString("F0");
    }

}
