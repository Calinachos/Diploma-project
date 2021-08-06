using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{

    public Text HStext;

    // Start is called before the first frame update
    void Start()
    {
        HStext.text = PlayerPrefs.GetFloat("highscore").ToString("0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
