using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_an_Enemy : MonoBehaviour
{
    public enum Monster { beserker, charger, ranger }
    public Monster type;
    public GameObject myFace;
    public float health;
    public float speed;
    public float damage;
    public float delayBetweenShots;

    public GameObject Grave_Prefab;

    Rigidbody2D _rigidBody;
    float _invincibilityTimer;
    Transform _Target;
    //Type behavior variables
    bool _inRange;
    bool _charging;
    float _delay;

    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(type == Monster.beserker)
        {
            _Target = GameObject.FindGameObjectWithTag("Player").transform;
            transform.up = _Target.position - transform.position; //face player with y axis
            transform.Translate(Vector2.up * speed * Time.deltaTime); //move along y axis              
            //chance to play travel sound
        }
        if (type == Monster.charger)
        {
            if(!_inRange && _delay == 0 && !_charging)
            {
                _Target = GameObject.FindGameObjectWithTag("Player").transform;
                transform.up = _Target.position - transform.position; //face player with y axis
                transform.Translate(Vector2.up * speed * Time.deltaTime); //move along y axis
                //chance to play travel sound
            }
            if (_inRange && _delay == 0 && !_charging)
            {
                transform.up = _Target.position - transform.position; //face player with y axis
                StartCoroutine(MonsterCharges());
            }
            if (_charging) transform.Translate(Vector2.up * speed * 3 * Time.deltaTime); //move along y axis
        }
        if(type == Monster.ranger)
        {
            if (!_inRange)
            {
                _Target = GameObject.FindGameObjectWithTag("Player").transform;
                transform.up = _Target.position - transform.position; //face player with y axis
                transform.Translate(Vector2.up * speed * Time.deltaTime); //move along y axis
                //chance to play travel sound
            }
            if (_inRange)
            {
                _Target = GameObject.FindGameObjectWithTag("Player").transform;
                transform.up = _Target.position - transform.position; //face player with y axis
                transform.Translate(Vector2.down * speed * Time.deltaTime); //move back along y axis
                transform.Translate(Vector2.right * speed * 2 * Time.deltaTime); //move sideways
                //chance to play travel sound
            }
            if (_inRange && _delay == 0) StartCoroutine(MonsterShoots());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_invincibilityTimer == 0)
        {
            if (collision.collider.gameObject.tag == "Splosion")
            {
                health -= collision.collider.GetComponent<Boom>().damage;
                StartCoroutine(Invincible_Timer());
                if (health <= 0) EnemyDies();
            }
            if (collision.collider.gameObject.tag == "Sword")
            {
                health -= collision.collider.GetComponent<I_am_a_Sword>().damage;
                Vector2 dir = collision.collider.transform.position - transform.position;
                dir = -dir.normalized;
                GetComponent<Rigidbody2D>().AddForce(dir * 4000);
                StartCoroutine(Invincible_Timer()); 
                if (health <= 0) EnemyDies();
            }
            if (collision.collider.gameObject.tag == "Arrow")
            {
                collision.collider.GetComponent<I_am_an_Arrow>().Stop_Flight();
                health -= collision.collider.GetComponent<I_am_an_Arrow>().damage;
                Vector2 dir = collision.collider.transform.position - transform.position;
                dir = -dir.normalized;
                GetComponent<Rigidbody2D>().AddForce(dir * 2000);
                StartCoroutine(Invincible_Timer());
                if (health <= 0) EnemyDies();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Range")
        {
            _inRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Range") _inRange = false;
    }

    private void EnemyDies()
    {

    }

    IEnumerator Invincible_Timer()
    {
        _invincibilityTimer = GameManager.GAME.InvicibibleTime;
        myFace.GetComponent<Animator>().SetBool("Flash", true);
        for (float _c = _invincibilityTimer; _c > 0f; _c -= GameManager.GAME.InvincibleRateOfDecay) yield return new WaitForSeconds(1f);
        myFace.GetComponent<Animator>().SetBool("Flash", false);
        _invincibilityTimer = 0;
    }

    IEnumerator MonsterCharges()
    {
        _delay = delayBetweenShots;
        yield return new WaitForSeconds(delayBetweenShots);
        _charging = true;
        //Play Charging Sound
        yield return new WaitForSeconds(1);
        _charging = false;
        yield return new WaitForSeconds(delayBetweenShots);
        _delay = 0;
    }
    IEnumerator MonsterShoots()
    {
        _delay = delayBetweenShots;
        int _rock_index = 0;
        for (int _i = 0; _i < GameManager.GAME.ArrowPool.Count; _i++) if (!GameManager.GAME.RockPool[_i].GetComponent<I_am_an_Arrow>().inFlight) _rock_index = _i;
        GameManager.GAME.RockPool[_rock_index].transform.position = transform.position;
        GameManager.GAME.RockPool[_rock_index].transform.rotation = transform.rotation;
        GameManager.GAME.RockPool[_rock_index].GetComponent<I_am_an_Arrow>().Start_Flight();
        //PLAY FIRE Rock SOUND
        yield return new WaitForSeconds(delayBetweenShots);
        _delay = 0;
    }
}
