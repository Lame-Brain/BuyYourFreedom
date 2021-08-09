using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_GUI : MonoBehaviour
{
    public GameObject Hud;
    public bool ShowHUD;

    void Update()
    {
        Hud.SetActive(ShowHUD);
        if (ShowHUD)
        {

        }
    }
}
