using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;
    public GameObject resetButton;
    public GameObject getLivesButton;
    public GameObject rewardButton;
    public Text txtLives;
    public float timeRemaining = 15;
    public bool timerIsRunning = false;
    public Text timeText;
    public GameObject vicText;
    public GameObject noLivesText;
    private bool _noAds = false;
    public GameObject noAdsButton;
    
    private int _cntLive;
    private float _score = 0;
    

    private void Awake()
    {
        Instance = this;
    }
    public void RemoveAds()
    {
        _noAds = true;
        noAdsButton.SetActive(false);
       
    }

    public void Reset()
    {
        SceneManager.LoadScene("Game");
    }

    // Start is called before the first frame update
    private void Start()
    {
        _cntLive = 2;
        timerIsRunning = true;
        
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                var app = Firebase.FirebaseApp.DefaultInstance;
              
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    // Update is called once per frame
    private void Update()
    {
        if (!timerIsRunning) return;
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            RemoveLives();
        }
    }



    public void ScoreKeep()
    {
        _score++;
        if (_score >= 4)
        {
            Victory();
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    private async Task Victory()
    {
        timerIsRunning = false;
        timeRemaining = 15;
        await Task.Delay(1000);
        vicText.SetActive(true);
        resetButton.SetActive(true);
        rewardButton.SetActive(true);
        noAdsButton.SetActive(true);
    }

    public void RewardLives()
    {
        timerIsRunning = true;
        timeRemaining = 30;
        _cntLive++;
        txtLives.text = "Lives: " + _cntLive;
        noLivesText.SetActive(false);
        getLivesButton.SetActive(false);
    }

    private void RemoveLives()
    {
        timerIsRunning = true;
        timeRemaining = 15;
        _cntLive--;
        if (_cntLive == 0)
        {
            timerIsRunning = false;
            noLivesText.SetActive(true);
            getLivesButton.SetActive(true);
        }
        else
        {
            txtLives.text = "Lives: " + _cntLive;
            timerIsRunning = true;
            timeRemaining = 15;
        }
    }
}