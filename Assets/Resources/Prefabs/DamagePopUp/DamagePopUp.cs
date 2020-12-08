using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour {

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private float DISAPPEAR_TIMER_MAX = Properties.popUpDisappearTimer;
    private Vector3 moveVector;
    
    public static DamagePopUp Create(Vector3 pos, int dmg) {
        Transform damagePopUpTransform = Instantiate(GameAssets.i.damagePopUp,pos,Quaternion.identity);
        
       DamagePopUp damagePopUp =  damagePopUpTransform.GetComponent<DamagePopUp>();
        damagePopUp.Setup(dmg);
        return damagePopUp;
    }

    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Setup(int dmg) {
        textMesh.SetText(dmg.ToString());
        textColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;
        
        moveVector = new Vector3(0f,1)*3f;
    }
    
    private void Update() {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * (8f * Time.deltaTime);
        if (disappearTimer >= DISAPPEAR_TIMER_MAX * .5f) {
            //first half of popup lifetime 
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * (increaseScaleAmount * Time.deltaTime);
        } else {
            //second half of popup lifetime
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * (decreaseScaleAmount * Time.deltaTime);
        
            
        }
        disappearTimer -= Time.deltaTime;
        if (disappearTimer <= 0) {
            //start disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed;
            textMesh.color = textColor;
            if (textColor.a <= 0) {
                //popup not visible
                Destroy(gameObject);
            }
        }
    }
}
