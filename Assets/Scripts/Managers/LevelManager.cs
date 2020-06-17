using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSettings
{
    public int currentLevel = 0;
}

public class LevelManager : MonoBehaviour
{
    public static int currentLevel;

    [SerializeField] private Slider levelSlider;
    [SerializeField] private Text currentLevelText;
    [SerializeField] private Text nextLevelText;

    [SerializeField] private Text LevelFailedText;
    [SerializeField] private Text LevelCompletedText;

    [SerializeField] GameObject LevelPassedParticles;

    public void InitLevel()
    {
        currentLevel = DataManager.GetLevelSettings().currentLevel;

        levelSlider.value = 0;
        currentLevelText.text = (currentLevel + 1).ToString();
        nextLevelText.text = (currentLevel + 2).ToString();

        CalculateLevel();

        passedStairsAmount = 0;

        LevelPassedParticles.SetActive(false);
    }



    void CalculateLevel()
    {
        
    }

    int passedStairsAmount = 0;
    public void StairCompleted()
    {
        passedStairsAmount += 1;

        levelSlider.value = (passedStairsAmount * 1f) / (GameManager.self.sceneManager.StairsAmount * 1f);
    }

    

    public void LevelPassed()
    {

        LevelPassedParticles.SetActive(true);

        LevelSettings levelSettings = DataManager.GetLevelSettings();
        levelSettings.currentLevel += 1;
        DataManager.SetLevelSettings(levelSettings);

        LevelCompletedText.text = "Level " + (currentLevel + 1).ToString() + "\nCompleted\nScore: " + GameManager.self.scoreManager.Score.ToString();
    }


    public void LevelFailed()
    {
        LevelFailedText.text = "Try Again\n"+ ((int)((passedStairsAmount * 100) / GameManager.self.sceneManager.StairsAmount)).ToString() + "% Completed\nScore: " + GameManager.self.scoreManager.Score.ToString();
    }

}
