using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    private int maxHealth;
    private BasicCreature creature;
    private Transform bar;
    private Color normalColor;
    private Color injuredColor;
    

    void Start()
    {
        creature = transform.parent.gameObject.GetComponentInChildren<BasicCreature>();
        bar = transform.Find("Bar");
        maxHealth = creature.MaxHealth;
        creature.OnHealthChanged += CreatureOnOnHealthChanged;
        normalColor = new Color(0f,1f,0f,0.6f);
        injuredColor = new Color(1f,0f,0f,0.6f);
        
    }

    private void CreatureOnOnHealthChanged(object sender, EventArgs e) {
        float percentage = creature.GetHealthPercent();
        if (percentage < 0.5f) {
            bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = injuredColor;
        }
        
        bar.localScale= new Vector3(percentage, 1);
    }


 
    void Update() {
       
    }
}
