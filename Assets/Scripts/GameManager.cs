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
    [HideInInspector] public List<GameObject> ArrowPool = new List<GameObject>();
    [HideInInspector] public List<GameObject> BombPool = new List<GameObject>();
    [HideInInspector] public List<GameObject> RockPool = new List<GameObject>();
    public GameObject arrow_prefab, bomb_prefab, rock_prefab;
    public GameObject[] floor, wall, monster;
    private void Awake()
    {
        GAME = this;
        POOL = GameObject.FindGameObjectWithTag("Pool").transform;
    }

    private void Start()
    {
        //Default player
        HEALTH = 25;
        ARMOR = 0;
        ARROWS = 6;
        BOMBS = 0;
        GOLD = 0;
        POINTS = 0;

        //build level
        for (int _y = -10; _y < 11; _y++)
        {
            for (int _x = -10; _x < 11; _x++)
            {
                Instantiate(floor[Random.Range(0, floor.Length)], new Vector3(_x, _y, 0), Quaternion.identity);
            }
        }
        for (int _i = -11; _i < 12; _i++)
        {
            Instantiate(wall[Random.Range(0, wall.Length)], new Vector3(_i, -11, 0), Quaternion.identity);
            Instantiate(wall[Random.Range(0, wall.Length)], new Vector3(_i, 11, 0), Quaternion.identity);
            Instantiate(wall[Random.Range(0, wall.Length)], new Vector3(-11, _i, 0), Quaternion.identity);
            Instantiate(wall[Random.Range(0, wall.Length)], new Vector3(11, _i, 0), Quaternion.identity);
        }
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

    public void Reset_Pools()
    {
        //clean up existing projectiles
        foreach (GameObject _go in GameObject.FindGameObjectsWithTag("Arrow")) Destroy(_go);
        foreach (GameObject _go in GameObject.FindGameObjectsWithTag("Bomb")) Destroy(_go);
        foreach (GameObject _go in GameObject.FindGameObjectsWithTag("Rock")) Destroy(_go);

        //Clear lists
        ArrowPool.Clear();
        BombPool.Clear();
        RockPool.Clear();

        //Initialize object pools
        int _Pool_Amount = 25;
        for (int _i = 0; _i < _Pool_Amount; _i++)
        {
            ArrowPool.Add(Instantiate(arrow_prefab, POOL.position, Quaternion.identity));
            BombPool.Add(Instantiate(bomb_prefab, POOL.position, Quaternion.identity));
            RockPool.Add(Instantiate(rock_prefab, POOL.position, Quaternion.identity));
            //RockPool.Add(Instantiate(rock_prefab, POOL.position, Quaternion.identity));
        }
    }
}

