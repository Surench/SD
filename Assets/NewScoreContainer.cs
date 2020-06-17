using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScoreContainer : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro ScoreText;

    private void Start()
    {
        ScoreText.text = "+" + GameManager.self.scoreManager.newScore.ToString();

        transform.position = GameManager.self.scoreManager.prevStair.transform.position;

        Destroy(gameObject, 3);
    }
}
