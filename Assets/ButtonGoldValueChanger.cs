using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGoldValueChanger : MonoBehaviour {
    private void Awake() {
        GameObject.FindWithTag("SwordsMan").transform.Find("Gold").GetComponent<Text>().text =UnitProperties.SwordsManGoldValue.ToString();
        GameObject.FindWithTag("SpearMan").transform.Find("Gold").GetComponent<Text>().text = UnitProperties.SpearManGoldValue.ToString();
        GameObject.FindWithTag("Archer").transform.Find("Gold").GetComponent<Text>().text =UnitProperties.ArcherGoldValue.ToString();
        GameObject.FindWithTag("AxeMan").transform.Find("Gold").GetComponent<Text>().text = UnitProperties.AxeManGoldValue.ToString();
        GameObject.FindWithTag("CrossbowMan").transform.Find("Gold").GetComponent<Text>().text = UnitProperties.CrossbowManGoldValue.ToString();
        GameObject.FindWithTag("Berserker").transform.Find("Gold").GetComponent<Text>().text = UnitProperties.BerserkerGoldValue.ToString();
        GameObject.FindWithTag("ShieldMan").transform.Find("Gold").GetComponent<Text>().text = UnitProperties.ShieldManGoldValue.ToString();
        GameObject.FindWithTag("Mage").transform.Find("Gold").GetComponent<Text>().text = UnitProperties.MageGoldValue.ToString();
    }
}
