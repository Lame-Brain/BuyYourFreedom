using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_GUI : MonoBehaviour
{
    public GameObject Hud;
    public bool ShowHUD;
    public TMPro.TextMeshProUGUI Health, Armor, Arrows, Bombs, Points, Gold, Timer, enemies_left;

    void Update()
    {
        Hud.SetActive(ShowHUD);
        if (ShowHUD)
        {
            Health.text = GameManager.HEALTH.ToString();
            Armor.text = GameManager.ARMOR.ToString();
            Arrows.text = GameManager.ARROWS.ToString();
            Bombs.text = GameManager.BOMBS.ToString();
            Gold.text = GameManager.GOLD.ToString();
            Points.text = GameManager.POINTS.ToString();
            Timer.text = GameManager.SECONDS_LEFT.ToString();
            enemies_left.text = GameManager.GAME.MonsterPoolObject.childCount.ToString();
        }
    }
}
