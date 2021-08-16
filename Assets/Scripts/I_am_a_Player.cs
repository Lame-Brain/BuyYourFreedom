using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_Player : MonoBehaviour
{
    public enum Directions { North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest }
    public enum Weapons { None, Sword, Arrow, Bomb }
    public GameObject myFace;
    public bool canMove;
    public float speed, dashModifier, arrow_reloadTime, bomb_reloadTime;
    public Weapons selectedWeapon;
    public AudioSource SlashSFX, OuchSFX, TinkSFX, ClickSFX;

    bool _move_up, _move_right, _move_down, _move_left, _button1, _button2, _button3, _Esc, _changedDirection, _ready_arrow, _ready_bomb;
    Directions _facing, _old_facing, _new_facing;
    Rigidbody2D _rigidBody;
    float _invincibilityTimer;
    int _arrow_index, _bomb_index;

    private void Start()
    {
        Init_Play();
    }
    public void Init_Play()
    {
        _facing = Directions.North;
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        selectedWeapon = Weapons.Sword;
        _invincibilityTimer = 0f;
        //StartCoroutine(Invincible_Timer());
        _arrow_index = -1;
        _bomb_index = -1;
        GameManager.GAME.Reset_Pools();
        StartCoroutine(ReloadArrow());
        StartCoroutine(ReloadBomb());
    }

    private void Update()
    {
        _move_up = false; _move_left = false; _move_down = false; _move_right = false;
        _button1 = false; _button2 = false; _button3 = false; _Esc = false;
        _old_facing = _facing; _changedDirection = false;

        //CONTROLS
        if (!GameManager.PAUSED && canMove)
        {
            //Get axis Input
            if (Input.GetAxisRaw("Vertical") > 0) _move_up = true;
            if (Input.GetAxisRaw("Horizontal") > 0) _move_left = true;
            if (Input.GetAxisRaw("Vertical") < 0) _move_down = true;
            if (Input.GetAxisRaw("Horizontal") < 0) _move_right = true;

            //Get Button Input
            if (Input.GetButtonUp("Cancel")) _Esc = true;
            if (Input.GetButtonUp("Fire1")) _button1 = true;
            if (Input.GetButtonUp("Fire2")) _button2 = true;
            if (Input.GetButton("Fire3")) _button3 = true;
        }

        //Facing
        if (_move_up && !_move_left && !_move_right && !_move_down) _new_facing = Directions.North;
        if (_move_up && _move_left && !_move_right && !_move_down) _new_facing = Directions.NorthEast;
        if (!_move_up && _move_left && !_move_right && !_move_down) _new_facing = Directions.East;
        if (!_move_up && _move_left && !_move_right && _move_down) _new_facing = Directions.SouthEast;
        if (!_move_up && !_move_left && !_move_right && _move_down) _new_facing = Directions.South;
        if (!_move_up && !_move_left && _move_right && _move_down) _new_facing = Directions.SouthWest;
        if (!_move_up && !_move_left && _move_right && !_move_down) _new_facing = Directions.West;
        if (_move_up && !_move_left && _move_right && !_move_down) _new_facing = Directions.NorthWest;
        if (_old_facing != _new_facing) _changedDirection = true;
        _facing = _new_facing;

        //Movement
        if (_facing == Directions.North) transform.eulerAngles = new Vector3(0, 0, 0);
        if (_facing == Directions.NorthEast) transform.eulerAngles = new Vector3(0, 0, 315);
        if (_facing == Directions.East) transform.eulerAngles = new Vector3(0, 0, 270);
        if (_facing == Directions.SouthEast) transform.eulerAngles = new Vector3(0, 0, 225);
        if (_facing == Directions.South) transform.eulerAngles = new Vector3(0, 0, 180);
        if (_facing == Directions.SouthWest) transform.eulerAngles = new Vector3(0, 0, 135);
        if (_facing == Directions.West) transform.eulerAngles = new Vector3(0, 0, 90);
        if (_facing == Directions.NorthWest) transform.eulerAngles = new Vector3(0, 0, 45);
        float _totalSpeed = speed;
        if (_button3)
        {
            _totalSpeed += dashModifier;            
            //Play Dash AudioClip
        }
        if (_move_up || _move_left || _move_down || _move_right) transform.Translate(Vector2.up * _totalSpeed * Time.deltaTime);

        //Slashing Sound
        if (selectedWeapon == Weapons.Sword && _changedDirection) SlashSFX.PlayOneShot(SlashSFX.clip);

        //Button 1 (fire selected weapon)
        if(_button1 && selectedWeapon == Weapons.Arrow && _ready_arrow && GameManager.ARROWS > 0)
        {
            for (int _i = 0; _i < GameManager.GAME.ArrowPool.Count; _i++) if (!GameManager.GAME.ArrowPool[_i].GetComponent<I_am_an_Arrow>().inFlight) _arrow_index = _i;
            GameManager.GAME.ArrowPool[_arrow_index].transform.position = transform.position;
            GameManager.GAME.ArrowPool[_arrow_index].transform.rotation = transform.rotation;
            GameManager.GAME.ArrowPool[_arrow_index].GetComponent<I_am_an_Arrow>().Start_Flight();
            StartCoroutine(ReloadArrow());
            GameManager.ARROWS--;
            _button1 = false;
            //PLAY FIRE ARROW SOUND
        }
        if(_button1 && selectedWeapon == Weapons.Bomb && _ready_bomb && GameManager.BOMBS > 0)
        {
            //figure out which bomb in pool is next
            _bomb_index++;
            if (_bomb_index >= GameManager.GAME.BombPool.Count - 1) _bomb_index = 0;
            GameManager.GAME.BombPool[_bomb_index].transform.position = transform.position;
            GameManager.GAME.BombPool[_bomb_index].transform.rotation = transform.rotation;
            GameManager.GAME.BombPool[_bomb_index].transform.Translate(Vector2.down * 0.5f);
            GameManager.GAME.BombPool[_bomb_index].GetComponent<I_am_a_Bomb>().Arm_Bomb();
            StartCoroutine(ReloadBomb());
            GameManager.BOMBS--;
            _button2 = false;
            //PLAY BOMB PLOP SOUND
        }

        //Button 2 (change weapon selection)
        if (_button2)
        {
            if (selectedWeapon == Weapons.Sword) { selectedWeapon = Weapons.Arrow; _button2 = false; ClickSFX.PlayOneShot(ClickSFX.clip); }
            if (selectedWeapon == Weapons.Arrow && _button2) { selectedWeapon = Weapons.Bomb; _button2 = false; ClickSFX.PlayOneShot(ClickSFX.clip); }
            if (selectedWeapon == Weapons.Arrow && GameManager.ARROWS < 1) { selectedWeapon = Weapons.Bomb; ClickSFX.PlayOneShot(ClickSFX.clip); }
                if (selectedWeapon == Weapons.Bomb && _button2) { selectedWeapon = Weapons.Sword; _button2 = false; SlashSFX.PlayOneShot(ClickSFX.clip); }
            if (selectedWeapon == Weapons.Bomb && GameManager.BOMBS < 1) { selectedWeapon = Weapons.Sword; SlashSFX.PlayOneShot(ClickSFX.clip); }
            }

        //Weapon Selection
        if(selectedWeapon == Weapons.None)
        {
            transform.Find("Sword").gameObject.SetActive(false);
            transform.Find("Arrow").gameObject.SetActive(false);
            transform.Find("Bomb").gameObject.SetActive(false);
        }
        if(selectedWeapon == Weapons.Sword)
        {
            transform.Find("Sword").gameObject.SetActive(true);
            transform.Find("Arrow").gameObject.SetActive(false);
            transform.Find("Bomb").gameObject.SetActive(false);
        }
        if(selectedWeapon == Weapons.Arrow)
        {
            transform.Find("Sword").gameObject.SetActive(false);
            transform.Find("Arrow").gameObject.SetActive(_ready_arrow);
            transform.Find("Bomb").gameObject.SetActive(false);
        }
        if(selectedWeapon == Weapons.Bomb)
        {
            transform.Find("Sword").gameObject.SetActive(false);
            transform.Find("Arrow").gameObject.SetActive(false);
            transform.Find("Bomb").gameObject.SetActive(_ready_bomb);
        }

        //Escape Button: Quit Panel
        if (_Esc)
        {
            _Esc = false;
            GameManager.GAME.PopQuitMenu();
        }

        //Update check for death
        if (GameManager.HEALTH <= 0) PlayerDies();
    }

    //Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {   
        if (_invincibilityTimer == 0)
        {            
            if(collision.collider.gameObject.tag == "Splosion")
            {
                PlayerDamage(collision.collider.GetComponent<Boom>().damage + GameManager.GAME.bomb_bonus);
                StartCoroutine(Invincible_Timer());
                if (GameManager.HEALTH <= 0) PlayerDies();
            }
            if(collision.collider.gameObject.tag == "Enemy" || collision.collider.tag == "Heavy Enemy")
            {
                if(collision.collider.gameObject.GetComponent<I_am_an_Enemy>() != null) PlayerDamage(collision.collider.gameObject.GetComponent<I_am_an_Enemy>().damage);
                if (collision.collider.gameObject.GetComponent<I_am_an_Dragon>() != null) PlayerDamage(collision.collider.gameObject.GetComponent<I_am_an_Dragon>().damage);
                StartCoroutine(Invincible_Timer());
                if (GameManager.HEALTH <= 0) PlayerDies();
                Vector2 dir = collision.collider.transform.position - transform.position;
                dir = -dir.normalized;
                float _force = 0;
                if (collision.collider.gameObject.tag == "Enemy") _force = 10000;
                if (collision.collider.gameObject.tag == "Heavy_Enemy") _force = 2000;
                GetComponent<Rigidbody2D>().AddForce(dir * _force);
            }
            if(collision.collider.gameObject.tag == "Rock")
            {
                PlayerDamage(collision.collider.gameObject.GetComponent<I_am_an_Arrow>().damage + GameManager.GAME.arrow_bonus);
                collision.collider.gameObject.GetComponent<I_am_an_Arrow>().Stop_Flight();
                StartCoroutine(Invincible_Timer());
                if (GameManager.HEALTH <= 0) PlayerDies();
            }
        } 
    }
    
    //Handle Player Damage
    public void PlayerDamage(float _damage)
    {
        float _adjustedDamage = _damage;
        _adjustedDamage -= GameManager.ARMOR; if (_adjustedDamage < 0) _adjustedDamage = 0;
        GameManager.ARMOR -= _damage; if (GameManager.ARMOR < 0) GameManager.ARMOR = 0;
        GameManager.HEALTH -= _adjustedDamage;
        if(_adjustedDamage == 0)
        {
            //Play armor tink sound
            if (!OuchSFX.isPlaying && !TinkSFX.isPlaying) TinkSFX.PlayOneShot(TinkSFX.clip);
        }
        else
        {
            //Play player ouch sound
            if(!OuchSFX.isPlaying && !TinkSFX.isPlaying) OuchSFX.PlayOneShot(OuchSFX.clip);
            GameManager.GAME.InfoTextPop(transform.position, "-" + _adjustedDamage, Color.red);
        }

        Camera.main.GetComponent<Camera_Script>().ScreenShake(.075f);
    }

    //Handle Player Death
    public void PlayerDies()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    IEnumerator Invincible_Timer()
    {
        _invincibilityTimer = GameManager.GAME.InvicibibleTime;
        myFace.GetComponent<Animator>().SetBool("Flash", true);
        for(float _c = _invincibilityTimer; _c > 0f; _c -= GameManager.GAME.InvincibleRateOfDecay) yield return new WaitForSeconds(1f);
        myFace.GetComponent<Animator>().SetBool("Flash", false);
        _invincibilityTimer = 0;
    }

    IEnumerator ReloadArrow()
    {
        _ready_arrow = false;
        yield return new WaitForSeconds(arrow_reloadTime);
        _ready_arrow = true;
    }
    IEnumerator ReloadBomb()
    {
        _ready_bomb = false;
        yield return new WaitForSeconds(bomb_reloadTime);
        _ready_bomb = true;
    }
}
