using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GAME;
    public static Transform POOL;
    public static float HEALTH, ARMOR, ARROWS, BOMBS, GOLD, POINTS;
    public static bool PAUSED;
    public static int SECONDS_LEFT;

    public GameObject QuitPanel;
    public float InvicibibleTime, InvincibleRateOfDecay;
    [HideInInspector] public List<GameObject> ArrowPool = new List<GameObject>();
    [HideInInspector] public List<GameObject> BombPool = new List<GameObject>();
    [HideInInspector] public List<GameObject> RockPool = new List<GameObject>();
    public GameObject arrow_prefab, bomb_prefab, rock_prefab;
    public GameObject[] floor, wall, monster;

    private int _wave;
    private bool _readyForNextPhase;
    private string _phase;

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

        _wave = 5;
        _readyForNextPhase = true;
        _phase = "KILL";

        //build level
        for (int _y = -12; _y < 13; _y++)
        {
            for (int _x = -12; _x < 13; _x++)
            {
                Instantiate(floor[Random.Range(0, floor.Length)], new Vector3(_x, _y, 0), Quaternion.identity);
            }
        }
        for (int _i = -13; _i < 14; _i++)
        {
            Instantiate(wall[Random.Range(0, wall.Length)], new Vector3(_i, -13, 0), Quaternion.identity);
            Instantiate(wall[Random.Range(0, wall.Length)], new Vector3(_i, 13, 0), Quaternion.identity);
            Instantiate(wall[Random.Range(0, wall.Length)], new Vector3(-13, _i, 0), Quaternion.identity);
            Instantiate(wall[Random.Range(0, wall.Length)], new Vector3(13, _i, 0), Quaternion.identity);
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
        
        //Phases
        if(_readyForNextPhase && _phase == "KILL")
        {
            _readyForNextPhase = false;
            //spawn waves of bad guys
            SpawnWave(_wave);
            _wave += 2;
            if (_wave > 25) _wave = 25;

            //Refill the seconds (I think 60 for this phase)
            SECONDS_LEFT = 60;

            //Start the timer
            StartCoroutine(Countdown());
        }

        if(_readyForNextPhase && _phase == "LOOT")
        {
            _readyForNextPhase = false;
            //Convert all graves into loot
            //10 seconds on the clock
            //Start the timer
            //IF there are no graves, go directly to next phase
            _phase = "BUY";
        }

        if(_readyForNextPhase && _phase == "BUY")
        {
            _readyForNextPhase = false;
            //Pop the store screen
            //activate store controls
            //10 seconds on the clock
            //Start the timer
            _phase = "KILL";
        }
    }

    public void PopQuitMenu()
    {
        QuitPanel.SetActive(true);
        //Switch music to menu music
    }

    public void SpawnWave(int _num)
    {
        for(int _n = 0; _n < _num; _n++)
        {
            int _random = Random.Range(1, 66);
            int _selected_index = 0;
            if (_random > 14) _selected_index = 1;
            if (_random > 26) _selected_index = 2;
            if (_random > 34) _selected_index = 3;
            if (_random > 41) _selected_index = 4;
            if (_random > 47) _selected_index = 5;
            if (_random > 52) _selected_index = 6;
            if (_random > 56) _selected_index = 7;
            if (_random > 60) _selected_index = 8;
            if (_random > 63) _selected_index = 9;
            if (_random == 66) _selected_index = 10;


            int x1 = 0, x2 = 0, y1 = 0, y2 = 0, zone = 1;
            if (zone == 1) //North Gate
            {
                x1 = -2; y1 = 11;
                x2 = 2; y2 = 12;
            }
            if (zone == 1) //East Gate
            {
                x1 = 11; y1 = -2;
                x2 = 12; y2 = 2;
            }
            if (zone == 1) // South Gate
            {
                x1 = -2; y1 = -12;
                x2 = 2; y2 = -11;
            }
            if (zone == 1) // West Gate
            {
                x1 = -12; y1 = -2;
                x2 = -11; y2 = 2;
            }

            Instantiate(monster[_selected_index], new Vector3(Random.Range(x1, x2), Random.Range(y1, y2)), Quaternion.identity);
        }
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

    IEnumerator Countdown()
    {
        for (int _c = 0; _c < SECONDS_LEFT; _c++)
        {
            yield return new WaitForSeconds(1f);
        }

        //Phase Clean up
        if(_phase == "KILL")
        {
            //Destroy any remaining enemies
            _phase = "LOOT";
        }
        else if(_phase == "LOOT")
        {
            //Destroy any remaining Powerups
            _phase = "BUY";
        }
        else if(_phase == "BUY")
        {
            //Set store screen to inactive
            _phase = "KILL";
        }

        _readyForNextPhase = true;

    }
}

