using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GAME;
    public static Transform POOL;
    public static float HEALTH, ARMOR, ARROWS, BOMBS, GOLD, POINTS;
    public static bool PAUSED;

    public GameObject QuitPanel;
    public float InvicibibleTime, InvincibleRateOfDecay;

    private void Awake()
    {
        GAME = this;
        POOL = GameObject.FindGameObjectWithTag("Pool").transform;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (QuitPanel.activeSelf) //checks if QuitPanel has been popped
        {
            if (Input.GetButtonUp("Fire1")) UnityEngine.SceneManagement.SceneManager.LoadScene(0); //quit game, back to intro screen
            if (Input.GetButtonUp("Fire2") || Input.GetButtonUp("Cancel")) //hide quit panel
            {
                QuitPanel.SetActive(false);
                PAUSED = false;
                //Switch music back to game music
            }
        }
        
    }

    public void PopQuitMenu()
    {
        QuitPanel.SetActive(true);
        //Switch music to menu music
    }
}

