using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{


    [SerializeField] Gradient[] StairsGradients;
    [SerializeField] public Color[] CameraColors;

    [SerializeField] public Material[] CharacterMaterials;
    [SerializeField] public Material[] ActiveStairs;
    [SerializeField] public Material[] BrokenStairs;

    [SerializeField] public Material[] MovingStairs;



    //[SerializeField] Gradient[] CameraGradients;

    public static int ColorIndex = 0;

    public void InitColor()
    {
        int currentLevel = LevelManager.currentLevel;
        int maxColorIndex = StairsGradients.Length;

        if (currentLevel < maxColorIndex)
        {
            ColorIndex = currentLevel;
        }
        else
        {
            float division = (float)currentLevel * 1f / maxColorIndex * 1f;
            int totalMax = (int)Mathf.Floor(division) * (maxColorIndex);

            ColorIndex = currentLevel - (totalMax);

        }

    }

    public Color GetStairColor(int i)
    {
        float stairColorPercent = (1f / (GameManager.self.sceneManager.StairsAmount * 1f)) * (i * 1f);

        Color color = StairsGradients[ColorIndex].Evaluate(stairColorPercent);

        return color;
    }




    /*
    public Color GetCameraGradient(int stairIndex)
    {
        float stairColorPercent = (1f / (GameManager.self.sceneManager.StairsAmount * 1f)) * (stairIndex * 1f);

        Color color = CameraGradients[ColorIndex].Evaluate(stairColorPercent);

        return color;
    }
    */
}
