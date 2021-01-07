using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectButtonsShortcutHandler : MonoBehaviour
{
    private GameManager manager;
    private GameObject SwordManButton;
    private GameObject SpearManButton;
    private GameObject ArcherButton;
    private GameObject AxeManButton;
    private GameObject CrossbowManButton;
    private GameObject BerserkerButton;
    private GameObject ShieldManButton;
    private GameObject MageButton;
    // Start is called before the first frame update
    void Start()
    {
        manager = gameObject.GetComponent<GameManager>();
        SwordManButton = GameObject.FindWithTag("SwordsMan");
        SpearManButton = GameObject.FindWithTag("SpearMan");
        ArcherButton = GameObject.FindWithTag("Archer");
        AxeManButton = GameObject.FindWithTag("AxeMan");
        CrossbowManButton = GameObject.FindWithTag("CrossbowMan");
        BerserkerButton = GameObject.FindWithTag("Berserker");
        ShieldManButton = GameObject.FindWithTag("ShieldMan");
        MageButton = GameObject.FindWithTag("Mage");
    }

    // Update is called once per frame
    void Update() {
        SelectUnitToSpawnByKeyboard();
    }
    private void SelectUnitToSpawnByKeyboard() {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (manager.PlayerGold >= UnitProperties.SpearManGoldValue) {
                manager.SelectUnitToSpawn("SpearMan");
                EventSystem.current.SetSelectedGameObject(SpearManButton);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            if (manager.PlayerGold >= UnitProperties.SwordsManGoldValue) {
                manager.SelectUnitToSpawn("SwordsMan");
                EventSystem.current.SetSelectedGameObject(SwordManButton);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            if (manager.PlayerGold >= UnitProperties.ArcherGoldValue) {
                manager.SelectUnitToSpawn("Archer");
                EventSystem.current.SetSelectedGameObject(ArcherButton);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            if (manager.PlayerGold >= UnitProperties.AxeManGoldValue) {
                manager.SelectUnitToSpawn("AxeMan");
                EventSystem.current.SetSelectedGameObject(AxeManButton);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            if (manager.PlayerGold >= UnitProperties.CrossbowManGoldValue) {
                manager.SelectUnitToSpawn("CrossbowMan");
                EventSystem.current.SetSelectedGameObject(CrossbowManButton);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            if (manager.PlayerGold >= UnitProperties.BerserkerGoldValue) {
                manager.SelectUnitToSpawn("Berserker");
                EventSystem.current.SetSelectedGameObject(BerserkerButton);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            if (manager.PlayerGold >= UnitProperties.ShieldManGoldValue) {
                manager.SelectUnitToSpawn("ShieldMan");
                EventSystem.current.SetSelectedGameObject(ShieldManButton);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            if (manager.PlayerGold >= UnitProperties.MageGoldValue) {
                manager.SelectUnitToSpawn("Mage");
                EventSystem.current.SetSelectedGameObject(MageButton);
            }
        }
    }
}
