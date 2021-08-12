using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_PowerUp : MonoBehaviour
{
    public int health, armor, gold, points, arrows, bombs;

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        //Play powerup sound

        GameManager.HEALTH += health;
        GameManager.ARMOR += armor;
        GameManager.GOLD += (gold * 50);
        GameManager.POINTS += points;
        GameManager.ARROWS += arrows;
        GameManager.BOMBS += bombs;

        Destroy(gameObject);
    }
}
