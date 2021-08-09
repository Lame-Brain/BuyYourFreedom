using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GAME;
    public static float HEALTH, ARMOR, ARROWS, BOMBS, GOLD, POINTS;
    public static bool PAUSED;

    private void Awake()
    {
        GAME = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}

