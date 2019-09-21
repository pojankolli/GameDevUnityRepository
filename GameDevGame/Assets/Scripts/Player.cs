﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Stored global variables
    public static int HP=100;
    public static int monies=0;
    public static int wave=1;

    public Text hpText;
    public Text waveText;
    public Text moniesText;
    
    // Update is called once per frame
    void Update()
    {
        //TODO: check current HP, if 0 end game
        //TODO: check monies
        //TODO: update text on UI (maybe necessary to make canvas a child of player gameobject)

        UpdateTextBoxes();
    }

    void UpdateTextBoxes()
    {
        hpText.text = "Planet HP: " + HP;
        waveText.text = "Wave: " + wave;
        moniesText.text = "Monies: " + monies;
    }
}
