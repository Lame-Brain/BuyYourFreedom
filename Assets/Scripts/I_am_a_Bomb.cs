using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_Bomb : MonoBehaviour
{
    public bool isArmed;
    public float fuseLength;
    public float damage;
    public GameObject Splosion_Prefab;

    public void Arm_Bomb()
    {
        isArmed = true;
        StartCoroutine(FuseTimer(fuseLength));
    }

    public void Disarm_Bomb()
    {
        isArmed = false;
        transform.position = GameManager.POOL.position;
    }

    IEnumerator FuseTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject _go;
        if (isArmed)
        {
            _go = Instantiate(Splosion_Prefab, transform.position, Quaternion.identity);
            _go.GetComponent<Boom>().damage = damage;
        }
        Disarm_Bomb();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit by " + collision.collider.gameObject.tag);
        if(collision.collider.gameObject.tag == "Splosion")
        {
            GameObject _go = Instantiate(Splosion_Prefab, transform.position, Quaternion.identity);
            _go.GetComponent<Boom>().damage = damage;
            Disarm_Bomb();
        }
    }
}
