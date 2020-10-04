using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextController : MonoBehaviour
{
    TextMeshProUGUI t;
    Vector3 origPosition;
    // Start is called before the first frame update
    void Start()
    {

        t = GetComponent<TextMeshProUGUI>();
        origPosition = t.rectTransform.position;
        RegenText();
        GameManager.instance.onCollect += printProgressText;
        GameManager.instance.onLevelIncrease += printProgressText;
        GameManager.instance.onRespawn += printProgressText;
    }
    private void RegenText()
    {
        t.text = "Level: " + GameManager.instance.currentLevel;
        t.text += ", Score: " + GameManager.instance.score;
        foreach (var cond in GameManager.instance.currentGoal.winConditions)
        {
            if (cond.cond.hidden) continue;
            t.text += "\n" + cond.GenerateDescription();
        }
        if(GameManager.instance.currentGoal.complete())
        {
            t.text += "\n" + "All goals complete!";
        }
        t.text += "\n" + "Coins Collected: " + GameManager.instance.coinsCollected;
        t.text += " (" + "Score Multiplier: " + GameManager.instance.scoreMultiplier.ToString("#.00") + ")";
    }
    private void printProgressText(object sender, EventArgs e)
    {
        RegenText();
    }
    
    private IEnumerator waitForText()
    {
        yield return new WaitForSeconds(5f);
        t.text = "Hello World!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
