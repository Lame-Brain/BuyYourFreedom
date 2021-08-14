using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GAME;
    public static Transform POOL;
    public static float HEALTH, ARMOR, ARROWS, BOMBS, GOLD, POINTS, FREEDOM;
    public static bool PAUSED;
    public static int SECONDS_LEFT;

    public GameObject QuitPanel, StorePanel;
    public float InvicibibleTime, InvincibleRateOfDecay;
    [HideInInspector] public List<GameObject> ArrowPool = new List<GameObject>();
    [HideInInspector] public List<GameObject> BombPool = new List<GameObject>();
    [HideInInspector] public List<GameObject> RockPool = new List<GameObject>();
    public GameObject arrow_prefab, bomb_prefab, rock_prefab, pop_prefab;
    public GameObject HealthUp_prefab, ArmorUp_prefab, CoinUp_prefab, PointsUp_prefab, MoreArrows_prefab, MoreBombs_prefab;
    public GameObject[] floor, wall, monster;
    public GameObject GUI, InfoPop_prefab, littleInfo_prefab;
    public int sword_bonus, arrow_bonus, bomb_bonus;
    public Transform MonsterPoolObject, LootRootObject;

    private int _wave;
    private bool _readyForNextPhase;
    private string _phase;
    
    private void Awake()
    {
        GAME = this;
        POOL = GameObject.FindGameObjectWithTag("Pool").transform;

        //for (int _i = 0; _i < 11; _i++) Debug.Log(_i + ". " + ((int)_i/5));
    }

    private void Start()
    {
        //Default player
        HEALTH = 25;
        ARMOR = 0;
        ARROWS = 6;
        BOMBS = 1;
        GOLD = 0;
        POINTS = 0;
        FREEDOM = 100000;

        _wave = 1;
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

        if (QuitPanel.activeSelf && Input.GetButtonUp("Fire1")) Yes_Quit(false);
        
        //Phases
        if(_readyForNextPhase && _phase == "KILL")
        {
            _readyForNextPhase = false;
            //spawn waves of bad guys
            SpawnWave(_wave);
            _wave ++;
            GameObject.FindGameObjectWithTag("Player").GetComponent<I_am_a_Player>().Init_Play();


            //Refill the seconds (I think 60 for this phase)
            SECONDS_LEFT = 60;

            //Start the timer
            StartCoroutine(Countdown(SECONDS_LEFT));
        }

        if(_readyForNextPhase && _phase == "LOOT")
        {
            _readyForNextPhase = false;

            if (GameObject.FindGameObjectsWithTag("Grave") != null) //IF there are no graves, go directly to next phase
            {
                float _delta = 1f;
                GameObject _hpUp = null, _apUp = null, _xpUp = null, _gpUp = null, _arrwUp = null, _bmbUp = null;
                foreach(GameObject _go in GameObject.FindGameObjectsWithTag("Grave"))
                {
                    //Convert all graves into loot
                    Instantiate(pop_prefab, _go.transform.position, Quaternion.identity);
                    _hpUp = null;
                    if (_go.GetComponent<I_am_a_grave>().num_Hearts > 0) _hpUp = Instantiate(HealthUp_prefab, new Vector2(_go.transform.position.x + (Random.Range(-_delta, _delta)), _go.transform.position.y + (Random.Range(-_delta, _delta))), Quaternion.identity, LootRootObject);
                    if (_hpUp != null) { _hpUp.GetComponent<I_am_a_PowerUp>().health = _go.GetComponent<I_am_a_grave>().num_Hearts; }

                    _apUp = null;
                    if (_go.GetComponent<I_am_a_grave>().num_Shields > 0) _apUp = Instantiate(ArmorUp_prefab, new Vector2(_go.transform.position.x + (Random.Range(-_delta, _delta)), _go.transform.position.y + (Random.Range(-_delta, _delta))), Quaternion.identity, LootRootObject);
                    if (_apUp != null) { _apUp.GetComponent<I_am_a_PowerUp>().armor = _go.GetComponent<I_am_a_grave>().num_Shields; }

                    _gpUp = null;
                    if (_go.GetComponent<I_am_a_grave>().num_Coins > 0) _gpUp = Instantiate(CoinUp_prefab, new Vector2(_go.transform.position.x + (Random.Range(-_delta, _delta)), _go.transform.position.y + (Random.Range(-_delta, _delta))), Quaternion.identity, LootRootObject);
                    if (_gpUp != null) { _gpUp.GetComponent<I_am_a_PowerUp>().gold = _go.GetComponent<I_am_a_grave>().num_Coins; }

                    _arrwUp = null;
                    if (_go.GetComponent<I_am_a_grave>().num_Arrows > 0) _arrwUp = Instantiate(MoreArrows_prefab, new Vector2(_go.transform.position.x + (Random.Range(-_delta, _delta)), _go.transform.position.y + (Random.Range(-_delta, _delta))), Quaternion.identity, LootRootObject);
                    if (_arrwUp != null) { _arrwUp.GetComponent<I_am_a_PowerUp>().arrows = _go.GetComponent<I_am_a_grave>().num_Arrows; }

                    _bmbUp = null;
                    if (_go.GetComponent<I_am_a_grave>().num_Bombs > 0) _bmbUp = Instantiate(MoreBombs_prefab, new Vector2(_go.transform.position.x + (Random.Range(-_delta, _delta)), _go.transform.position.y + (Random.Range(-_delta, _delta))), Quaternion.identity, LootRootObject);
                    if (_bmbUp != null) { _bmbUp.GetComponent<I_am_a_PowerUp>().bombs = _go.GetComponent<I_am_a_grave>().num_Bombs; }

                    _xpUp = null;
                    if (_go.GetComponent<I_am_a_grave>().num_Bags > 0)
                        for (int _i = 0; _i < _go.GetComponent<I_am_a_grave>().num_Bags; _i++)
                        {
                            _xpUp = Instantiate(PointsUp_prefab, new Vector2(_go.transform.position.x + (Random.Range(-_delta, _delta)), _go.transform.position.y + (Random.Range(-_delta, _delta))), Quaternion.identity, LootRootObject);
                            _xpUp.GetComponent<I_am_a_PowerUp>().points = _go.GetComponent<I_am_a_grave>().bag_Scale;
                        }
                    Destroy(_go);
                }

                for(int _i = 0; _i < LootRootObject.childCount; _i++)
                {
                    if (LootRootObject.GetChild(_i).position.x > 12.5f) LootRootObject.GetChild(_i).position = new Vector2(12.5f, LootRootObject.GetChild(_i).position.y);
                    if (LootRootObject.GetChild(_i).position.x < -12.5f) LootRootObject.GetChild(_i).position = new Vector2(-12.5f, LootRootObject.GetChild(_i).position.y);
                    if (LootRootObject.GetChild(_i).position.y > 12.5f) LootRootObject.GetChild(_i).position = new Vector2(LootRootObject.GetChild(_i).position.x, 12.5f);
                    if (LootRootObject.GetChild(_i).position.y < -12.5f) LootRootObject.GetChild(_i).position = new Vector2(LootRootObject.GetChild(_i).position.x, -12.5f);
            }

            //10 seconds on the clock
            SECONDS_LEFT = 10;                

                //Start the timer
                StartCoroutine(Countdown(SECONDS_LEFT));                
            }
            
        }

        if(_readyForNextPhase && _phase == "BUY")
        {
            _readyForNextPhase = false;
            //Pop the store screen
            StorePanel.SetActive(true);
            StorePanel.GetComponent<I_am_a_Store>().InitStore();           

            //Switch to store music

            //activate store controls
            GameObject.FindGameObjectWithTag("Player").GetComponent<I_am_a_Player>().canMove = false;

            //10 seconds on the clock
            //SECONDS_LEFT = 10;
            SECONDS_LEFT = 10;

            //Start the timer
            StartCoroutine(Countdown(SECONDS_LEFT));
        }

    }

    public void PopQuitMenu()
    {
        PAUSED = true;
        QuitPanel.SetActive(true);
        //Switch music to menu music
    }

    public void SpawnWave(int _t)
    {
        int _num = _t + 4; if (_num > 25) _num = 25;
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


            int x1 = 0, x2 = 0, y1 = 0, y2 = 0, zone = Random.Range(1,5);
            if (zone == 1) //North Gate
            {
                x1 = -2; y1 = 11;
                x2 = 2; y2 = 12;
            }
            if (zone == 2) //East Gate
            {
                x1 = 11; y1 = -2;
                x2 = 12; y2 = 2;
            }
            if (zone == 3) // South Gate
            {
                x1 = -2; y1 = -12;
                x2 = 2; y2 = -11;
            }
            if (zone == 4) // West Gate
            {
                x1 = -12; y1 = -2;
                x2 = -11; y2 = 2;
            }

            GameObject _monster = Instantiate(monster[_selected_index], new Vector3(Random.Range(x1, x2), Random.Range(y1, y2)), Quaternion.identity, MonsterPoolObject);
            _monster.GetComponentInChildren<I_am_an_Enemy>().health += (int)(_wave / 5);
            //_monster.GetComponent<I_am_an_Enemy>().damage += sword_bonus;
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

    IEnumerator Countdown(int _c)
    {
        while(SECONDS_LEFT > 0)
        {
            yield return new WaitForSeconds(1f);
            if(!PAUSED) SECONDS_LEFT--;
            if (_phase == "KILL" && MonsterPoolObject.childCount == 0) SECONDS_LEFT = 0;
            if (_phase == "LOOT" && LootRootObject.childCount == 0) SECONDS_LEFT = 0;
            if (_phase == "BUY" && GOLD <= 0) SECONDS_LEFT = 0;
        }

        //Phase Clean up
        if(_phase == "KILL" && !_readyForNextPhase)
        {
            //Destroy any remaining enemies
            foreach (GameObject _enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Instantiate(pop_prefab, _enemy.transform.position, Quaternion.identity);
                Destroy(_enemy);
            }
            foreach (GameObject _enemy in GameObject.FindGameObjectsWithTag("Heavy Enemy"))
            {
                Instantiate(pop_prefab, _enemy.transform.position, Quaternion.identity);
                Destroy(_enemy);
            }

            _phase = "LOOT";
            _readyForNextPhase = true;
        }
        
        if(_phase == "LOOT" && !_readyForNextPhase)
        {
            //Destroy any remaining Powerups
            foreach (GameObject _pu in GameObject.FindGameObjectsWithTag("PowerUp"))
            {
                Instantiate(pop_prefab, _pu.transform.position, Quaternion.identity);
                Destroy(_pu);
            }

            _phase = "BUY";
            _readyForNextPhase = true;
        }
        
        if(_phase == "BUY" && !_readyForNextPhase)
        {            
            StorePanel.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<I_am_a_Player>().canMove = true;            
            _phase = "KILL";
            _readyForNextPhase = true;
        }
    }

    public void InfoTextPop(Vector2 _pos, string _message, Color _color)
    {
        GameObject _popup;
        Vector2 _screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, _pos);
        
        _popup = Instantiate(InfoPop_prefab, _screenPoint, Quaternion.identity, GUI.transform);
        _popup.GetComponent<TMPro.TextMeshProUGUI>().text = _message;
        _popup.GetComponent<TMPro.TextMeshProUGUI>().color = _color;
    }
    public void LittleTextPop(Vector2 _pos, string _message)
    {
        GameObject _popup;
        Vector2 _screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, _pos);
        
        _popup = Instantiate(littleInfo_prefab, _screenPoint, Quaternion.identity, GUI.transform);
        _popup.GetComponent<TMPro.TextMeshProUGUI>().text = _message;
    }

    public void Yes_Quit(bool yes) 
    {
        PAUSED = false;
        if(yes) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        if (!yes)
        {
            QuitPanel.SetActive(false);
            //Switch back to game music
        }
    }
}

