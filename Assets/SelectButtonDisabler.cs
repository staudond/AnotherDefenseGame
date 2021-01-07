using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;


public class SelectButtonDisabler : MonoBehaviour {
    private GameManager manager;
    private int gold;
    private Color defaultColor;
    private Text goldtext;
    void Start() {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        goldtext = transform.Find("Gold").GetComponent<Text>();
        gold = int.Parse(goldtext.text);
        defaultColor = goldtext.color;
    }

    // Update is called once per frame
    void Update() {
        int playerGold = manager.PlayerGold;
        if (playerGold < gold) {
            gameObject.GetComponent<Button>().interactable = false;
            goldtext.color = Color.red;
        }
        else {
            gameObject.GetComponent<Button>().interactable = true;
            goldtext.color = defaultColor;
        }
    }
}
