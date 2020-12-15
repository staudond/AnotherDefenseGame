using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopUp : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private float DISAPPEAR_TIMER_MAX = Properties.textPopUpDisappearTimer;
    private Vector3 moveVector;
    
    public static TextPopUp Create(Vector3 pos, string text,Color color) {
        Transform textPopUpTransform = Instantiate(GameAssets.i.textPopUp,pos,Quaternion.identity);
        
        TextPopUp textPopUp =  textPopUpTransform.GetComponent<TextPopUp>();
        textPopUp.Setup(text,color);
        return textPopUp;
    }

    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Setup(string text,Color color) {
        textMesh.SetText(text);
        textMesh.color = color;
        textColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;
        
        moveVector = new Vector3(0f,1)*3f;
    }
    private void Setup(string text) {
        textMesh.SetText(text);
        textColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;
        
        moveVector = new Vector3(0f,1)*3f;
    }
    
    private void Update() {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * (8f * Time.deltaTime);
        // if (disappearTimer >= DISAPPEAR_TIMER_MAX * .5f) {
        //     //first half of popup lifetime 
        //     float increaseScaleAmount = 1f;
        //     transform.localScale += Vector3.one * (increaseScaleAmount * Time.deltaTime);
        // } else {
        //     //second half of popup lifetime
        //     float decreaseScaleAmount = 1f;
        //     transform.localScale -= Vector3.one * (decreaseScaleAmount * Time.deltaTime);
        //
        //     
        // }
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

