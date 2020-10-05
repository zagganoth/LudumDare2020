using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITextController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI loop;
    [SerializeField]
    TextMeshProUGUI score;
    [SerializeField]
    TextMeshProUGUI coins;
    [SerializeField]
    TextMeshProUGUI multiplier;
    [SerializeField]
    RawImage[] lives;
    [SerializeField]
    RawImage[] yellowLives;
    [SerializeField]
    RawImage redLife;
    [SerializeField]
    TextMeshProUGUI goals;
    [SerializeField]
    Image deadPanel;

    Vector3 origPosition;
    // Start is called before the first frame update
    void Start()
    {

        //t = GetComponent<TextMeshProUGUI>();
        //origPosition = t.rectTransform.position;
        RegenText();
        GameManager.instance.onCollect += printProgressText;
        GameManager.instance.onLevelIncrease += printProgressText;
        GameManager.instance.onRespawn += printProgressText;
        GameManager.instance.onDie += showDeadPanel;
    }

    private void showDeadPanel(object sender, EventArgs e)
    {
        deadPanel.gameObject.SetActive(true);
        StopCoroutine(clearDeadTint());
        StartCoroutine(clearDeadTint());
    }

    private void RegenText()
    {
        loop.text = "LOOP: " + GameManager.instance.currentLevel;
        score.text = GameManager.instance.score.ToString();
        multiplier.text = GameManager.instance.scoreMultiplier.ToString("#.0");
        coins.text = GameManager.instance.coinsCollected.ToString();
        if(GameManager.instance.currentLives <= 1)
        {
            redLife.enabled = true;
            yellowLives[0].enabled = false;
            yellowLives[1].enabled = false;
            lives[0].enabled = false;
            lives[1].enabled = false;
            lives[2].enabled = false;
        }
        else if (GameManager.instance.currentLives == 2)
        {
            redLife.enabled = false;
            yellowLives[0].enabled = true;
            yellowLives[1].enabled = true;
            lives[0].enabled = false;
            lives[1].enabled = false;
            lives[2].enabled = false;
        }
        else if (GameManager.instance.currentLives == 3)
        {
            redLife.enabled = false;
            yellowLives[0].enabled = false;
            yellowLives[1].enabled = false;
            lives[0].enabled = true;
            lives[1].enabled = true;
            lives[2].enabled = true;
        }
        /*t.text = "Lives: " + GameManager.instance.currentLives;
        t.text += "\nLoop: " + GameManager.instance.currentLevel;
        t.text += ", Score: " + GameManager.instance.score;*/
        goals.text = "";
        string tempGoals = "";
        foreach (var cond in GameManager.instance.currentGoal.winConditions)
        {
            if (cond.cond.hidden) continue;
            tempGoals += " " + cond.GenerateDescription();
        }
        if(GameManager.instance.currentGoal.complete())
        {
            goals.text += "All goals complete!";
        }
        else
        {
            goals.text += tempGoals;
        }
        

        /*
        t.text += "\n" + "Coins Collected: " + GameManager.instance.coinsCollected;
        t.text += " (" + "Score Multiplier: " + GameManager.instance.scoreMultiplier.ToString("#.00") + ")";*/
    }
    private void printProgressText(object sender, EventArgs e)
    {
        RegenText();
    }
    
    private IEnumerator clearDeadTint()
    {

        yield return new WaitForSeconds(5f);
        deadPanel.gameObject.SetActive(false);
        //t.text = "Hello World!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
