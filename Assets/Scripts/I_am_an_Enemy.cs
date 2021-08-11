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

    Rigidbody2D _rigidBody;
    float _invincibilityTimer;

    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("IT = " + _invincibilityTimer + " and I hit " + collision.collider.gameObject.name);
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
}
