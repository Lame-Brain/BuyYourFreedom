using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_an_Dragon : MonoBehaviour
{
    public GameObject myFace;
    public float health;
    public float speed;
    public float damage;
    public float delayBetweenShots;
    public int min_hearts, max_hearts, min_shields, max_shields, min_coins, max_coins, min_bags, max_bags, min_points, max_points, min_Arrows, max_Arrows, min_Bombs, max_Bombs;
    public GameObject Grave_Prefab, Poof_Prefab, Fire_Breath;

    Rigidbody2D _rigidBody;
    GameObject _Player;
    float _invincibilityTimer;
    Transform _Target;
    //Type behavior variables
    bool _inRange;
    bool _charging;
    float _delay;
    bool _shooting;

    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, _Player.transform.position) < 2.5f)
        {  _inRange = true; } else { _inRange = false; }

        if (!GameManager.PAUSED)
        {
            if (health > 0)
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_invincibilityTimer == 0)
        {
            if (collision.collider.gameObject.tag == "Splosion")
            {
                Enemy_takes_damage(collision.collider.GetComponent<Boom>().damage + GameManager.GAME.bomb_bonus);
                StartCoroutine(Invincible_Timer());
            }
            if (collision.collider.gameObject.tag == "Sword")
            {
                Enemy_takes_damage(collision.collider.GetComponent<I_am_a_Sword>().damage + GameManager.GAME.sword_bonus);
                Vector2 dir = collision.collider.transform.position - transform.position;
                dir = -dir.normalized;
                StartCoroutine(Invincible_Timer());
                if (health > 0)
                {
                    _rigidBody.AddForce(dir * 3000);
                }
            }
            if (collision.collider.gameObject.tag == "Arrow")
            {
                collision.collider.GetComponent<I_am_an_Arrow>().Stop_Flight();
                Enemy_takes_damage(collision.collider.GetComponent<I_am_an_Arrow>().damage + GameManager.GAME.arrow_bonus);
                Vector2 dir = collision.collider.transform.position - transform.position;
                dir = -dir.normalized;
                GetComponent<Rigidbody2D>().AddForce(dir * 2000);
                StartCoroutine(Invincible_Timer());                
            }
        }
    }
    private void Enemy_takes_damage(float _d)
    {
        health -= _d;
        if (health <= 0) EnemyDies();
        GameManager.GAME.LittleTextPop(transform.position, "-" + _d);
        //Play enemy hit sound
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Range") _inRange = false;
    }

    private void EnemyDies()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = 0;
        GameObject _go = Instantiate(Grave_Prefab, transform.position, Quaternion.identity);
        int _hrts = Random.Range(min_hearts, max_hearts); if (_hrts < 0) _hrts = 0;
        int _shlds = Random.Range(min_shields, max_shields); if (_shlds < 0) _shlds = 0;
        int _coins = Random.Range(min_coins, max_coins); if (_coins < 0) _coins = 0;
        int _bags = Random.Range(min_bags, max_bags); if (_bags < 0) _bags = 0;
        int _scale = Random.Range(min_points, max_points); if (_scale < 0) _scale = 0;
        int _arrws = Random.Range(min_Arrows, max_Arrows); if (_arrws < 0) _arrws = 0;
        int _bmbs = Random.Range(min_Bombs, max_Bombs); if (_bmbs < 0) _bmbs = 0;
        _go = Instantiate(Grave_Prefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        _go.GetComponent<I_am_a_grave>().num_Hearts = _hrts;
        _go.GetComponent<I_am_a_grave>().num_Shields = _shlds;
        _go.GetComponent<I_am_a_grave>().num_Coins = _coins;
        _go.GetComponent<I_am_a_grave>().num_Bags = _bags;
        _go.GetComponent<I_am_a_grave>().bag_Scale = _scale;
        _go.GetComponent<I_am_a_grave>().num_Arrows = _arrws;
        _go.GetComponent<I_am_a_grave>().num_Bombs = _bmbs;

        Instantiate(Poof_Prefab, transform.position, Quaternion.identity);
        myFace.GetComponent<Animator>().SetBool("Dead", true);
        StartCoroutine(WaitForDeath());
    }

    IEnumerator Invincible_Timer()
    {
        _invincibilityTimer = GameManager.GAME.InvicibibleTime;
        myFace.GetComponent<Animator>().SetBool("Flash", true);
        for (float _c = _invincibilityTimer; _c > 0f; _c -= GameManager.GAME.InvincibleRateOfDecay) yield return new WaitForSeconds(1f);
        if (myFace != null) myFace.GetComponent<Animator>().SetBool("Flash", false);
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
        Fire_Breath.SetActive(true);
        //PLAY FIRE Rock SOUND
        yield return new WaitForSeconds(delayBetweenShots);
        _delay = 0;
    }

    IEnumerator WaitForDeath()
    {
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.angularVelocity = 0f;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject.transform.parent.gameObject);
    }
}
