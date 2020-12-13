using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGoldValueChanger : MonoBehaviour {
    private GameObject SwordManButtonGold;
    private GameObject SpearManButtonGold;
    private GameObject ArcherButtonGold;
    private GameObject AxeManButtonGold;
    private GameObject CrossbowManButtonGold;
    private GameObject BerserkerButtonGold;
    private GameObject ShieldManButtonGold;
    private GameObject MageButtonGold;

    private void Awake() {
        SwordManButtonGold = GameObject.FindWithTag("SwordsMan");
        SpearManButtonGold = GameObject.FindWithTag("SpearMan");
        ArcherButtonGold = GameObject.FindWithTag("Archer");
        AxeManButtonGold = GameObject.FindWithTag("AxeMan");
        CrossbowManButtonGold = GameObject.FindWithTag("CrossbowMan");
        BerserkerButtonGold = GameObject.FindWithTag("Berserker");
        ShieldManButtonGold = GameObject.FindWithTag("ShieldMan");
        MageButtonGold = GameObject.FindWithTag("Mage");
    }

    
    void Start() {
        SwordManButtonGold.GetComponent<Text>().text =UnitProperties.SwordsManGoldValue.ToString();
        SpearManButtonGold.GetComponent<Text>().text = UnitProperties.SpearManGoldValue.ToString();
        ArcherButtonGold.GetComponent<Text>().text =UnitProperties.ArcherGoldValue.ToString();
        AxeManButtonGold.GetComponent<Text>().text = UnitProperties.AxeManGoldValue.ToString();
        CrossbowManButtonGold.GetComponent<Text>().text = UnitProperties.CrossbowManGoldValue.ToString();
        BerserkerButtonGold.GetComponent<Text>().text = UnitProperties.BerserkerGoldValue.ToString();
        ShieldManButtonGold.GetComponent<Text>().text = UnitProperties.ShieldManGoldValue.ToString();
        MageButtonGold.GetComponent<Text>().text = UnitProperties.MageGoldValue.ToString();
    }
    
}
