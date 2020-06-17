using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] Text[] scoretexts;
    [SerializeField] GameObject NewScorePrefab;

    public int Score;
    int RawCounter;

    public void InitScore()
    {
        RawCounter = 0;

        ShowScore();
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public void ResetRawCounter()
    {
        RawCounter = 0;
    }

    public int newScore;
    public StairController prevStair;
    public void AddScore(StairController prevStair_)
    {
        prevStair = prevStair_;

        RawCounter += 1;

        newScore = RawCounter;

        Score += newScore;

        ShowScore(prevStair);
    }


    void ShowScore(StairController prevStair = null)
    {


        foreach (Text text in scoretexts)
        {
            text.text = Score.ToString();
        }

        if(prevStair != null)
        {
           Instantiate(NewScorePrefab);
        }

    }

}
