using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_an_Arrow : MonoBehaviour
{
    public bool inFlight = false;
    public float flightTime;
    public float speed;
    public float damage;

    int _c = 0;

    // Update is called once per frame
    void Update()
    {
        if (inFlight)
        {
           if(!GameManager.PAUSED) transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }

    public void Start_Flight()
    {
        inFlight = true;
        _c = 0;
        StartCoroutine(FlightTimer());
    }

    public void Stop_Flight()
    {
        inFlight = false;
        transform.position = GameManager.POOL.position;
    }

    IEnumerator FlightTimer()
    {
        yield return new WaitForSeconds(1f);
        waitforflighttime();
    }

    private void waitforflighttime()
    {
        if (!GameManager.PAUSED) _c++;
        StartCoroutine(FlightTimer());
        if (_c > flightTime) Stop_Flight();
    }
}
