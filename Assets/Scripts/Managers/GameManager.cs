using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameAnalyticsSDK;
using Facebook.Unity;

public class GameManager : MonoBehaviour
{
    public static GameManager self;

    public SceneManager sceneManager;
    public PlayerController playerController;
    public CameraController cameraController;
    public ColorManager colorManager;
    public LevelManager levelManager;
    public SoundManager soundManager;
    public ScoreManager scoreManager;

    public static int GameStatus = 0;

    public static bool TapticEnabled = true;


    [SerializeField] GameObject GameCanvas;
    [SerializeField] GameObject MenuPanel;
    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject LevelPassedPanel;

    [SerializeField] Text MenuLevelNum;

    [SerializeField] Text[] BestMenuScore;


    void Awake()
    {
        Application.targetFrameRate = 60;
        self = this;


        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }



    private void Start()
    {
        GameAnalytics.Initialize();

        scoreManager.ResetScore();
        OpenMenu();
    }

    public void OpenMenu()
    {
        Init();

        foreach (Text text in BestMenuScore)
        {
            text.text = "BEST: "+ DataManager.GetBestScore().ToString();
        }

        MenuPanel.SetActive(true);
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        LevelPassedPanel.SetActive(false);
    }

    private void Init()
    {
        MenuLevelNum.text = "Level " + (DataManager.GetLevelSettings().currentLevel+1).ToString();

        GameStatus = 0;

        DeleteObjects();
        levelManager.InitLevel();
        colorManager.InitColor();
        sceneManager.InitScene();
        playerController.InitPlayer();
        cameraController.Init();
        soundManager.InitSoundManager();
        scoreManager.InitScore();
    }


    public void StartGame()
    {
        GameStatus = 1;

        MenuPanel.SetActive(false);
        GamePanel.SetActive(true);
        GameOverPanel.SetActive(false);
        LevelPassedPanel.SetActive(false);

        cameraController.StartCamera();

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, Application.version,LevelManager.currentLevel);

    }



    public void StopGame()
    {
        GameStatus = 2;
        soundManager.StopSound();
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        GamePanel.SetActive(false);

        levelManager.LevelFailed();

        if(scoreManager.Score > DataManager.GetBestScore())
        {
            DataManager.SetBestScore(scoreManager.Score);
        }

#if UNITY_IOS
       if (TapticEnabled)
        {
            TapticEngine.TriggerError();
        }
#endif
        

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, Application.version, LevelManager.currentLevel);


        scoreManager.ResetScore();
    }

    public void LevelPassed()
    {
        soundManager.StopSound();

        GameStatus = 3;

        GamePanel.SetActive(false);
        LevelPassedPanel.SetActive(true);

        levelManager.LevelPassed();

        if (scoreManager.Score > DataManager.GetBestScore())
        {
            DataManager.SetBestScore(scoreManager.Score);
        }

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, Application.version, LevelManager.currentLevel);
    }

    void DeleteObjects()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("stair");

        foreach(GameObject obj in objs)
        {
            Destroy(obj);
        }
    }


    [SerializeField] GameObject tapticLinesImg;

    public void ToggleTaptic()
    {
        TapticEnabled = !TapticEnabled;
        tapticLinesImg.SetActive(TapticEnabled);
    }
}
