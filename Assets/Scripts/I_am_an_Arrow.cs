using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_an_Arrow : MonoBehaviour
{
    public bool inFlight = false;
    public float flightTime;
    public float speed;
    public float damage;

    // Update is called once per frame
    void Update()
    {
        if (inFlight)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }

    public void Start_Flight()
    {
        inFlight = true;
        StartCoroutine(FlightTimer(flightTime));
    }

    public void Stop_Flight()
    {
        inFlight = false;
        transform.position = GameManager.POOL.position;
    }

    IEnumerator FlightTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        Stop_Flight();
    }
}
