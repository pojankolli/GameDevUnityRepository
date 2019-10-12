﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Stored global variables
    public static int HP=10;
    public static int monies=100;
    public static int wave=1;

    public Text hpText;
    public Text waveText;
    public Text moniesText;
    
    void Update()
    {
        //TODO: check current HP, if 0 end game

        UpdateTextBoxes();
    }

    void UpdateTextBoxes()
    {
        hpText.text = "Planet HP: " + HP;
        waveText.text = "Wave: " + (wave-1);
        moniesText.text = "Money: " + monies;
    }
}
