using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//stores last active button in scene
public class LastActiveButton : MonoBehaviour {
    public static GameObject lastActiveButton;
    void Start() {
        lastActiveButton = EventSystem.current.currentSelectedGameObject;
    }
    
    void Update() {
        GameObject temp = EventSystem.current.currentSelectedGameObject;
        if ( temp != null) {
            lastActiveButton = temp;
        }
        
    }
}
