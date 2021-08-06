using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text score;
    public float scoreAmount;
    public float pointIncreasePerSecond;

    void Start()
    {
        scoreAmount = 0f;
        pointIncreasePerSecond = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        score.text = scoreAmount.ToString("0");
        scoreAmount += pointIncreasePerSecond * Time.deltaTime;

        if (scoreAmount > PlayerPrefs.GetFloat("highscore"))
        {
            PlayerPrefs.SetFloat("highscore", scoreAmount);
        }
    }
}
