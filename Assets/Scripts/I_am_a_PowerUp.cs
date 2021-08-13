using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_PowerUp : MonoBehaviour
{
    public int health, armor, gold, points, arrows, bombs;

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        //Play powerup sound

        if(health > 0) 
        {
            GameManager.HEALTH += health;
            GameManager.GAME.InfoTextPop(transform.position, "+" + health, Color.red);
        }
        if(armor > 0)
        {
            GameManager.ARMOR += armor;
            GameManager.GAME.InfoTextPop(transform.position, "+" + armor, Color.cyan);
        }
        if(gold > 0) 
        {
            GameManager.GOLD += gold;
            GameManager.GAME.InfoTextPop(transform.position, "+" + gold, Color.yellow);
        }
        if(points > 0)
        {
            GameManager.POINTS += points;
            GameManager.GAME.InfoTextPop(transform.position, "+" + points, Color.grey);
        }
        if(arrows > 0) 
        {
            GameManager.ARROWS += arrows;
            GameManager.GAME.InfoTextPop(transform.position, "+" + arrows, Color.green);
        }
        if(bombs > 0)
        {
            GameManager.BOMBS += bombs;
            GameManager.GAME.InfoTextPop(transform.position, "+" + bombs, Color.black);
        }

        Destroy(gameObject);
    }
}
