using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pref_Shuttle : MonoBehaviour
{
    public static Pref_Shuttle PREF;
    public bool Tutorials;
    public int MyPoints;

    // Start is called before the first frame update
    void Start()
    {
        if (PREF == null)
        {
            PREF = gameObject.GetComponent<Pref_Shuttle>();
            DontDestroyOnLoad(gameObject);
            Tutorials = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
