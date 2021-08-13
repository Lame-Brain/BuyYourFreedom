using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_Bomb : MonoBehaviour
{
    public bool isArmed;
    public float fuseLength;
    public float damage;
    public GameObject Splosion_Prefab;

    int _c = 0;
    public void Arm_Bomb()
    {
        isArmed = true;
        _c = 0;
        FuseTimer();
    }

    public void Disarm_Bomb()
    {
        isArmed = false;
        transform.position = GameManager.POOL.position;
    }

    IEnumerator FuseTimer1sec()
    {
        yield return new WaitForSeconds(1f);
        FuseTimer();
    }
    private void FuseTimer()
    {
        StartCoroutine(FuseTimer1sec());
        if(!GameManager.PAUSED) _c++;

        if (_c > fuseLength)
        {
            GameObject _go;
            if (isArmed)
            {
                _go = Instantiate(Splosion_Prefab, transform.position, Quaternion.identity);
                _go.GetComponent<Boom>().damage = damage;
            }
            Disarm_Bomb();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Splosion")
        {
            GameObject _go = Instantiate(Splosion_Prefab, transform.position, Quaternion.identity);
            _go.GetComponent<Boom>().damage = damage;
            Disarm_Bomb();
        }
    }
}
