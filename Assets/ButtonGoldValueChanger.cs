using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGoldValueChanger : MonoBehaviour {
    private GameObject SwordManButton;
    private GameObject SpearManButton;
    private GameObject ArcherButton;
    private GameObject AxeManButton;
    private GameObject CrossbowManButton;
    private GameObject BerserkerButton;

    private void Awake() {
        SwordManButton = GameObject.FindWithTag("SwordsMan");
        SpearManButton = GameObject.FindWithTag("SpearMan");
        ArcherButton = GameObject.FindWithTag("Archer");
        AxeManButton = GameObject.FindWithTag("AxeMan");
        CrossbowManButton = GameObject.FindWithTag("CrossbowMan");
        BerserkerButton = GameObject.FindWithTag("Berserker");
    }

    
    void Start() {
        SwordManButton.GetComponent<Text>().text =UnitProperties.SwordsManGoldValue.ToString();
        SpearManButton.GetComponent<Text>().text = UnitProperties.SpearManGoldValue.ToString();
        ArcherButton.GetComponent<Text>().text =UnitProperties.ArcherGoldValue.ToString();
        AxeManButton.GetComponent<Text>().text = UnitProperties.AxeManGoldValue.ToString();
        CrossbowManButton.GetComponent<Text>().text = UnitProperties.CrossbowManGoldValue.ToString();
        BerserkerButton.GetComponent<Text>().text = UnitProperties.BerserkerGoldValue.ToString();
    }
    
}
