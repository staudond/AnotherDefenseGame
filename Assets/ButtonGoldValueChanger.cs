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
    private GameObject ShieldManButton;
    private GameObject MageButton;
    

    private void Awake() {
        SwordManButton = GameObject.FindWithTag("SwordsMan");
        SpearManButton = GameObject.FindWithTag("SpearMan");
        ArcherButton = GameObject.FindWithTag("Archer");
        AxeManButton = GameObject.FindWithTag("AxeMan");
        CrossbowManButton = GameObject.FindWithTag("CrossbowMan");
        BerserkerButton = GameObject.FindWithTag("Berserker");
        ShieldManButton = GameObject.FindWithTag("ShieldMan");
        MageButton = GameObject.FindWithTag("Mage");
    }

    
    void Start() {
        SwordManButton.transform.Find("Gold").GetComponent<Text>().text =UnitProperties.SwordsManGoldValue.ToString();
        SpearManButton.transform.Find("Gold").GetComponent<Text>().text = UnitProperties.SpearManGoldValue.ToString();
        ArcherButton.transform.Find("Gold").GetComponent<Text>().text =UnitProperties.ArcherGoldValue.ToString();
        AxeManButton.transform.Find("Gold").GetComponent<Text>().text = UnitProperties.AxeManGoldValue.ToString();
        CrossbowManButton.transform.Find("Gold").GetComponent<Text>().text = UnitProperties.CrossbowManGoldValue.ToString();
        BerserkerButton.transform.Find("Gold").GetComponent<Text>().text = UnitProperties.BerserkerGoldValue.ToString();
        ShieldManButton.transform.Find("Gold").GetComponent<Text>().text = UnitProperties.ShieldManGoldValue.ToString();
        MageButton.transform.Find("Gold").GetComponent<Text>().text = UnitProperties.MageGoldValue.ToString();
    }
}
