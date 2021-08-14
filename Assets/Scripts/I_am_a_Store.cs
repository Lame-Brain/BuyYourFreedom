using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_Store : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Selected, Prices;
    public int selected, HP_UP, AP_UP, Arrows, Bombs, Swrd_Up, Arrw_Up, Bmbs_Up;
    
    private float _inflation, _goldSpent;
    bool _button = false;

    public void InitStore()
    {
        selected = 0;
        CalculateInflation();
        CalculatePrices();
        CalculateSelected();
    }

    public void CalculateSelected()
    {
        if (selected == 0) Selected.text = ">\n\n\n\n\n\n\n\n\n\n";
        if (selected == 1) Selected.text = "\n>\n\n\n\n\n\n\n\n";
        if (selected == 2) Selected.text = "\n\n>\n\n\n\n\n\n\n";
        if (selected == 3) Selected.text = "\n\n\n>\n\n\n\n\n\n";
        if (selected == 4) Selected.text = "\n\n\n\n>\n\n\n\n\n";
        if (selected == 5) Selected.text = "\n\n\n\n\n>\n\n\n\n";
        if (selected == 6) Selected.text = "\n\n\n\n\n\n>\n\n\n";
        if (selected == 7) Selected.text = "\n\n\n\n\n\n\n>\n\n";
        if (selected == 8) Selected.text = "\n\n\n\n\n\n\n\n>\n";
        if (selected == 9) Selected.text = "\n\n\n\n\n\n\n\n\n>";
    }

    public void CalculateInflation()
    {
        _inflation = Random.Range((_goldSpent / 999), (_goldSpent / 1001));
    }

    public void CalculatePrices()
    {
        Prices.text = "Gain Health (" + (HP_UP + (int)_inflation) + ")\n" +
             "Repair Armor (" + (AP_UP + (int)_inflation) + ")\n" +
             "Buy some Arrows (" + (Arrows + (int)_inflation) + ")\n" +
             "Buy some Bombs (" + (Bombs + (int)_inflation) + ")\n" +
             "Improve Sword (" + (Swrd_Up + (int)_inflation) + ")\n" +
             "Improve Arrows (" + (Arrw_Up + (int)_inflation) + ")\n" +
             "Improve Bombs (" + (Bmbs_Up + (int)_inflation) + ")\n" +
             "Gold -> Points (" + GameManager.GOLD + ")\n" +
             "Buy your Freedom! (" + GameManager.FREEDOM + ")\n" +
             "Leave Store";
    }

    private void Update()
    {
        //CONTROLS
        if (!GameManager.PAUSED)
        {

            //Get axis Input
            if (Input.GetAxisRaw("Vertical") > 0 && !_button)
            {
                selected--;
                if (selected < 0) selected = 9;
                CalculateSelected();
                _button = true;
            }
            if (Input.GetAxisRaw("Vertical") < 0 && !_button)
            {
                selected++;
                if (selected > 9) selected = 0;
                CalculateSelected();
                _button = true;
            }

            if (Input.GetAxisRaw("Vertical") == 0) _button = false;

            //Get Button Input
            if (Input.GetButtonUp("Cancel"))
            {
                GameManager.GAME.PopQuitMenu();
            }

            if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2"))
            {                
                if(selected == 0 && GameManager.GOLD >= (HP_UP + (int)_inflation))
                {
                    GameManager.GOLD -= (HP_UP + (int)_inflation);
                    _goldSpent += (HP_UP + (int)_inflation);
                    GameManager.HEALTH += 10;
                    CalculatePrices();
                    //Play Success Sound
                }
                //if (selected == 0 && GameManager.GOLD < (HP_UP + (int)_inflation)) Play Failure Sound

                if(selected == 1 && GameManager.GOLD >= (AP_UP + (int)_inflation))
                {
                    GameManager.GOLD -= (AP_UP + (int)_inflation);
                    _goldSpent += (AP_UP + (int)_inflation);
                    GameManager.ARMOR += 1;
                    CalculatePrices();
                    //Play Success Sound
                }
                //if (selected == 1 && GameManager.GOLD < (AP_UP + (int)_inflation)) Play Failure Sound

                if (selected == 2 && GameManager.GOLD >= (Arrows + (int)_inflation))
                {
                    GameManager.GOLD -= (Arrows + (int)_inflation);
                    _goldSpent += (Arrows + (int)_inflation);
                    GameManager.ARROWS += 5;
                    CalculatePrices();
                    //Play Success Sound
                }
                //if (selected == 2 && GameManager.GOLD < (Arrows + (int)_inflation)) Play Failure Sound

                if (selected == 3 && GameManager.GOLD >= (Bombs + (int)_inflation))
                {
                    GameManager.GOLD -= (Bombs + (int)_inflation);
                    _goldSpent += (Bombs + (int)_inflation);
                    GameManager.BOMBS++;
                    CalculatePrices();
                    //Play Success Sound
                }
                //if (selected == 3 && GameManager.GOLD < (Bombs + (int)_inflation)) Play Failure Sound

                if (selected == 4 && GameManager.GOLD >= (Swrd_Up + (int)_inflation))
                {
                    GameManager.GOLD -= (Swrd_Up + (int)_inflation);
                    _goldSpent += (Swrd_Up + (int)_inflation);
                    GameManager.GAME.sword_bonus++;
                    CalculatePrices();
                    //Play Success Sound
                }
                //if (selected == 4 && GameManager.GOLD < (Swrd_Up + (int)_inflation)) Play Failure Sound

                if (selected == 5 && GameManager.GOLD >= (Arrw_Up + (int)_inflation))
                {
                    GameManager.GOLD -= (Arrw_Up + (int)_inflation);
                    _goldSpent += (Arrw_Up + (int)_inflation);
                    GameManager.GAME.arrow_bonus++;
                    CalculatePrices();
                    //Play Success Sound
                }
                //if (selected == 5 && GameManager.GOLD < (Arrw_Up + (int)_inflation)) Play Failure Sound

                if (selected == 6 && GameManager.GOLD >= (Bmbs_Up + (int)_inflation))
                {
                    GameManager.GOLD -= (Bmbs_Up + (int)_inflation);
                    _goldSpent += (Bmbs_Up + (int)_inflation);
                    GameManager.GAME.bomb_bonus++;
                    CalculatePrices();
                    //Play Success Sound
                }
                //if (selected == 6 && GameManager.GOLD < (Bmbs_Up + (int)_inflation)) Play Failure Sound

                if (selected == 7 && GameManager.GOLD > 0)
                {
                    GameManager.POINTS += GameManager.GOLD * 10;
                    GameManager.GOLD = 0;
                    _goldSpent -= 1000;
                    if (_goldSpent < 0) _goldSpent = 0;
                    CalculatePrices();                    
                    //play success sound
                }
                //if(selected == 7 && GameManager.GOLD <= 0) Play Failure Sound

                if (selected == 8 && GameManager.GOLD >= GameManager.FREEDOM)
                {
                    //Win Game
                }
                //if(selected == 7 && GameManager.GOLD < GameManager.FREEDOM) Play Failure Sound

                if(selected == 9)
                {
                    GameManager.SECONDS_LEFT = 1;
                }
            }
        }
    }
}
