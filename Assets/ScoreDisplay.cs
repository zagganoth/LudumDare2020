using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    TextMeshProUGUI t;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<TextMeshProUGUI>();
        t.text = "Score: " + GameManager.instance.score;
        Destroy(GameManager.instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
